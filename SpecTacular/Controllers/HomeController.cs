using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SpecTacular.Models;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

namespace SpecTacular.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }




        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(string Name = "", string Email = "", string Message = "")
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Message))
                {
                    ViewBag.ErrorMessage = "All fields are required.";
                    return View();
                }

                // Email format validation
                if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    ViewBag.ErrorMessage = "Invalid email address format.";
                    return View();
                }

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("jatintaadiyal@gmail.com"), // ✅ YOUR email, not the user's
                    Subject = "New Contact Form Submission",
                    Body = $"Name: {Name}\nEmail: {Email}\nMessage: {Message}",
                    IsBodyHtml = false
                };
                mail.To.Add("jatintaadiyal@gmail.com");  // ✅ Receiving email
                mail.ReplyToList.Add(new MailAddress(Email)); // ✅ User's email as reply-to

                // SMTP Configuration
                SmtpClient smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("jatintaadiyal@gmail.com", "gqdv dwaa ltus xvtw"), // 🔴 Use an App Password!
                    EnableSsl = true
                };

                smtp.Send(mail);
                ViewBag.Message = "Thank you! Your message has been sent.";
            }
            catch (SmtpException smtpEx)
            {
                ViewBag.ErrorMessage = "SMTP Error: " + smtpEx.Message;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error: " + ex.Message;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Promotion()
        {
            return RedirectToAction("Index", "Promotions");
        }


        public IActionResult Cart()
        {
            string userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var cartData = HttpContext.Session.GetString("ShoppingCart");

            // ✅ Log the session data to debug
            Console.WriteLine("Cart Data Retrieved: " + cartData);

            List<CartItem> cartItems = string.IsNullOrEmpty(cartData)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartData);

            return View(cartItems);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
