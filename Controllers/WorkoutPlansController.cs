using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using gym.Models;

namespace gym.Controllers
{
    public class WorkoutPlansController : Controller
    {
        private readonly ModelContext _context;

        public WorkoutPlansController(ModelContext context)
        {
            _context = context;
        }

        // GET: WorkoutPlans
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.WorkoutPlans.Include(w => w.Member).Include(w => w.Trainer);
            return View(await modelContext.ToListAsync());
        }

        // GET: WorkoutPlans/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.WorkoutPlans == null)
            {
                return NotFound();
            }

            var workoutPlan = await _context.WorkoutPlans
                .Include(w => w.Member)
                .Include(w => w.Trainer)
                .FirstOrDefaultAsync(m => m.WorkoutId == id);
            if (workoutPlan == null)
            {
                return NotFound();
            }

            return View(workoutPlan);
        }

        // GET: WorkoutPlans/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Userrs, "UserId", "UserId");
            ViewData["TrainerId"] = new SelectList(_context.Userrs, "UserId", "UserId");
            return View();
        }

        // POST: WorkoutPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkoutId,TrainerId,MemberId,Details,Goals")] WorkoutPlan workoutPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workoutPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Userrs, "UserId", "UserId", workoutPlan.MemberId);
            ViewData["TrainerId"] = new SelectList(_context.Userrs, "UserId", "UserId", workoutPlan.TrainerId);
            return View(workoutPlan);
        }

        // GET: WorkoutPlans/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.WorkoutPlans == null)
            {
                return NotFound();
            }

            var workoutPlan = await _context.WorkoutPlans.FindAsync(id);
            if (workoutPlan == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Userrs, "UserId", "UserId", workoutPlan.MemberId);
            ViewData["TrainerId"] = new SelectList(_context.Userrs, "UserId", "UserId", workoutPlan.TrainerId);
            return View(workoutPlan);
        }

        // POST: WorkoutPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("WorkoutId,TrainerId,MemberId,Details,Goals")] WorkoutPlan workoutPlan)
        {
            if (id != workoutPlan.WorkoutId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workoutPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutPlanExists(workoutPlan.WorkoutId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Userrs, "UserId", "UserId", workoutPlan.MemberId);
            ViewData["TrainerId"] = new SelectList(_context.Userrs, "UserId", "UserId", workoutPlan.TrainerId);
            return View(workoutPlan);
        }

        // GET: WorkoutPlans/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.WorkoutPlans == null)
            {
                return NotFound();
            }

            var workoutPlan = await _context.WorkoutPlans
                .Include(w => w.Member)
                .Include(w => w.Trainer)
                .FirstOrDefaultAsync(m => m.WorkoutId == id);
            if (workoutPlan == null)
            {
                return NotFound();
            }

            return View(workoutPlan);
        }

        // POST: WorkoutPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.WorkoutPlans == null)
            {
                return Problem("Entity set 'ModelContext.WorkoutPlans'  is null.");
            }
            var workoutPlan = await _context.WorkoutPlans.FindAsync(id);
            if (workoutPlan != null)
            {
                _context.WorkoutPlans.Remove(workoutPlan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutPlanExists(decimal id)
        {
          return (_context.WorkoutPlans?.Any(e => e.WorkoutId == id)).GetValueOrDefault();
        }
    }
}
