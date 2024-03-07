using Microsoft.AspNetCore.Mvc;
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
        public List<Restaurant> GetResults(List<bool> filters)
        {
            List<Restaurant> restaurants = db.Restaurants.ToList();
            List<Restaurant> results = new();

            foreach (var restaurant in restaurants)
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

                results.Add(restaurant);
            }

            return results;
        }


        [NonAction]
        public List<Restaurant> GetResults(string searchKey)
        {
            
            List<Restaurant> restaurants = db.Restaurants.ToList();

            List<KeyValuePair<double, Restaurant>> scores = new();
            searchKey = searchKey.ToLower();
            foreach (var restaurant in restaurants)
            {   

                double score = 0;
                string name = restaurant.Name.ToLower();

                foreach (var character in searchKey)
                {
                    if (name.Contains(character))
                    {
                        score++;
                    }
                }

                scores.Add(new(score, restaurant));
            }

            scores.Sort(Compare);
            List<Restaurant> results = scores.Select(p => p.Value).Reverse().ToList();
            return results;
        }

        [NonAction]
        static int Compare(KeyValuePair<double, Restaurant> a, KeyValuePair<double, Restaurant> b)
        {
            return b.Key.CompareTo(a.Key);
        }
    }
}
