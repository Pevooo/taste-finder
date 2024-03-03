using ChatAIze.GenerativeCS.Models;
using ChatAIze.GenerativeCS.Options.Gemini;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.Json;
using TasteFinder.Chatbot;
using TasteFinder.Models;


namespace TasteFinder.Controllers
{
    public class ChatbotController : Controller
    {
        private readonly TasteFinderContext db;
        
        private readonly Gemini chatBot;

        private readonly ChatConversation chatConversation;
        public ChatbotController(TasteFinderContext db)
        {
            this.db = db;
            var options = new ChatCompletionOptions();
            options.AddFunction(Gotosearch);
            options.AddFunction(Gotohome);
            options.AddFunction(Logout);
            options.AddFunction(GoToProfile);
            options.AddFunction(RespondToText);
            options.AddFunction(GoToLogin);
            options.AddFunction(Register);
            options.AddFunction(SearchFor);
            chatBot = new Gemini(options);
            chatConversation = new ChatConversation();
        }
        [HttpGet]
        public IActionResult Chatbot2()
        {
            ViewBag.n1 = "hi";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Chatbot2(string n1)
        {
            chatConversation.FromUser(n1);
            string res = await chatBot.Getresponse(chatConversation);
            ViewBag.res = res;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetResponseAJAX(string text)
        {
            chatConversation.FromUser(text);

            Console.WriteLine(chatConversation.Messages.Count);
            string res = await chatBot.Getresponse(chatConversation);
            return Json(new { text = res }, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            });
        }

        [Description("Goes to search page")]
        public void Gotosearch()
        {
            Response.Redirect("/Search/Index");
        }
        public void Gotohome()
        {
            Response.Redirect("/Home/Index");
        }
        public void Logout()
        {

            Response.Redirect("/Account/logout");
        }
        public void GoToProfile()
        {
            string email = User.Identity.Name;
            var user = db.Users.FirstOrDefault(x => x.Email == email);
            if (user != null)
            {
                Response.Redirect("/User/userprofile");
            }
            else
            {
                Response.Redirect("/Restaurant/Profile");
            }

        }

        [Description("Redirects the user to the registration page depending on the type of the account")]
        public string Register(string type)
        {
            if (type.ToLower() == "user")
            {
                Response.Redirect("/Account/Userform");
            }
            else if (type.ToLower() == "restaurant")
            {
                Response.Redirect("/Account/Restaurantform");
            }
            return "Can't Create an account for this type. You can select either user or restaurant";
        }
        public void GoToLogin()
        {
            Response.Redirect("/Account/LoginHome");
        }

        [Description("Responds to Greetings, Questions, Text that do not require going to any page")]
        public void RespondToText()
        {
            return;
        }

        [Description("Searches for a specific restaurant")]
        public void SearchFor(string restaurant)
        {
            Response.Redirect($"/Search/Results?Search={restaurant}");
        }
    }
}
