using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using gym.Models;
using Microsoft.AspNetCore.Hosting;

namespace gym.Controllers
{
    public class HomepagesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomepagesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Homepages
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Homepages.Include(h => h.Admin);
            return View(await modelContext.ToListAsync());
        }

        // GET: Homepages/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Homepages == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages
                .Include(h => h.Admin)
                .FirstOrDefaultAsync(m => m.HModificationid == id);
            if (homepage == null)
            {
                return NotFound();
            }

            return View(homepage);
        }

        // GET: Homepages/Create
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

        // POST: Homepages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(hmForm form)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Unauthorized("User is not logged in.");
            }

            decimal loggedInUserId = Convert.ToDecimal(userId.Value);
            var user = _context.Userrs.Find(loggedInUserId);
            if (user == null || user.RoleId != 1)
            {
                return Forbid("You are not authorized to create.");
            }
            if (ModelState.IsValid)
            {
                var homepage = new Homepage
                {
                    Slidertitle1 = form.Slidertitle1,
                    Slidersubtitle1 = form.Slidersubtitle1,
                    Slidertitle2 = form.Slidertitle2,
                    Slidersubtitle2 = form.Slidersubtitle2,
                    Lastupdated = DateTime.Now,
                    Adminid = loggedInUserId
                };

                if (form.ImageFileS1 != null)
                {
                    string fileName = Guid.NewGuid() + "_" + Path.GetFileName(form.ImageFileS1.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Assets/img", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await form.ImageFileS1.CopyToAsync(stream);
                    }
                    homepage.Sliderimage1 = "/images/" + fileName;
                }

                if (form.ImageFileS2 != null)
                {
                    string fileName = Guid.NewGuid() + "_" + Path.GetFileName(form.ImageFileS2.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Assets/img", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await form.ImageFileS2.CopyToAsync(stream);
                    }
                    homepage.Sliderimage2 = "/images/" + fileName;
                }
                homepage.Adminid = loggedInUserId;
                homepage.Lastupdated = DateTime.Now;
                _context.Add(homepage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(form); 
        }


        // GET: Homepages/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
                return NotFound();

            var homepage = await _context.Homepages.FindAsync(id);
            if (homepage == null)
                return NotFound();

            var viewModel = new hmForm
            {
                HModificationid = homepage.HModificationid,
                Slidertitle1 = homepage.Slidertitle1,
                Slidersubtitle1 = homepage.Slidersubtitle1,
                ImageFileS1 = homepage.ImageFileS1,
                Slidertitle2 = homepage.Slidertitle2,
                Slidersubtitle2 = homepage.Slidersubtitle2,
                ImageFileS2 = homepage.ImageFileS2,
                Lastupdated = homepage.Lastupdated
            };

            return View(viewModel);
        }
        // POST: Homepages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, hmForm form)
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

            if (id != form.HModificationid)
                return NotFound();

            if (ModelState.IsValid)
            {
                var homepage = await _context.Homepages.FindAsync(id);
                if (homepage == null)
                    return NotFound();

                homepage.Slidertitle1 = form.Slidertitle1;
                homepage.Slidersubtitle1 = form.Slidersubtitle1;
                homepage.Slidertitle2 = form.Slidertitle2;
                homepage.Slidersubtitle2 = form.Slidersubtitle2;
                homepage.Lastupdated = DateTime.Now;
                homepage.Adminid = loggedInUserId;

                // Update Slider Image 1
                if (form.ImageFileS1 != null)
                {
                    string fileName = Guid.NewGuid() + "_" + Path.GetFileName(form.ImageFileS1.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await form.ImageFileS1.CopyToAsync(stream);
                    }
                    homepage.Sliderimage1 = "/images/" + fileName;
                }

                // Update Slider Image 2
                if (form.ImageFileS2 != null)
                {
                    string fileName = Guid.NewGuid() + "_" + Path.GetFileName(form.ImageFileS2.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await form.ImageFileS2.CopyToAsync(stream);
                    }
                    homepage.Sliderimage2 = "/images/" + fileName;
                }

                _context.Update(homepage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(form);
        }
        // GET: Homepages/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Homepages == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages
                .Include(h => h.Admin)
                .FirstOrDefaultAsync(m => m.HModificationid == id);
            if (homepage == null)
            {
                return NotFound();
            }

            return View(homepage);
        }

        // POST: Homepages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Homepages == null)
            {
                return Problem("Entity set 'ModelContext.Homepages'  is null.");
            }
            var homepage = await _context.Homepages.FindAsync(id);
            if (homepage != null)
            {
                _context.Homepages.Remove(homepage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomepageExists(decimal id)
        {
          return (_context.Homepages?.Any(e => e.HModificationid == id)).GetValueOrDefault();
        }
    }
}
