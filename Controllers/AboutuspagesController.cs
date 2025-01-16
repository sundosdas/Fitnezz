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
    public class AboutuspagesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AboutuspagesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment= webHostEnvironment;
        }

        // GET: Aboutuspages
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Aboutuspages.Include(a => a.Admin);
            return View(await modelContext.ToListAsync());
        }

        // GET: Aboutuspages/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Aboutuspages == null)
            {
                return NotFound();
            }

            var aboutuspage = await _context.Aboutuspages
                .Include(a => a.Admin)
                .FirstOrDefaultAsync(m => m.AbModificationid == id);
            if (aboutuspage == null)
            {
                return NotFound();
            }

            return View(aboutuspage);
        }

        // GET: Aboutuspages/Create
        public IActionResult Create()
        {

            var userId = HttpContext.Session.GetInt32("UserId");
            decimal loggedInUserId = Convert.ToDecimal(userId);
            if (!userId.HasValue)
            {
                return Unauthorized("User is not logged in.");
            }

            var user = _context.Userrs.Find(loggedInUserId);
            if (user == null || user.RoleId != 1)
            {
                return Forbid("You are not authorized to create.");
            }
            return View();
        }

        // POST: Aboutuspages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(abUS model)
        {
            if (ModelState.IsValid)
            {
                var aboutuspage = new Aboutuspage
                {
                    Headertitle = model.Headertitle,
                    Paragraph1 = model.Paragraph1,
                    Paragraph2 = model.Paragraph2,
                    Paragraph3 = model.Paragraph3,
                    Lastupdated = DateTime.Now,
                    Adminid = HttpContext.Session.GetInt32("UserId") ?? 0 
                };

                // Handle Image 1 Upload
                if (model.ImageFile1 != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageFile1.FileName);
                    string filePath = Path.Combine(wwwRootPath, "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile1.CopyToAsync(stream);
                    }

                    aboutuspage.Image1 = $"/images/{fileName}";
                }

                // Handle Image 2 Upload
                if (model.ImageFile2 != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageFile2.FileName);
                    string filePath = Path.Combine(wwwRootPath, "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile2.CopyToAsync(stream);
                    }

                    aboutuspage.Image2 = $"/images/{fileName}";
                }

                _context.Add(aboutuspage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Aboutuspages/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null) return NotFound();

            var aboutuspage = await _context.Aboutuspages.FindAsync(id);
            if (aboutuspage == null) return NotFound();

            var model = new abUS
            {
                AbModificationid = aboutuspage.AbModificationid,
                Headertitle = aboutuspage.Headertitle,
                Paragraph1 = aboutuspage.Paragraph1,
                Paragraph2 = aboutuspage.Paragraph2,
                Paragraph3 = aboutuspage.Paragraph3,
                ImageFile1 = aboutuspage.ImageFile1,
                ImageFile2 = aboutuspage.ImageFile2
            };

            return View(model);
        }


        // POST: Aboutuspages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, abUS model)
        {
            if (id != model.AbModificationid) return NotFound();

            if (ModelState.IsValid)
            {
                var aboutuspage = await _context.Aboutuspages.FindAsync(id);
                if (aboutuspage == null) return NotFound();

                aboutuspage.Headertitle = model.Headertitle;
                aboutuspage.Paragraph1 = model.Paragraph1;
                aboutuspage.Paragraph2 = model.Paragraph2;
                aboutuspage.Paragraph3 = model.Paragraph3;
                aboutuspage.Lastupdated = DateTime.Now;

                // Handle Image 1 Upload
                if (model.ImageFile1 != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageFile1.FileName);
                    string filePath = Path.Combine(wwwRootPath, "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile1.CopyToAsync(stream);
                    }

                    aboutuspage.Image1 = $"/images/{fileName}";
                }

                // Handle Image 2 Upload
                if (model.ImageFile2 != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageFile2.FileName);
                    string filePath = Path.Combine(wwwRootPath, "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile2.CopyToAsync(stream);
                    }

                    aboutuspage.Image2 = $"/images/{fileName}";
                }

                _context.Update(aboutuspage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    

        // GET: Aboutuspages/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Aboutuspages == null)
            {
                return NotFound();
            }

            var aboutuspage = await _context.Aboutuspages
                .Include(a => a.Admin)
                .FirstOrDefaultAsync(m => m.AbModificationid == id);
            if (aboutuspage == null)
            {
                return NotFound();
            }

            return View(aboutuspage);
        }

        // POST: Aboutuspages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Aboutuspages == null)
            {
                return Problem("Entity set 'ModelContext.Aboutuspages'  is null.");
            }
            var aboutuspage = await _context.Aboutuspages.FindAsync(id);
            if (aboutuspage != null)
            {
                _context.Aboutuspages.Remove(aboutuspage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutuspageExists(decimal id)
        {
          return (_context.Aboutuspages?.Any(e => e.AbModificationid == id)).GetValueOrDefault();
        }
    }
}
