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
    public class SessionsController : Controller
    {
        private readonly ModelContext _context;

        public SessionsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Sessions
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Sessions.Include(s => s.Member).Include(s => s.Trainer);
            return View(await modelContext.ToListAsync());
        }

        // GET: Sessions/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Sessions == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Member)
                .Include(s => s.Trainer)
                .FirstOrDefaultAsync(m => m.SessionId == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // GET: Sessions/Create
        public IActionResult Create()
        {
            var trainerId = HttpContext.Session.GetInt32("UserId");
            if (trainerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewData["MemberId"] = new SelectList(_context.Userrs.Where(u => u.RoleId == 3), "UserId", "FirstName");
            return View(new SessionForm { TrainerId = trainerId.Value });
        }


        // POST: Sessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Sessions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SessionForm form)
        {
            var trainerId = HttpContext.Session.GetInt32("UserId");
            if (trainerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                var session = new Session
                {
                    MemberId = form.MemberId,
                    TrainerId = trainerId.Value,
                    StartTime = form.StartTime ?? DateTime.Now, // Default to now if not specified
                    Status = form.Status ?? "Scheduled"
                };

                _context.Sessions.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns in case of validation errors
            ViewData["MemberId"] = new SelectList(_context.Userrs.Where(u => u.RoleId == 3), "UserId", "FirstName", form.MemberId);
            return View(form);
        }

        // GET: Sessions/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            var form = new SessionForm
            {
                SessionId = session.SessionId,
                MemberId = session.MemberId,
                TrainerId = session.TrainerId,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Status = session.Status
            };

            ViewData["MemberId"] = new SelectList(_context.Userrs.Where(u => u.RoleId == 3), "UserId", "FirstName", session.MemberId);

            return View(form);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Sessions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, SessionForm form)
        {
            var trainerId = HttpContext.Session.GetInt32("UserId");
            if (trainerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id != form.SessionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var session = await _context.Sessions.FindAsync(id);
                if (session == null)
                {
                    return NotFound();
                }

                session.MemberId = form.MemberId;
                session.TrainerId = trainerId.Value;
                session.StartTime = form.StartTime ?? session.StartTime;
                session.EndTime = form.EndTime ?? session.EndTime;
                session.Status = form.Status ?? session.Status;

                try
                {
                    _context.Update(session);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Sessions.Any(e => e.SessionId == id))
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

            ViewData["MemberId"] = new SelectList(_context.Userrs.Where(u => u.RoleId == 3), "UserId", "FirstName", form.MemberId);
            return View(form);
        }

        // GET: Sessions/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Sessions == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Member)
                .Include(s => s.Trainer)
                .FirstOrDefaultAsync(m => m.SessionId == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Sessions == null)
            {
                return Problem("Entity set 'ModelContext.Sessions'  is null.");
            }
            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionExists(decimal id)
        {
          return (_context.Sessions?.Any(e => e.SessionId == id)).GetValueOrDefault();
        }
    }
}
