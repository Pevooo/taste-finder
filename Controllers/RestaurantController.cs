using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasteFinder.Models;
using TasteFinder.Views.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.SignalR.Protocol;


namespace TasteFinder.Controllers
{
    [Authorize(Policy = "RequireRestaurant")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RestaurantController : Controller
    {
        private readonly TasteFinderContext db;
        public MemoryStream MemoryStream = new();
        public RestaurantController(TasteFinderContext db)
        {
            this.db = db;
        }
        public IActionResult Profile()
        {
            // get information of the current Login user
            var currentLogin = User.Identity.Name;
            var current = db.Restaurants.Include(r => r.Photos).SingleOrDefault(s => s.Email == currentLogin);
            var RestaurantReviews = db.Reviews.Where(rev => rev.Restaurant == current).OrderByDescending(rev => rev.Author.Contributions.Count()).ToList();
            RestaurantProfile restaurantProfile = new RestaurantProfile();
            restaurantProfile.Restaurant = current;
            restaurantProfile.Reviews = RestaurantReviews;
            restaurantProfile.PhotoPaths = new List<string>();
            var tmp = current.Photos.OrderByDescending(c => c.Date);
            int i = 0;
            foreach (var p in tmp)
            {
                if (i == 5) break;
                restaurantProfile.PhotoPaths.Add(string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(p.PhotoData)));
                i++;
            }
            // if user not found logout
            if (current == null)
            {
                RedirectToAction("Logout", "Account");
            }
            // else return user information to user profile
            return View(restaurantProfile);
        }
        [HttpGet]
        public IActionResult Edit()
        {
            var currentLogin = User.Identity.Name;
            var current = db.Restaurants.SingleOrDefault(s => s.Email == currentLogin);
            // if user not found logout
            if (current == null)
            {
                RedirectToAction("Logout", "Account");
            }
            ValidateModel m = new ValidateModel()
            {
                name = current.Name,
                email = current.Email,
                FileType = "image",
                OldPassword = "Asd123456",
                NewPassword = "Asd123456",
                ConfirmPassword = "Asd123456",
                phonenumber = current.PhoneNumber
            };
            ViewData["stime"] = current.OpenTime.ToString(@"hh\:mm");
            ViewData["etime"] = current.CloseTime.ToString(@"hh\:mm");

            ViewData["KidsArea"] = current.KidsArea;
            ViewData["OpenAir"] = current.OpenAir;
            ViewData["Seats"] = current.Seats;
            ViewData["Food"] = current.Food;
            ViewData["Drinks"] = current.Drinks;
            ViewData["Desserts"] = current.Desserts;
            ViewData["Delivery"] = current.Delivery;
            ViewData["Vegan"] = current.Vegan;
            ViewData["Healthy"] = current.Healthy;
            ViewData["Buffet"] = current.Buffet;

            return View(m);
        }
        [HttpPost]
        public IActionResult Edit(ValidateModel m, IFormCollection form)
        {
            if (!ModelState.IsValid)
            {
                return View(m);
            }
            var curemail = User.Identity.Name;
            var curruser = db.Restaurants.SingleOrDefault(x => x.Email == curemail);
            if (m.email != curemail)
            {
                var iscan = db.Restaurants.SingleOrDefault(c => c.Email == m.email);
                if (iscan != null)
                {
                    return View(curruser);
                }
            }
            curruser.Name = m.name;
            curruser.Email = m.email;
			curruser.OpenTime = DateTime.Parse(Request.Form["StartTime"]);
			curruser.CloseTime = DateTime.Parse(Request.Form["EndTime"]);
			curruser.PhoneNumber = m.phonenumber;
            curruser.Buffet = form["Buffet"] == "on";
            curruser.Healthy = form["Healthy"] == "on";
            curruser.Vegan = form["Vegan"] == "on";
            curruser.Delivery = form["Delivery"] == "on";
            curruser.Desserts = form["Desserts"] == "on";
            curruser.Drinks = form["Drinks"] == "on";
            curruser.Food = form["Food"] == "on";
            curruser.Seats = form["Seats"] == "on";
            curruser.KidsArea = form["KidsArea"] == "on";
            curruser.OpenAir = form["OpenAir"] == "on";
            db.SaveChanges();
            return RedirectToAction("Profile");

        }
		public IActionResult Remove()
		{
			var curemail = User.Identity.Name;
			var curruser = db.Restaurants.SingleOrDefault(x => x.Email == curemail);
			db.Restaurants.Remove(curruser);
			db.SaveChanges();
			return RedirectToAction("Logout", "Account");
		}
		[HttpPost]
        public IActionResult AddPhotos(IFormCollection fom)
        {
            var currentLogin = User.Identity.Name;
            var current = db.Restaurants.SingleOrDefault(s => s.Email == currentLogin);
            if (!Request.Form.Files[0].ContentType.ToLower().StartsWith("image"))
            {
                return RedirectToAction("Profile");
            }
            Request.Form.Files[0].CopyTo(MemoryStream);
            string n = Request.Form.Files[0].ToString();
            Photo restaurantPhoto = new()
            {
                PhotoData = MemoryStream.ToArray(),
                Date = DateTime.Now,
                Owner = current
            };
            db.Photos.Add(restaurantPhoto);
            db.SaveChanges();
            return RedirectToAction("Profile");
        }
    }
}
