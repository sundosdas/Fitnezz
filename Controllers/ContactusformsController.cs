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
    public class ContactusformsController : Controller
    {
        private readonly ModelContext _context;

        public ContactusformsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Contactusforms
        public async Task<IActionResult> Index()
        {
              return _context.Contactusforms != null ? 
                          View(await _context.Contactusforms.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Contactusforms'  is null.");
        }

        // GET: Contactusforms/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Contactusforms == null)
            {
                return NotFound();
            }

            var contactusform = await _context.Contactusforms
                .FirstOrDefaultAsync(m => m.Formid == id);
            if (contactusform == null)
            {
                return NotFound();
            }

            return View(contactusform);
        }

        // GET: Contactusforms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contactusforms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Formid,Username,Phone,Email,Message,Submittedat,Isreviewed")] Contactusform contactusform)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactusform);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactusform);
        }

        // GET: Contactusforms/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Contactusforms == null)
            {
                return NotFound();
            }

            var contactusform = await _context.Contactusforms.FindAsync(id);
            if (contactusform == null)
            {
                return NotFound();
            }
            return View(contactusform);
        }

        // POST: Contactusforms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Formid,Username,Phone,Email,Message,Submittedat,Isreviewed")] Contactusform contactusform)
        {
            if (id != contactusform.Formid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactusform);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactusformExists(contactusform.Formid))
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
            return View(contactusform);
        }

        // GET: Contactusforms/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Contactusforms == null)
            {
                return NotFound();
            }

            var contactusform = await _context.Contactusforms
                .FirstOrDefaultAsync(m => m.Formid == id);
            if (contactusform == null)
            {
                return NotFound();
            }

            return View(contactusform);
        }

        // POST: Contactusforms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Contactusforms == null)
            {
                return Problem("Entity set 'ModelContext.Contactusforms'  is null.");
            }
            var contactusform = await _context.Contactusforms.FindAsync(id);
            if (contactusform != null)
            {
                _context.Contactusforms.Remove(contactusform);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactusformExists(decimal id)
        {
          return (_context.Contactusforms?.Any(e => e.Formid == id)).GetValueOrDefault();
        }
    }
}
