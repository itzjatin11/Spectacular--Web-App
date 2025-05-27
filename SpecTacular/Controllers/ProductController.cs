using Microsoft.AspNetCore.Mvc;
using SpecTacular.Data;
using SpecTacular.Models;
using System.Linq;

namespace SpecTacular.Controllers
{
    [Route("Shop")]  // Makes the URL start with /Shop
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Default Shop Page (Shows all products)
        [HttpGet("")]
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // Filter products by category (Eyeglass, Sunglass, Screenglass)
        [HttpGet("{category}")]
        public IActionResult Category(string category)
        {
            var products = _context.Products.Where(p => p.Category == category).ToList();

            if (!products.Any())
            {
                return NotFound("No products found in this category.");
            }

            // Determine the corresponding view file based on the category
            string viewName = category switch
            {
                "Eyeglass" => "Eyeglass",
                "Sunglass" => "Sunglass",
                "ScreenGlass" => "ScreenGlass",
                _ => "Index" // Default to Index if no match found
            };

            return View($"~/Views/Product/{viewName}.cshtml", products);
        }

    }
}
