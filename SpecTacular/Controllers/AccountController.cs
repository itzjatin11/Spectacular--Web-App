using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpecTacular.Data;
using SpecTacular.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpecTacular.Controllers
{
    [Route("Account/[action]")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AccountController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // ✅ Check if credentials match the Admin user from appsettings.json
                string adminUser = _config["AdminCredentials:Username"];
                string adminPass = _config["AdminCredentials:Password"];

                if (model.EmailOrUsername == adminUser && model.Password == adminPass)
                {
                    HttpContext.Session.SetString("UserRole", "Admin");
                    HttpContext.Session.SetString("UserName", adminUser); // Store Admin's username
                    return RedirectToAction("Dashboard", "Admin");
                }

                // 🔹 Check if user exists in the database (login with either Username or Email)
                var user = await _context.Users
                    .FirstOrDefaultAsync(u =>
                        (u.Name == model.EmailOrUsername || u.Email == model.EmailOrUsername) &&
                        u.PasswordHash == model.Password
                    );

                if (user == null)
                {
                    // 🔴 Add an error if the username or password is incorrect
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }

                // ✅ Set session for normal user
                HttpContext.Session.SetString("UserRole", "User");
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserName", user.Name); // Store User's name

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                // _logger.LogError(ex, "Error occurred during login");

                // Add a generic error message to display on the view
                ViewData["ErrorMessage"] = "An error occurred while processing your login. Please try again later.";

                // Return the view with the model and the error message
                return View(model);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // 🔹 Check if email already exists
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(model);
                }

                // 🔹 Create a new user object
                var user = new User
                {
                    Name = model.Username,
                    Email = model.Email,
                    PasswordHash = model.Password // ⚠️ Hash this before saving!
                };

                // 🔹 Save user in database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // ✅ Redirect after successful registration
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                // Optionally log the exception to a logging service
                // _logger.LogError(ex, "Error occurred during registration");

                // Add a generic error message to be displayed on the view
                ModelState.AddModelError("", "An error occurred while processing your registration. Please try again later.");

                // Return the view with the model and the error message
                return View(model);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Checkout()
        {
            // Retrieve cart data from session
            var cartData = HttpContext.Session.GetString("ShoppingCart");

            if (!string.IsNullOrEmpty(cartData))
            {
                var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cartData);

                // Process order logic (save order to database, handle payment, etc.)
                // Example: OrderService.ProcessOrder(cartItems);

                // Clear cart after successful checkout
                HttpContext.Session.Remove("ShoppingCart");
            }

            return RedirectToAction("OrderConfirmation"); // Redirect to an order confirmation page
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}
