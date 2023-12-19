using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.Controllers
{
    public class SavingController : Controller
    {
        private readonly AppDbContext _context;

        public SavingController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Saving
        [Authorize]
        public async Task<IActionResult> Index()
        {
             return _context.Savings != null ? 
                          View(await _context.Savings.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Savings'  is null.");

            ViewBag.Savings = await _context.Savings.ToListAsync();
        }


        // GET: Saving/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Saving/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SavingId,SavingName,Amount,GoalAmount,CreatedAt")] Saving saving)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    saving.UserId = userId;

                    _context.Add(saving);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(saving);
        }

        // GET: Saving/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Savings == null)
            {
                return NotFound();
            }

            var saving = await _context.Savings.FindAsync(id);
            if (saving == null)
            {
                return NotFound();
            }
            return View(saving);
        }


        // POST: Saving/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int amountEnteredByUser)
        {
            var savingsRecord = await _context.Savings.FindAsync(id);

            if (savingsRecord == null)
            {
                return NotFound();
            }

            int currentAmount = savingsRecord.Amount;

            int updatedAmount = currentAmount + amountEnteredByUser;

            savingsRecord.Amount = updatedAmount;

            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Saving/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Savings == null)
            {
                return NotFound();
            }

            var saving = await _context.Savings
                .FirstOrDefaultAsync(m => m.SavingId == id);
            if (saving == null)
            {
                return NotFound();
            }

            return View(saving);
        }

        // POST: Saving/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Savings == null)
            {
                return Problem("Entity set 'AppDbContext.Savings'  is null.");
            }
            var saving = await _context.Savings.FindAsync(id);
            if (saving != null)
            {
                _context.Savings.Remove(saving);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SavingExists(int id)
        {
          return (_context.Savings?.Any(e => e.SavingId == id)).GetValueOrDefault();
        }
    }
}
