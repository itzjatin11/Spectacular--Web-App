using Microsoft.AspNetCore.Mvc;
using SpecTacular.Data;
using SpecTacular.Models;
using System;
using System.Linq;

namespace SpecTacular.Controllers
{
    public class PromotionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PromotionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var promotions = _context.Promotions.ToList();
            return View(promotions);
        }

        // New action to fetch active promotions as JSON
        [HttpGet]
        public IActionResult GetActivePromotions()
        {
            var currentDate = DateTime.Now;
            var activePromotions = _context.Promotions
                .Where(p => p.IsActive &&
                            (p.ValidUntil == null || p.ValidUntil >= currentDate))
                .OrderByDescending(p => p.DiscountPercentage) // Prioritize higher discounts
                .ToList();

            return Json(activePromotions);
        }
    }
}