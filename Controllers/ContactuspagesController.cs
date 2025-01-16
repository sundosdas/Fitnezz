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
    public class ContactuspagesController : Controller
    {
        private readonly ModelContext _context;

        public ContactuspagesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Contactuspages
        public async Task<IActionResult> Index()
        {
           
            var modelContext = _context.Contactuspages.Include(c => c.Admin);
            return View(await modelContext.ToListAsync());
        }

        // GET: Contactuspages/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Contactuspages == null)
            {
                return NotFound();
            }

            var contactuspage = await _context.Contactuspages
                .Include(c => c.Admin)
                .FirstOrDefaultAsync(m => m.CModificationid == id);
            if (contactuspage == null)
            {
                return NotFound();
            }

            return View(contactuspage);
        }

        // GET: Contactuspages/Create
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Unauthorized("User is not logged in.");
            }

            // Ensure the user is an admin
            decimal loggedInUserId = Convert.ToDecimal(userId.Value);
            var user = _context.Userrs.Find(loggedInUserId);
            if (user == null || user.RoleId != 1)
            {
                return Forbid("You are not authorized to create.");
            }

            return View();
        }


        // POST: Contactuspages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(cUS form)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Unauthorized("User is not logged in.");
            }

            // Ensure the user is an admin
            decimal loggedInUserId = Convert.ToDecimal(userId.Value);
            var user = _context.Userrs.Find(loggedInUserId);
            if (user == null || user.RoleId != 1)
            {
                return Forbid("You are not authorized to create.");
            }

            if (ModelState.IsValid)
            {
                var contactuspage = new Contactuspage
                {
                    Address = form.Address,
                    Phone = form.Phone,
                    Email = form.Email,
                    Lastupdated = DateTime.Now,
                    Adminid = loggedInUserId
                };

                _context.Add(contactuspage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(form);
        }

        // GET: Contactuspages/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactuspage = await _context.Contactuspages.FindAsync(id);
            if (contactuspage == null)
            {
                return NotFound();
            }

            var form = new cUS
            {
                Address = contactuspage.Address,
                Phone = contactuspage.Phone,
                Email = contactuspage.Email
            };

            return View(form);
        }


        // POST: Contactuspages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, cUS form)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Unauthorized("User is not logged in.");
            }

            // Ensure the user is an admin
            decimal loggedInUserId = Convert.ToDecimal(userId.Value);
            var user = _context.Userrs.Find(loggedInUserId);
            if (user == null || user.RoleId != 1)
            {
                return Forbid("You are not authorized to edit.");
            }

            var contactuspage = await _context.Contactuspages.FindAsync(id);
            if (contactuspage == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                contactuspage.Address = form.Address;
                contactuspage.Phone = form.Phone;
                contactuspage.Email = form.Email;
                contactuspage.Lastupdated = DateTime.Now;

                _context.Update(contactuspage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(form);
        }

        // GET: Contactuspages/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Contactuspages == null)
            {
                return NotFound();
            }

            var contactuspage = await _context.Contactuspages
                .Include(c => c.Admin)
                .FirstOrDefaultAsync(m => m.CModificationid == id);
            if (contactuspage == null)
            {
                return NotFound();
            }

            return View(contactuspage);
        }

        // POST: Contactuspages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Contactuspages == null)
            {
                return Problem("Entity set 'ModelContext.Contactuspages'  is null.");
            }
            var contactuspage = await _context.Contactuspages.FindAsync(id);
            if (contactuspage != null)
            {
                _context.Contactuspages.Remove(contactuspage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactuspageExists(decimal id)
        {
          return (_context.Contactuspages?.Any(e => e.CModificationid == id)).GetValueOrDefault();
        }
    }
}
