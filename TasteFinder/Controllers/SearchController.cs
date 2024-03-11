using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return View();
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
            List<KeyValuePair<int, Restaurant>> scores = new();
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
                    cache[0, i] = i;
                }

                // Evaluating Values
                for (int i = 1; i <= restaurantLength; i++)
                {
                    for (int j = 1; j <= searchLength; j++)
                    {
                        int add = (restaurantName[i - 1] == searchKey[j - 1]) ? 1 : 0;
                        cache[i, j] = Math.Min(1 + cache[i - 1, j], Math.Min(1 + cache[i, j - 1], add + cache[i - 1, j - 1]));
                    }
                }

                scores.Add(new(cache[restaurantLength, searchLength], restaurant));

            }

            scores.Sort(Compare);
            results = scores.Select(p => p.Value).Reverse().ToList();
            return results;
        }

        [NonAction]
        static int Compare(KeyValuePair<int, Restaurant> a, KeyValuePair<int, Restaurant> b)
        {
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
