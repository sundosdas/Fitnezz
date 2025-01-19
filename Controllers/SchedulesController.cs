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
    public class SchedulesController : Controller
    {
        private readonly ModelContext _context;

        public SchedulesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Schedules.Include(s => s.Session).Include(s => s.Trainer);
            return View(await modelContext.ToListAsync());
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Session)
                .Include(s => s.Trainer)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        public IActionResult Create()
        {
            var trainerId = HttpContext.Session.GetInt32("UserId"); 
            if (trainerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewData["SessionId"] = new SelectList(_context.Sessions, "SessionId", "SessionId");
            return View();
        }
        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ScheduleForm scheduleForm)
        {
            var trainerId = HttpContext.Session.GetInt32("UserId"); 
            if (trainerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                var schedule = new Schedule
                {
                    TrainerId = (decimal)trainerId, 
                    AvailableFrom = scheduleForm.AvailableFrom,
                    AvailableTo = scheduleForm.AvailableTo,
                    SessionId = scheduleForm.SessionId,
                    DayOfWeek = scheduleForm.DayOfWeek
                };

                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SessionId"] = new SelectList(_context.Sessions, "SessionId", "SessionId", scheduleForm.SessionId);
            return View(scheduleForm);
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            var trainerId = HttpContext.Session.GetInt32("UserId"); 
            if (trainerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null || schedule.TrainerId != trainerId) 
            {
                return Unauthorized();
            }

            var scheduleForm = new ScheduleForm
            {
                ScheduleId = schedule.ScheduleId,
                AvailableFrom = schedule.AvailableFrom,
                AvailableTo = schedule.AvailableTo,
                SessionId = schedule.SessionId,
                DayOfWeek = schedule.DayOfWeek
            };

            ViewData["SessionId"] = new SelectList(_context.Sessions, "SessionId", "SessionId", scheduleForm.SessionId);
            return View(scheduleForm);
        }


        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, ScheduleForm scheduleForm)
        {
            var trainerId = HttpContext.Session.GetInt32("UserId"); // Retrieve TrainerId from session
            if (trainerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id != scheduleForm.ScheduleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var schedule = await _context.Schedules.FindAsync(id);
                if (schedule == null || schedule.TrainerId != trainerId) // Verify ownership
                {
                    return Unauthorized();
                }

                schedule.AvailableFrom = scheduleForm.AvailableFrom;
                schedule.AvailableTo = scheduleForm.AvailableTo;
                schedule.SessionId = scheduleForm.SessionId;
                schedule.DayOfWeek = scheduleForm.DayOfWeek;

                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(scheduleForm.ScheduleId))
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

            ViewData["SessionId"] = new SelectList(_context.Sessions, "SessionId", "SessionId", scheduleForm.SessionId);
            return View(scheduleForm);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Session)
                .Include(s => s.Trainer)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Schedules == null)
            {
                return Problem("Entity set 'ModelContext.Schedules'  is null.");
            }
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(decimal id)
        {
          return (_context.Schedules?.Any(e => e.ScheduleId == id)).GetValueOrDefault();
        }
    }
}
