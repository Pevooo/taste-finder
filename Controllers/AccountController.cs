using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TasteFinder.Models;
using System;


namespace TasteFinder.Controllers
{
	//public enum RestaurantFeature
	//{
	//	Buffet,
	//	Healthy,
	//	Vegan,
	//	Delivery,
	//	Desserts,
	//	Drinks,
	//	Food,
	//	Seats,
	//	KidsArea,
	//	OpenAir
	//}
	public class AccountController : Controller
	{
		private readonly TasteFinderContext db;
		MemoryStream MemoryStream { get; set; }
		public AccountController(TasteFinderContext db)
		{
			this.db = db;
			MemoryStream = new();
		}


		public IActionResult LoginHome()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
		}


		[HttpPost]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult LoginVerfiy()
		{
			string email = Request.Form["Email"];
			var userResult = db.Users.SingleOrDefault(user => user.Email == email);
			var restaurantResult = db.Restaurants.SingleOrDefault(restaurant => restaurant.Email == email);
			string Password = Request.Form["Password"];
			if (userResult != null && BCrypt.Net.BCrypt.Verify(Password, userResult.Password))
			{
                Authenticate(userResult);
				return RedirectToAction("Index", "Home");
			}
			else if (restaurantResult != null && BCrypt.Net.BCrypt.Verify(Password, restaurantResult.Password))
			{
                Authenticate(restaurantResult);
				return RedirectToAction("Index", "Home");
			}
			else
			{
				return RedirectToAction("LoginHome", "Account");
			}
		}
		private void Authenticate(User user)
		{
            var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.Email),
                    new(ClaimTypes.Role, "User")
                };

            // Create identity
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

			// Create principal
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

			// Sign in the user
			HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
		}
		private void Authenticate(Restaurant restaurant)
		{
			var claims = new List<Claim>
				{
					new(ClaimTypes.Name, restaurant.Email),
					new(ClaimTypes.Role, "Restaurant")
				};

			// Create identity
			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

			// Create principal
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

			// Sign in the user
			HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
		}
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("LoginHome");
		}
		public IActionResult RegisterType()
		{
			return View();
		}
		[HttpGet]
		public IActionResult Userform()
		{
			ValidateModel m = new ValidateModel()
			{
				name = "",
				OldPassword="asd123456",
				NewPassword = "",
				ConfirmPassword = "",
				phonenumber = ""
			};
			return View(m);
		}
		[HttpPost]
		public IActionResult Userform(ValidateModel m)
		{
			if(!ModelState.IsValid)
			{
				return View(m);
			}
			Request.Form.Files[0].CopyTo(MemoryStream);
			
            string n=Request.Form.Files[0].ToString();
			var userResult = db.Users.FirstOrDefault(user => user.Email == m.email);
			if (userResult != null)
			{
				return RedirectToAction("Userform");
			}
			User newUser = new()
			{
				Email = m.email,
				Name = m.name,
				Password = BCrypt.Net.BCrypt.HashPassword(m.ConfirmPassword),
				PhoneNumber = m.phonenumber,
				ProfilePicture = MemoryStream.ToArray()
			};

			db.Users.Add(newUser);
			db.SaveChanges();
			return RedirectToAction("Index", "Home");


		}
		[HttpGet]
		public IActionResult Restaurantform()
		{
			ValidateModel m = new ValidateModel()
			{
				name = "",
				OldPassword = "asd123456",
				NewPassword = "",
				ConfirmPassword = "",
				phonenumber = ""
			};
			return View(m);
		}
		[HttpPost]
		public IActionResult Restaurantform(ValidateModel m,IFormCollection form)
		{
			if (!ModelState.IsValid)
			{
				return View(m);
			}
			string lat = form["lat"];
			string lng = form["lng"];
			Request.Form.Files[0].CopyTo(MemoryStream);
			//var selectedFeatures = new List<string>();
			//foreach (var key in form.Keys)
			//{
			//	if (key.StartsWith("feature_"))  // Check if key starts with "feature_"
			//	{
			//		selectedFeatures.Add(form[key]);
			//	}
			//}
			var item = db.Restaurants.FirstOrDefault(rest => rest.Email == m.email);
			if (item != null)
			{
				ViewBag.found = "An Account with this Email already exists";
				return RedirectToAction("Restaurantform");
			}

			Restaurant newRestaurant = new()
			{
				Email = m.email,
				Name = m.name,
				Password = BCrypt.Net.BCrypt.HashPassword(m.ConfirmPassword),
				PhoneNumber = m.phonenumber,
				OpenTime = DateTime.Parse(Request.Form["StartTime"]),
				CloseTime = DateTime.Parse(Request.Form["EndTime"]),
				Latitude = Convert.ToDouble(lat),
				Longitude = Convert.ToDouble(lng),
				Buffet = form["Buffet"] == "on",
				Healthy = form["Healthy"] == "on",
				Vegan = form["Vegan"] == "on",
				Delivery = form["Delivery"] == "on",
				Desserts = form["Desserts"] == "on",
				Drinks = form["Drinks"] == "on",
				Food = form["Food"] == "on",
				Seats = form["Seats"] == "on",
				KidsArea = form["KidsArea"] == "on",
				OpenAir = form["OpenAir"] == "on",

			};
			//foreach (var feature in selectedFeatures)
			//{
			//	switch (feature)
			//	{
			//		case "Buffet":
			//			newRestaurant.Buffet = true;
			//			break;
			//		case "Healthy":
			//			newRestaurant.Healthy = true;
			//			break;
			//		case "Vegan":
			//			newRestaurant.Vegan = true;
			//			break;
			//		case "Delivery":
			//			newRestaurant.Delivery = true;
			//			break;
			//		case "Desserts":
			//			newRestaurant.Desserts = true;
			//			break;
			//		case "Drinks":
			//			newRestaurant.Drinks = true;
			//			break;
			//		case "Food":
			//			newRestaurant.Food = true;
			//			break;
			//		case "Seats":
			//			newRestaurant.Seats = true;
			//			break;
			//		case "KidsArea":
			//			newRestaurant.KidsArea = true;
			//			break;
			//		case "OpenAir":
			//			newRestaurant.OpenAir = true;
			//			break;
			//	}
			//}
			Photo restaurantPhoto = new()
			{
				PhotoData = MemoryStream.ToArray(),
				Date = DateTime.Now,
				Owner = newRestaurant
			};
			db.Photos.Add(restaurantPhoto);
			db.Restaurants.Add(newRestaurant);
			db.SaveChanges();
			return RedirectToAction("Index", "Home");

		}
		
    }
	
}
