using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasteFinder.Chatbot;
using TasteFinder.Models;
using TasteFinder.ViewModels;

namespace TasteFinder.Controllers
{
	[Authorize(Policy = "RequireUser")]
	[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
	public class UserController : Controller
	{
        private readonly TasteFinderContext db;
        MemoryStream MemoryStream { get; set; }
        Gemini chatBot { get; set; }
        public UserController(TasteFinderContext db)
        {
            this.db = db;
            MemoryStream = new();
            chatBot = new();
        }
        public IActionResult userprofile()
		{

            var curemail = User.Identity.Name;
			var user = db.Users.FirstOrDefault(x => x.Email == curemail);
            if (user == null)
            {
                return View("Logout", "Account");
            }
            var userReviews = db.Reviews.Where(rev => rev.Author == user).OrderByDescending(x=>x.Date).Include(rev=>rev.Author).Include(rev=>rev.Restaurant);
            ProfileUser userprofile = new()
            {
                Email = curemail,
                ProfilePicture = user.ProfilePicture,
                photo = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(user.ProfilePicture)),
                Trust = db.Contributions.Where(c => c.Review.Author == user&&c.Up==true).Count()- db.Contributions.Where(c => c.Review.Author == user && c.Up == false).Count(),
                Latitude = user.Latitude,
                Longitude = user.Longitude,
                PhoneNumber = user.PhoneNumber,
                Name = user.Name,
                reviews = userReviews
            };

            return View(userprofile);
		}
        [HttpGet]
        public IActionResult EditUser()
        {
            var curemail = User.Identity.Name;
            var curruser = db.Users.SingleOrDefault(x => x.Email == curemail);
            if (curruser == null)
            {
                RedirectToAction("Logout", "Account");
            }
            ValidateModel m = new ValidateModel()
            {
                name = curruser.Name,
                email = curruser.Email,
                OldPassword = "asd123456",
                NewPassword = "Asd12345678",
                ConfirmPassword = "Asd12345678",
                FileType = "image",
                phonenumber = curruser.PhoneNumber,
            };
            return View(m);
        }
        [HttpPost]
        public IActionResult EditUser(ValidateModel m)
        {
            if (Request.Form.Files.Count != 0)
            {
				if (!Request.Form.Files[0].ContentType.ToLower().StartsWith("image"))
                {
                    return View(m);
                }
				Request.Form.Files[0].CopyTo(MemoryStream);
			}
            
            if (!ModelState.IsValid)
            {
                return View(m);
            }
            var curemail = User.Identity.Name;
            var curruser = db.Users.SingleOrDefault(x => x.Email == curemail);
            if (m.email != curemail)
            {
                var iscan = db.Users.SingleOrDefault(c => c.Email == m.email);
                if (iscan != null)
                {
                    return View(curruser);
                }
            }
            curruser.Email = m.email;
            curruser.PhoneNumber = m.phonenumber;
            if (Request.Form.Files.Count != 0)
            {
                curruser.ProfilePicture = MemoryStream.ToArray();
            }
            curruser.Name = m.name;
            db.SaveChanges();
            return RedirectToAction("userprofile");
        }
        public IActionResult RemoveUser()
        {
            var curemail = User.Identity.Name;
            var curruser = db.Users.SingleOrDefault(x => x.Email == curemail);
            db.Users.Remove(curruser);
            db.SaveChanges();
            return RedirectToAction("Logout", "Account");
        }

        [HttpGet]
        public IActionResult RemoveReview(int id, string? restaurantEmail)
        {
            var review = db.Reviews.SingleOrDefault(r => r.ReviewId == id);

            if (review == null)
            {
                return RedirectToAction("Index", "Home");
            }

            db.Reviews.Remove(review);
            db.SaveChanges();

            if (!string.IsNullOrEmpty(restaurantEmail))
            {
                return RedirectToAction("UserProfile", "User");
            }
            else
            {
                return RedirectToAction("UserProfile", "User");
            }
        }
        [HttpPost]
        public IActionResult RemoveReview(int id)
        {
            var review = db.Reviews.SingleOrDefault(r => r.ReviewId == id);

            if (review == null)
            {
                return RedirectToAction("Index", "Home");
            }

            db.Reviews.Remove(review);
            db.SaveChanges();

            
            return RedirectToAction("UserProfile", "User");
            
        }
        [HttpGet]
        public IActionResult UpdateReview(int id)
        {
            var review = db.Reviews.Include(r => r.Restaurant).SingleOrDefault(x => x.ReviewId == id);
            return View(review);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateReview(int reviewId, string newText)
        {
            var review = db.Reviews.SingleOrDefault(r => r.ReviewId == reviewId);
            if (review != null)
            {
                if (await chatBot.CheckNonOffensive(newText))
                {
                    review.Text = newText;
                    db.Update(review);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("userprofile");
        }

    }
}

