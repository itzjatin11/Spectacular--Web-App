using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpecTacular.Data;
using SpecTacular.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SpecTacular.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ApplicationDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ Admin Dashboard - Loads Users & Products, Handles Search via AJAX
        // ✅ Admin Dashboard - Loads Users & Products, Handles Search via AJAX
        // ✅ Admin Dashboard - Loads Users & Products, Handles Search via AJAX

        public IActionResult LiveSearchCustomer(string searchTerm)
        {
            var filteredUsers = _context.Users
                .Where(u => string.IsNullOrEmpty(searchTerm) || u.Name.ToLower().Contains(searchTerm.ToLower()))
                .ToList();

            return PartialView("_UserTablePartial", filteredUsers);
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard(string? searchTerm, string? category, DateTime? dateAdded)
        {
            try
            {
                _logger.LogInformation("Dashboard called with searchTerm: {SearchTerm}, category: {Category}, dateAdded: {DateAdded}",
                    searchTerm, category, dateAdded);

                bool isFilteredSearch = !string.IsNullOrEmpty(searchTerm)
                                        || dateAdded.HasValue
                                        || (!string.IsNullOrEmpty(category) && category != "All Categories");

                if (isFilteredSearch)
                {
                    var productsQuery = _context.Products.AsQueryable();

                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        var loweredTerm = searchTerm.ToLower();
                        productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(loweredTerm));
                    }

                    if (!string.IsNullOrEmpty(category) && category != "All Categories")
                    {
                        productsQuery = productsQuery.Where(p => p.Category.ToLower() == category.ToLower());
                    }

                    if (dateAdded.HasValue)
                    {
                        productsQuery = productsQuery.Where(p => p.DateAdded.Date == dateAdded.Value.Date);
                    }

                    var filteredProducts = await productsQuery.ToListAsync();
                    _logger.LogInformation("Filtered products count: {FilteredCount}", filteredProducts.Count);

                    return Json(filteredProducts);
                }

                // Full page load or "All Categories" selected with no other filters
                var users = await _context.Users.ToListAsync();
                var products = await _context.Products.ToListAsync();

                var promotions = await _context.Promotions.ToListAsync(); // Fetch promotions



                var model = new Tuple<List<User>, List<Product>, List<Promotion>>(users, products, promotions);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error loading dashboard: {Message}", ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: AddCustomer View
        public IActionResult AddCustomer()
        {
            return View(new User()); // Ensure the model is empty
        }

        // POST: AddCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCustomer(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(user);
                }

                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View(user);
        }

        // ✅ GET: Edit User Page
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return View(user); // Pass the user with the existing password
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(User model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users.FindAsync(model.Id);
            if (user == null) return NotFound();

            user.Name = model.Name;
            user.Email = model.Email;
            user.PasswordHash = model.PasswordHash; // Directly storing without hashing

            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

        // ✅ DELETE: User
        [HttpGet]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return NotFound();
        }

        public IActionResult Index()
        {
            return View();
        }

        // ✅ GET: All Products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            if (!products.Any()) return Json(new { message = "No products found" });
            return Json(products);
        }

        // ✅ GET: Edit Product Page
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View("EditProduct", product); // Ensure "EditProduct.cshtml" exists
        }

        // ✅ POST: Save Edited Product Data
        [HttpPost]
        public async Task<IActionResult> EditProduct(Product model)
        {
            if (!ModelState.IsValid) return View(model);

            var product = await _context.Products.FindAsync(model.Id);
            if (product == null) return NotFound();

            product.Name = model.Name;
            product.Price = model.Price;
            product.ImageUrl = model.ImageUrl;
            product.Category = model.Category;

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        // ✅ GET: Display the Add Product form
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        // ✅ POST: Save product (Image URL is already set from AJAX upload)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Error: Please fill in all required fields correctly.";
                return View(product);
            }

            if (string.IsNullOrEmpty(product.ImageUrl))
            {
                TempData["ErrorMessage"] = "Error: Please upload an image before submitting.";
                return View(product);
            }

            try
            {
                // Add product to database
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Product added successfully!";
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: Failed to add product. " + ex.Message;
                return View(product);
            }
        }

        // ✅ POST: Handle AJAX Image Upload
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                // Ensure "wwwroot/images" directory exists
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique file name
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return relative path (to be stored in ImageUrl field)
                return Ok("/images/" + uniqueFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error uploading image: " + ex.Message);
            }
        }


    }
}