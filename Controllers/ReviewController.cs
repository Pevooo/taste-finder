using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using TasteFinder.Chatbot;
using TasteFinder.Models;
using TasteFinder.ViewModels;


namespace TasteFinder.Controllers
{
    [Authorize(Policy = "RequireUser")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ReviewController : Controller
    {
        
        private readonly Gemini chatBot;
        private readonly TasteFinderContext db;
        private readonly double TrustScoreConstant = 2;

        public ReviewController(TasteFinderContext db)
        {
            chatBot = new();
            this.db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(string id, int stars, string comment)  
        {

            if (await chatBot.CheckNonOffensive(comment)||comment==null)
            {
                Review review = new();
                review.Date = DateTime.Now;
                review.Text = comment;
                review.Stars = stars;

                var user = db.Users.SingleOrDefault(user => user.Email == User.Identity.Name);
                review.Author = user;
                var restaurant = db.Restaurants.SingleOrDefault(res => res.Email == id);
                review.Restaurant = restaurant;
                var totalStars = restaurant.TotalStars;
                var score = restaurant.WeightedReviewScore;
                var trustScore = db.Contributions.Where(c => c.Review.Author == user && c.Up == true).Count() - db.Contributions.Where(c => c.Review.Author == user && c.Up == false).Count();
                var weight = GetWeight(trustScore);
                restaurant.WeightedReviewScore += 5 * weight;
                restaurant.TotalStars += review.Stars * weight;
                db.Reviews.Add(review);
                db.Update(restaurant);
                db.SaveChanges();
            }
            return Redirect($"/Search/ProfileView?id={id}");
        }

        [HttpGet]
        public void Contribute(string id, string impression)
        {
            var user = db.Users.SingleOrDefault(user => user.Email == User.Identity.Name);
            bool up;
            if (impression == "Up"){
                up = true;
            }else{
                up = false;
            }
            var review = db.Reviews.SingleOrDefault(rev => rev.ReviewId==int.Parse(id));
            if (review.Author == user)
            {
                return;
            }
            var alreadyContributed = db.Contributions.SingleOrDefault(c => c.Author == user && c.Review == review);
            if (alreadyContributed is null)
            {
                var new_contribution = new Contribution()
                {
                    Up = up,
                    Review = review,
                    Author = user
                };
                db.Contributions.Add(new_contribution);
                db.SaveChanges();
            }
            else
            {
                if (up == alreadyContributed.Up)
                {
                    db.Contributions.Remove(alreadyContributed);
                    db.SaveChanges();
                }
                else
                {
                    alreadyContributed.Up = !up;
                    db.SaveChanges();
                }
               
            }
                        
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
    }
}
