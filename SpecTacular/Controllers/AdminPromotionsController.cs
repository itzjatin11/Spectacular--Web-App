using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpecTacular.Data;
using SpecTacular.Models;

namespace SpecTacular.Controllers
{
 
    public class AdminPromotionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminPromotionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Display form to create a new promotion
        public IActionResult Create()
        {
            return View();
        }

        // POST: Save new promotion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                promotion.IsActive = true; // Default to active
                _context.Add(promotion);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Promotion added successfully!";
                return RedirectToAction("Dashboard", "Admin");
            }
            return View(promotion);
        }

        // GET: Display form to edit a promotion
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion == null) return NotFound();
            return View(promotion);
        }

        // POST: Save edited promotion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Promotion promotion)
        {
            if (id != promotion.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(promotion);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Promotion updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Promotions.Any(e => e.Id == id)) return NotFound();
                    throw;
                }
                return RedirectToAction("Dashboard", "Admin");
            }
            return View(promotion);
        }

        // GET: Display confirmation to delete a promotion
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion == null) return NotFound();
            return View(promotion);
        }

        // POST: Confirm deletion of a promotion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion != null)
            {
                _context.Promotions.Remove(promotion);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Promotion deleted successfully!";
            }
            return RedirectToAction("Dashboard", "Admin");
        }
    }
}