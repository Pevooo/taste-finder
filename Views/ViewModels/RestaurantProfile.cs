using TasteFinder.Models;

namespace TasteFinder.Views.ViewModels
{
    public class RestaurantProfile
    {
        public Restaurant Restaurant { get; set; }
        public List<Review> Reviews { get; set; }
        public List<string> PhotoPaths { get; set; }
    }
}
