using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using gym.Models;

namespace FitnessGYM.Controllers
{
    public class UserrsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserrsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Userrs
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Userrs.Include(u => u.Role);
            return View(await modelContext.ToListAsync());
        }
        // GET: Members
        public IActionResult MembersProfile()
        {
            var membersProfile = _context.Userrs.Where(u => u.RoleId == 3).ToList();
            return View("Index", membersProfile);
        }

        //GET: Trainers
        public IActionResult TrainersProfile()
        {
            var trainersProfile = _context.Userrs.Where(u => u.RoleId == 2).ToList();
            return View("Index", trainersProfile);
        }
        // GET: Userrs/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Userrs == null)
            {
                return NotFound();
            }

            var userr = await _context.Userrs
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userr == null)
            {
                return NotFound();
            }

            return View(userr);
        }

        // GET: Userrs/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Rolees, "RoleId", "RoleId");
            return View();
        }

        // POST: Userrs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserForm userIn)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Map UserForm to Userr
                    Userr userr = new Userr
                    {
                        FirstName = userIn.FirstName,
                        LastName = userIn.LastName,
                        Email = userIn.Email,
                        Phone = userIn.Phone,
                        Address = userIn.Address,
                        RoleId = userIn.RoleId,
                        CreatedAt = DateTime.Now
                    };

                    // Handle Image Upload
                    if (userIn.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(userIn.ImageFile.FileName);
                        string path = Path.Combine(wwwRootPath, "images", fileName);

                        // Ensure the directory exists
                        Directory.CreateDirectory(Path.GetDirectoryName(path));

                        // Save the file
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await userIn.ImageFile.CopyToAsync(fileStream);
                        }

                        // Save the file path in the database
                        userr.PicPath = $"/images/{fileName}";
                    }

                    // Add the user to the database
                    _context.Add(userr);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the user.");
                }
            }

            // Repopulate the dropdown list in case of an error
            ViewData["RoleId"] = new SelectList(_context.Rolees, "RoleId", "RoleName", userIn.RoleId);

            return View(userIn);
        }


        // GET: Userrs/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Userrs == null)
            {
                return NotFound();
            }

            var userr = await _context.Userrs.FindAsync(id);
            if (userr == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Rolees, "RoleId", "RoleId", userr.RoleId);
            return View(userr);
        }

        // POST: Userrs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, UserForm userIn)
        {
            if (id != userIn.UserId)
            {
                return NotFound();
            }

            // If no new image file is provided and there's no existing image, add a validation error
            if (userIn.ImageFile == null && string.IsNullOrEmpty(userIn.PicPath))
            {
                ModelState.AddModelError("ImageFile", "An image file is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Find the existing user
                    var userr = await _context.Userrs.FindAsync(id);
                    if (userr == null)
                    {
                        return NotFound();
                    }

                    // Map UserForm to Userr
                    userr.FirstName = userIn.FirstName;
                    userr.LastName = userIn.LastName;
                    userr.Email = userIn.Email;
                    userr.Phone = userIn.Phone;
                    userr.Address = userIn.Address;
                    userr.RoleId = userIn.RoleId;

                    // Handle Image Upload
                    if (userIn.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(userIn.ImageFile.FileName);
                        string path = Path.Combine(wwwRootPath, "images", fileName);

                        // Ensure the directory exists
                        Directory.CreateDirectory(Path.GetDirectoryName(path));

                        // Save the new file
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await userIn.ImageFile.CopyToAsync(fileStream);
                        }

                        // Delete the old image file if it exists
                        if (!string.IsNullOrEmpty(userr.PicPath))
                        {
                            string oldFilePath = Path.Combine(wwwRootPath, userr.PicPath.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Update the image path
                        userr.PicPath = $"/images/{fileName}";
                    }

                    // Update the user in the database
                    _context.Update(userr);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserrExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Repopulate dropdown in case of an error
            ViewData["RoleId"] = new SelectList(_context.Rolees, "RoleId", "RoleName", userIn.RoleId);
            return View(userIn);
        }

        // GET: Userrs/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Userrs == null)
            {
                return NotFound();
            }

            var userr = await _context.Userrs
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userr == null)
            {
                return NotFound();
            }

            return View(userr);
        }

        // POST: Userrs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Userrs == null)
            {
                return Problem("Entity set 'ModelContext.Userrs'  is null.");
            }
            var userr = await _context.Userrs.FindAsync(id);
            if (userr != null)
            {
                _context.Userrs.Remove(userr);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserrExists(decimal id)
        {
            return (_context.Userrs?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
