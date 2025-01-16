using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using gym.Models;

namespace GYM.Controllers
{
    public class AuthController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ModelContext _context;
        private readonly ILogger<AuthController> _logger;
        public AuthController(ModelContext context,
        IWebHostEnvironment hostEnvironment, ILogger<AuthController> logger)
        {
            this._hostEnvironment = hostEnvironment;
            _context = context;
            _logger = logger;
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Ensure username and password are not null
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Username or Password cannot be empty.";
                return View();
            }

            // Query UserLogin table
            var userLogin = await _context.UserLogins
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower() && u.Pass == password);

            if (userLogin == null)
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View();
            }

            // Query Userr table for the user details
            var user = await _context.Userrs
                .FirstOrDefaultAsync(u => u.UserId == userLogin.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = "User details not found.";
                return View();
            }
            else
            {
                HttpContext.Session.SetInt32("UserId", (int)user.UserId);
                HttpContext.Session.SetString("FirstName", user.FirstName);
                // Redirect based on role
                return user.RoleId switch
                {
                    1 => RedirectToAction("AdminView", "Home"),
                    2 => RedirectToAction("TrainerView", "Home"),
                    _ => RedirectToAction("MemberView", "Home")
                };
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserForm user1, string username, string password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Map UserForm to Userr
                    Userr user = new Userr
                    {
                        FirstName = user1.FirstName,
                        LastName = user1.LastName,
                        Email = user1.Email,
                        Phone = user1.Phone,
                        Address = user1.Address,
                        RoleId = 3, // Assign RoleId for a regular user (e.g., Member role)
                        CreatedAt = DateTime.Now
                    };

                    // Handle Image Upload
                    if (user1.ImageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(user1.ImageFile.FileName);
                        string filePath = Path.Combine(wwwRootPath, "images", fileName);

                        // Ensure the directory exists
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                        // Save the file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await user1.ImageFile.CopyToAsync(stream);
                        }

                        // Set the image path
                        user.PicPath = $"/images/{fileName}";
                    }

                    // Add the user to the database
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    // Create UserLogin for the user
                    UserLogin login = new UserLogin
                    {
                        UserId = user.UserId,
                        Username = username,
                        Pass = password
                    };

                    // Add the login credentials to the database
                    _context.Add(login);
                    await _context.SaveChangesAsync();

                    // Redirect to the Login page
                    return RedirectToAction("Login", "Auth");
                }
                catch (Exception ex)
                {
                    // Log the error
                    Console.WriteLine($"Error: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while registering the user.");
                }
            }

            // Return to the view with validation errors
            return View(user1);
        }


        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}
