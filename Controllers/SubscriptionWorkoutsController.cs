using gym.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace gym.Controllers
{
    public class SubscriptionWorkoutsController : Controller
    {
        private readonly ModelContext _context;

        public SubscriptionWorkoutsController(ModelContext context)
        {
            _context = context;
        }

        // GET: SubscriptionWorkouts/Create
        public IActionResult Create()
        {
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "SubscriptionId", "SubscriptionId");
            ViewData["WorkoutId"] = new SelectList(_context.WorkoutPlans, "WorkoutId", "WorkoutId");
            return View();
        }

        // POST: SubscriptionWorkouts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SWform subscriptionWorkoutForm)
        {
            if (ModelState.IsValid)
            {
                var subscriptionWorkout = new SubscriptionWorkout
                {
                    SubscriptionId = subscriptionWorkoutForm.SubscriptionId,
                    WorkoutId = subscriptionWorkoutForm.WorkoutId
                };

                _context.SubscriptionWorkouts.Add(subscriptionWorkout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to Index or another suitable action
            }

            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "SubscriptionId", "SubscriptionId", subscriptionWorkoutForm.SubscriptionId);
            ViewData["WorkoutId"] = new SelectList(_context.WorkoutPlans, "WorkoutId", "WorkoutId", subscriptionWorkoutForm.WorkoutId);
            return View(subscriptionWorkoutForm);
        }

        // GET: SubscriptionWorkouts/Index
        public async Task<IActionResult> Index()
        {
            var subscriptionWorkouts = _context.SubscriptionWorkouts
                .Include(sw => sw.Subscription)
                .Include(sw => sw.Workout)
                .ToListAsync();

            return View(await subscriptionWorkouts);
        }

        // Add other CRUD actions (Edit, Details, Delete) as required
    

        // GET: SubscriptionWorkouts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.SubscriptionWorkouts == null)
            {
                return NotFound();
            }

            var subscriptionWorkout = await _context.SubscriptionWorkouts.FindAsync(id);
            if (subscriptionWorkout == null)
            {
                return NotFound();
            }
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "SubscriptionId", "SubscriptionId", subscriptionWorkout.SubscriptionId);
            ViewData["WorkoutId"] = new SelectList(_context.WorkoutPlans, "WorkoutId", "WorkoutId", subscriptionWorkout.WorkoutId);
            return View(subscriptionWorkout);
        }

        // POST: SubscriptionWorkouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("SubscriptionId,WorkoutId")] SubscriptionWorkout subscriptionWorkout)
        {
            if (id != subscriptionWorkout.SubscriptionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscriptionWorkout);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionWorkoutExists(subscriptionWorkout.SubscriptionId))
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
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "SubscriptionId", "SubscriptionId", subscriptionWorkout.SubscriptionId);
            ViewData["WorkoutId"] = new SelectList(_context.WorkoutPlans, "WorkoutId", "WorkoutId", subscriptionWorkout.WorkoutId);
            return View(subscriptionWorkout);
        }

        // GET: SubscriptionWorkouts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.SubscriptionWorkouts == null)
            {
                return NotFound();
            }

            var subscriptionWorkout = await _context.SubscriptionWorkouts
                .Include(s => s.Subscription)
                .Include(s => s.Workout)
                .FirstOrDefaultAsync(m => m.SubscriptionId == id);
            if (subscriptionWorkout == null)
            {
                return NotFound();
            }

            return View(subscriptionWorkout);
        }

        // POST: SubscriptionWorkouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.SubscriptionWorkouts == null)
            {
                return Problem("Entity set 'ModelContext.SubscriptionWorkouts'  is null.");
            }
            var subscriptionWorkout = await _context.SubscriptionWorkouts.FindAsync(id);
            if (subscriptionWorkout != null)
            {
                _context.SubscriptionWorkouts.Remove(subscriptionWorkout);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscriptionWorkoutExists(decimal id)
        {
          return (_context.SubscriptionWorkouts?.Any(e => e.SubscriptionId == id)).GetValueOrDefault();
        }
    }
}
