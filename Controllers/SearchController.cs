using Microsoft.AspNetCore.Mvc;
using TasteFinder.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using TasteFinder.ViewModels;
using TasteFinder.Chatbot;
using Microsoft.EntityFrameworkCore;

namespace TasteFinder.Controllers
{
    
    public class SearchController : Controller
    {
        private const double searchThreshold = 1;
        private const double keywordThreshold = 0.6;
        private readonly TasteFinderContext db;
        private readonly int TrustScoreConstant = 2;
        private readonly Gemini chatBot = new Gemini();
        public SearchController(TasteFinderContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var topRatedRestaurants = GetTopRatedRestaurants();
            return View(topRatedRestaurants);
        }


        [HttpGet]
        public IActionResult Results(string Search, string OpenAir, string KidsArea, string Seats, string Food, string Drinks, string Desserts, string Delivery, string Buffet, string Vegan, string Healthy, string nearMe, string longitude, string latitude, int start = 0)
        {
            // For checkboxes "On" means true and "null" means false
            List<string> query = [OpenAir, KidsArea, Seats, Food, Drinks, Desserts, Delivery, Buffet, Vegan, Healthy];
            List<bool> filters = query.Select(p => true && p is not null).ToList();

            List<Restaurant> SearchResults;
            if (nearMe != "on")
            {
                SearchResults = GetResults(Search, filters);
            }
            else
            {
                try
                {
					SearchResults = GetResults(Search, filters, Convert.ToDouble(latitude), Convert.ToDouble(longitude));
				}
                catch
                {
					SearchResults = GetResults(Search, filters);
				}
				
            }
            
            if (start == 0)
            {
                return View(SearchResults.Take(5));
            }
            else
            {
                if (start + 5 < SearchResults.Count)
                {
                    return Json(SearchResults.GetRange(start, 5), new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        ReferenceHandler = ReferenceHandler.Preserve,
                        WriteIndented = true
                    });
                }
                else if (start < SearchResults.Count)
                {
                    return Json(SearchResults.GetRange(start, SearchResults.Count - start), new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        ReferenceHandler = ReferenceHandler.Preserve,
                        WriteIndented = true
                    });
                }
                else
                {
                    return Json(new List<string>(), new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        ReferenceHandler = ReferenceHandler.Preserve,
                        WriteIndented = true
                    });
                }

            }

        }

        [HttpGet]
        public IActionResult ProfileView(string id)
        {
            var user = db.Users.SingleOrDefault(user => user.Email == User.Identity.Name);
            var restaurant = db.Restaurants.Include(r => r.Photos).SingleOrDefault(rest => rest.Email == id); //Include photo
            //var restaurant = db.Restaurants.SingleOrDefault(rest => rest.Email == id);            
            var reviews = db.Reviews.Where(rev => rev.Restaurant == restaurant).Include(r=>r.Author).ToList();
            var photoPaths = new List<string>();
            var tmp = restaurant.Photos.OrderByDescending(c => c.Date);
            int i = 0;
            foreach (var p in tmp)
            {
                if (i == 5) break;
                photoPaths.Add(string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(p.PhotoData)));
                i++;
            }
            RestaurantandUser model = new()
            {
                User = user,
                Restaurant = new()
                {
                    Restaurant = restaurant,
                    Reviews = reviews,
                    PhotoPaths = photoPaths
                }
            };
            return View(model);
        }

        

        [NonAction]
        public List<Restaurant> GetResults(string searchKey = null, List<bool> filters = null, double? latitude = null, double? longitude = null)
        {
            
            List<Restaurant> restaurants = db.Restaurants.ToList();
            List<Restaurant> results = new();


            // Step 1: Filters (Feature Filters or Location Filter)
            if ((latitude is not null && longitude is not null) || filters is not null) 
            {
                foreach (var restaurant in restaurants)
                {
                    // Feature Filters
                    if (filters is not null)
                    {
                        if (filters[0] && !restaurant.OpenAir) continue;
                        if (filters[1] && !restaurant.KidsArea) continue;
                        if (filters[2] && !restaurant.Seats) continue;
                        if (filters[3] && !restaurant.Food) continue;
                        if (filters[4] && !restaurant.Drinks) continue;
                        if (filters[5] && !restaurant.Desserts) continue;
                        if (filters[6] && !restaurant.Delivery) continue;
                        if (filters[7] && !restaurant.Buffet) continue;
                        if (filters[8] && !restaurant.Vegan) continue;
                        if (filters[9] && !restaurant.Healthy) continue;
                    }

                    // Location Filter
                    if (longitude is not null && latitude is not null)
                    {

                        double lat = restaurant.Latitude;
                        double lon = restaurant.Longitude;

                        double distance = CalculateDistance(lat, lon, (double)latitude, (double)longitude);
                        if (distance > 10000) continue;

                    }


                    results.Add(restaurant);
                }
            }
 


            if (searchKey is null || searchKey == "")
            {
                return results;
            }



            // Step 2: Search (Text search is the last because of it's high complexity)
            restaurants = results;
            List<KeyValuePair<double, Restaurant>> scores = new();
            searchKey = searchKey.ToLower();
            int searchLength = searchKey.Length;
            foreach (var restaurant in restaurants)
            {
                string restaurantName = restaurant.Name.ToLower();
                int restaurantLength = restaurant.Name.Length;
                int[,] cache = new int[restaurantLength + 1, searchLength + 1];

                // Initializing Table
                for (int i = 0; i <= restaurantLength; i++)
                {
                    cache[i,0] = i;
                }

                for (int i = 0; i <= searchLength; i++)
                {
                    cache[0,i] = i;
                }

                // Evaluating Values
                for (int i = 1; i <= restaurantLength; i++)
                {
                    for (int j = 1; j <= searchLength; j++)
                    {
                        int add = (restaurantName[i - 1] == searchKey[j - 1]) ? 0 : 1;
                        cache[i, j] = Math.Min(1 + cache[i - 1, j], Math.Min(1 + cache[i, j - 1], add + cache[i - 1, j - 1]));
                    }
                }


                scores.Add(new(cache[restaurantLength, searchLength] / (double)restaurantLength, restaurant));


            }

            scores.Sort(Compare);

            List<Restaurant> KeywordFound = new();

            foreach (var keyword in db.Possessions)
            {
                if (keyword.Text.ToLower() == searchKey.ToLower())
                {
                    KeywordFound.Add(keyword.Restaurant);
                }
            }
            var keywordRestaurant = KeywordFound.OrderByDescending(res => res.TotalStars / res.WeightedReviewScore).FirstOrDefault();
            

            results = scores.Where((p, i) => scores[i].Key < searchThreshold).Select(p => p.Value).ToList();
            if (keywordRestaurant is not null && scores.Count != 0 && scores[0].Key > keywordThreshold)
            {
                results.Remove(keywordRestaurant);
                results.Insert(0, keywordRestaurant);
            }
            return results;
            
        }

        [NonAction]
        static int Compare(KeyValuePair<double, Restaurant> a, KeyValuePair<double, Restaurant> b)
        {
            if (a.Key == b.Key)
            {
                return a.Value.Name.CompareTo(b.Value.Name);
            }
            return a.Key.CompareTo(b.Key);
        }

        [NonAction]
        private static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        [NonAction]
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return 6371000 * c;
        }

        [NonAction]
        public double GetWeight(int TrustScore)
        {
            if (TrustScore >= 0)
            {
                return Math.Log2(TrustScoreConstant + TrustScore);
            }
            else
            {
                return Math.Log2(2 - (Math.Max(TrustScore, -50) / -50));
            }
        }

        public List<Restaurant> GetTopRatedRestaurants(int topCount = 5)
        {
            return db.Restaurants
                     .OrderByDescending(r => r.TotalStars)
                     .Take(topCount)
                     .ToList();
        }

    }
}
