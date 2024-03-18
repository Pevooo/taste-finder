using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Tracing;
using TasteFinder.Models;

namespace TasteFinder.Controllers
{
    public class SearchController : Controller
    {
        private readonly TasteFinderContext db;
        public SearchController(TasteFinderContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Results(string Search, string OpenAir, string KidsArea, string Seats, string Food, string Drinks, string Desserts, string Delivery, string Buffet, string Vegan, string Healthy, string NearMe)
        {
            // For checkboxes "On" means true and "null" means false
            List<string> query = new() { OpenAir, KidsArea, Seats, Food, Drinks, Desserts, Delivery, Buffet, Vegan, Healthy};
            List<bool> filters = query.Select(p => true ? p is not null : false).ToList();


            List<Restaurant> SearchResults = GetResults(Search, filters);
            return View(SearchResults);
        }

        


        [NonAction]
        public List<Restaurant> GetResults(string searchKey = null, List<bool> filters = null, double? latitude = null, double? longtitude = null)
        {
            
            List<Restaurant> restaurants = db.Restaurants.ToList();
            List<Restaurant> results = new();


            // Step 1: Filters (Feature Filters or Location Filter)
            if ((latitude is not null && longtitude is not null) || filters is not null) 
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
                    if (longtitude is not null && latitude is not null)
                    {

                        double lat = restaurant.Latitude;
                        double lon = restaurant.Longitude;

                        double distance = CalculateDistance(lat, lon, (double)latitude, (double)longtitude);
                        if (distance > 1000) continue;

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
            results = scores.Select(p => p.Value).ToList();

            List<KeyValuePair<double, Restaurant>> points_of_keys = new();
            List<Keyword> keywords = db.Possessions.ToList();
            List<Restaurant> result_keys = new();
            int number_of_different_letters = 0;
            foreach (var  key in keywords) {
                number_of_different_letters = 0;
                for (int i = 0;i<Math.Min(searchLength,key.Text.Length);i++)
                {
                    if (searchKey[i] != key.Text[i])
                    {
                        number_of_different_letters++;
                    }
                }
                points_of_keys.Add(new(number_of_different_letters, key.Restaurant));
            }
            points_of_keys.Sort(Compare);
            result_keys = points_of_keys.Select(p => p.Value).ToList();

            List<KeyValuePair<double, Restaurant>> points_of_res = new();
            int num_of_different_letters = 0;
            foreach (var res in results)
            {
                num_of_different_letters = 0;
                for (int i = 0; i <Math.Min( searchLength,res.Name.Length); i++)
                {
                    if (searchKey[i] != res.Name[i])
                    {
                        number_of_different_letters++;
                    }
                }
                points_of_res.Add(new(num_of_different_letters, res));
            }

            for (int i = 0; i < Math.Min(result_keys.Count, results.Count); i++)
            {
                if (results[i].Email == result_keys[i].Email)
                {
                    result_keys.Remove(result_keys[i]);
                }
            }
                List<Restaurant> result_of_all = new();
            for (int i = 0; i < Math.Min(points_of_keys.Count, points_of_res.Count); i++)
            {
               
                if (points_of_keys[i].Key < points_of_res[i].Key)
                {
                    result_of_all.Add(points_of_keys[i].Value);
                }
                else
                {
                    result_of_all.Add(points_of_res[i].Value);
                }
                
            }
               return result_of_all;
               //return results;
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
    }
}
