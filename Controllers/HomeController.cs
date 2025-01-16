using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using gym.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;

namespace FitnessGYM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        IWebHostEnvironment _webHostEnvironment;
        public HomeController(ModelContext context, ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminView()
        {
            var firstName = HttpContext.Session.GetString("FirstName");
            var AdminID = HttpContext.Session.GetInt32("UserId");
            if (AdminID == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var admin = _context.Userrs.FirstOrDefault(u => u.UserId == AdminID && u.RoleId == 1); // RoleId 1 for Admin

            if (admin != null)
            {
                ViewBag.FirstName = admin.FirstName;
                ViewBag.ProfilePicPath = admin.PicPath;
            }
            else
            {
                ViewBag.ProfilePicPath = "~/AdminAssets/images/faces/default-avatar.jpg";
            }
            ViewBag.FirstName = firstName;
            ViewBag.NumberOfMembers = _context.Userrs.Where(u => u.RoleId == 3).Count();
            ViewBag.ActiveSubscriptions = _context.Subscriptions.Where(s => s.EndDate > DateTime.UtcNow).Count();
            ViewBag.TotalRevenue = (from s in _context.Subscriptions
                                    join m in _context.MembershipPlans on s.PlanId equals m.PlanId
                                    select m.Price)
                                    .Sum();
            ViewBag.MonthlyRevenue = _context.Subscriptions
                                 .Where(s => s.StartDate.Month == DateTime.Now.Month)
                                 .Join(_context.MembershipPlans, s => s.PlanId, m => m.PlanId, (s, m) => m.Price)
                                 .Sum();
            ViewBag.ExpiringSoon = _context.Subscriptions
                                 .Where(s => s.EndDate > DateTime.UtcNow && s.EndDate <= DateTime.UtcNow.AddDays(7))
                                 .Count();

            ViewBag.MemberProgress = _context.Sessions
                                           .Where(s => s.Member.Subscriptions.Any(sub => sub.EndDate >= DateTime.Now))
                                           .Select(s => new
                                           {
                                               SessionId = s.SessionId,
                                               MemberName = s.Member.FirstName + " " + s.Member.LastName,
                                               Attendance = s.CheckIn.HasValue && s.CheckOut.HasValue &&
                                                            (s.CheckOut.Value - s.CheckIn.Value) == (s.EndTime - s.StartTime),
                                               TrainerId = s.TrainerId.ToString()
                                           })
                                           .ToList();
            ViewBag.Testimonials = _context.Testimonials
                .Select(t => new
                {
                    t.TestimonialId,
                    UserName = t.User.FirstName + " " + t.User.LastName,
                    t.TContent,
                    t.SubmittedAt,
                    Status = t.IsApproved
                }).ToList();



            return View();

        }

        [HttpGet]
        public IActionResult Search()
        {
            // Load all subscriptions initially (or you can return an empty list)
            var subscriptions = _context.Subscriptions
                .Include(s => s.User) // Include related User data
                .Include(s => s.Plan) // Include related Plan data
                .ToList();

            return View(subscriptions);
        }

        [HttpPost]
        public async Task<IActionResult> Search(DateTime? startDate, DateTime? endDate)
        {
            // Query to fetch subscriptions
            var subscriptionsQuery = _context.Subscriptions
                .Include(s => s.User) // Include related User data
                .Include(s => s.Plan) // Include related Plan data
                .AsQueryable();

            if (startDate == null && endDate == null)
            {
                // If no dates provided, return all subscriptions
                return View(await subscriptionsQuery.ToListAsync());
            }
            else if (startDate == null && endDate != null)
            {
                // Filter by endDate only
                var result = await subscriptionsQuery
                    .Where(s => s.StartDate.AddDays(-1).Date == endDate.Value.Date)
                    .ToListAsync();
                return View(result);
            }
            else if (startDate != null && endDate == null)
            {
                // Filter by startDate only
                var result = await subscriptionsQuery
                    .Where(s => s.StartDate.AddDays(-1).Date == startDate.Value.Date)
                    .ToListAsync();
                return View(result);
            }
            else
            {
                // Filter by both startDate and endDate
                var result = await subscriptionsQuery
                    .Where(s => s.StartDate.AddDays(-1) >= startDate && s.StartDate.AddDays(-1) <= endDate)
                    .ToListAsync();
                return View(result);
            }
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var user = _context.Userrs.FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var model = new Userr
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                RoleId = user.RoleId,
                PicPath = string.IsNullOrEmpty(user.PicPath)
                            ? "~/AdminAssets/images/faces/default-avatar.jpg"
                            : user.PicPath
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            // Get the UserId from session
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth"); // Redirect to login if no session exists
            }

            var user = _context.Userrs.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound();
            }

            // Map user data to UserForm
            var model = new UserForm
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                PicPath = user.PicPath
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UserForm model)
        {
            // Get the UserId from session
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth"); // Redirect to login if no session exists
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Userrs.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound();
            }

            // Update user fields
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Phone = model.Phone;
            user.Address = model.Address;

            // Handle image file upload
            if (model.ImageFile != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageFile.FileName);
                string path = Path.Combine(wwwRootPath, "images", fileName);

                // Ensure directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                // Save the file
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(user.PicPath))
                {
                    string oldFilePath = Path.Combine(wwwRootPath, user.PicPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Update PicPath
                user.PicPath = $"/images/{fileName}";
            }

            // Save changes to the database
            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Profile"); // Redirect to profile page
        }


        [HttpPost]
        public IActionResult ApproveItem(int id)
        {
            var testimonial = _context.Testimonials.FirstOrDefault(t => t.TestimonialId == id);
            if (testimonial != null)
            {
                testimonial.IsApproved = "Approved";
                _context.SaveChanges();
            }
            TempData["SuccessMessage"] = "The testimonial has been approved.";
            return RedirectToAction("AdminView");
        }

        [HttpPost]
        public IActionResult RejectItem(int id)
        {
            var testimonial = _context.Testimonials.FirstOrDefault(t => t.TestimonialId == id);
            if (testimonial != null)
            {
                testimonial.IsApproved = "Rejected";
                _context.SaveChanges();
            }
            TempData["ErrorMessage"] = "The testimonial has been rejected.";
            return RedirectToAction("AdminView");
        }


        public IActionResult TrainerView()
        {
            var firstName = HttpContext.Session.GetString("FirstName");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.FirstName = firstName;
            return View();
        }

        public IActionResult MemberView()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var member = _context.Userrs.FirstOrDefault(u => u.UserId == userId && u.RoleId == 3); // RoleId 3 for Member

            if (member != null)
            {
                ViewBag.FirstName = member.FirstName;
                ViewBag.ProfilePicPath = member.PicPath;
            }
            else
            {
                ViewBag.ProfilePicPath = "~/AdminAssets/images/faces/default-avatar.jpg";
            }

            var homepage = _context.Homepages.FirstOrDefault();
            if (homepage == null)
            {
                return NotFound("Homepage content not found.");
            }

            ViewBag.HomepageContent = new
            {
                SliderTitle1 = homepage.Slidertitle1,
                SliderSubtitle1 = homepage.Slidersubtitle1,
                SliderImage1 = homepage.Sliderimage1,
                SliderTitle2 = homepage.Slidertitle2,
                SliderSubtitle2 = homepage.Slidersubtitle2,
                SliderImage2 = homepage.Sliderimage2
            };

            var membershipPlans = _context.MembershipPlans.ToList(); 

            var plansWithImages = membershipPlans
                .Select((plan, index) => new
                {
                    PlanId = plan.PlanId,
                    Name = plan.PlanName,
                    Price = plan.Price,
                    ImagePath = $"~Assets/images/img_{(index % 4) + 1}_square.jpg" 
                })
                .ToList();

            ViewBag.Plans = plansWithImages; 


            ViewBag.Testimonials = _context.Testimonials
                .Include(t => t.User)
                .Where(t => t.IsApproved == "Yes")
                .ToList();


            
            return View();
        }

        public IActionResult MembershipPlans()
        {

            var plans = _context.MembershipPlans.ToList();

            return View(plans);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }






    }
}

