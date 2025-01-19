using SelectPdf;
using gym.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitnessGYM.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly ModelContext _context;
        private readonly EmailService _emailService;

        public SubscriptionsController(ModelContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: Subscriptions
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Subscriptions.Include(s => s.Plan).Include(s => s.User);
            return View(await modelContext.ToListAsync());
        }


        [HttpGet]
        public IActionResult SubscriptionsChartData()
        {
            var subscriptionData = _context.Subscriptions
                .GroupBy(s => new { Month = s.StartDate.Month, Year = s.StartDate.Year })
                .Select(g => new
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    Count = g.Count()
                })
                .OrderBy(data => data.Year)
                .ThenBy(data => data.Month)
                .ToList();

            return Json(subscriptionData);
        }

        // GET: Subscriptions/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.Plan)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SubscriptionId == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // GET: Subscriptions/Create
        public IActionResult Create()
        {
            ViewData["PlanId"] = new SelectList(_context.MembershipPlans, "PlanId", "PlanId");
            ViewData["UserId"] = new SelectList(_context.Userrs, "UserId", "UserId");
            return View();
        }

        // POST: Subscriptions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubscriptionForm subscriptionForm)
        {
            if (ModelState.IsValid)
            {
                // Map SubscriptionForm to Subscription
                var subscription = new Subscription
                {
                    UserId = subscriptionForm.UserId,
                    PlanId = subscriptionForm.PlanId,
                    StartDate = subscriptionForm.StartDate,
                    EndDate = subscriptionForm.StartDate.AddMonths((int)_context.MembershipPlans
                        .Where(p => p.PlanId == subscriptionForm.PlanId)
                        .Select(p => p.PDuration)
                        .FirstOrDefault()),
                    InvoicePath = "Pending" // Temporary placeholder
                };

                _context.Add(subscription);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Repopulate ViewData for dropdowns in case of an error
            ViewData["PlanId"] = new SelectList(_context.MembershipPlans, "PlanId", "PlanName", subscriptionForm.PlanId);
            ViewData["UserId"] = new SelectList(_context.Userrs, "UserId", "FullName", subscriptionForm.UserId);

            return View(subscriptionForm); // Return the form for correction
        }

        // GET: Subscriptions/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            ViewData["PlanId"] = new SelectList(_context.MembershipPlans, "PlanId", "PlanName", subscription.PlanId);
            ViewData["UserId"] = new SelectList(_context.Userrs, "UserId", "FullName", subscription.UserId);
            return View(subscription);
        }

        // POST: Subscriptions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("SubscriptionId,UserId,PlanId,StartDate,EndDate,InvoicePath")] Subscription subscription)
        {
            if (id != subscription.SubscriptionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionExists(subscription.SubscriptionId))
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
            ViewData["PlanId"] = new SelectList(_context.MembershipPlans, "PlanId", "PlanName", subscription.PlanId);
            ViewData["UserId"] = new SelectList(_context.Userrs, "UserId", "FullName", subscription.UserId);
            return View(subscription);
        }

        // GET: Subscriptions/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.Plan)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SubscriptionId == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // POST: Subscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SubscriptionExists(decimal id)
        {
            return (_context.Subscriptions?.Any(e => e.SubscriptionId == id)).GetValueOrDefault();
        }

        // GET: Subscribe
        public IActionResult Subscribe()
        {
            var membershipPlans = _context.MembershipPlans.ToList();

            var plansWithImages = membershipPlans
                .Select((plan, index) => new
                {
                    PlanId = plan.PlanId,
                    Name = plan.PlanName,
                    Price = plan.Price,
                    Workouts = plan.WorkoutsNum,
                    Duration = plan.PDuration, // Duration in DAYS
                    ImagePath = $"/Assets/img/img_{(index % 4) + 1}_square.jpg"
                })
                .ToList();

            ViewBag.Plans = plansWithImages;

            var workouts = _context.WorkoutPlans
                .Select(w => new
                {
                    Name = w.Details,
                    Goals = w.Goals,
                    TrainerName = w.Trainer.FirstName + " " + w.Trainer.LastName,
                    WorkoutId = w.WorkoutId,
                    ImagePath = "/Assets/img/img_" + (w.WorkoutId % 4 + 1) + "_square.jpg"
                })
                .ToList();

            ViewBag.Workouts = workouts;

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId"); // Assuming UserId is stored in the session
            return View();
        }

        // POST: Subscribe
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(SubscriptionForm subscriptionForm, List<decimal> SelectedWorkouts)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                var subscription = new Subscription
                {
                    UserId = (decimal)userId,
                    PlanId = subscriptionForm.PlanId,
                    StartDate = subscriptionForm.StartDate,
                    EndDate = subscriptionForm.StartDate.AddDays((int)_context.MembershipPlans
                        .Where(p => p.PlanId == subscriptionForm.PlanId)
                        .Select(p => p.PDuration)
                        .FirstOrDefault()),
                    InvoicePath = "Pending" 
                };

                _context.Subscriptions.Add(subscription);
                await _context.SaveChangesAsync();

                foreach (var workoutId in SelectedWorkouts)
                {
                    var subscriptionWorkout = new SubscriptionWorkout
                    {
                        SubscriptionId = subscription.SubscriptionId,
                        WorkoutId = workoutId
                    };
                    _context.SubscriptionWorkouts.Add(subscriptionWorkout);
                }
                await _context.SaveChangesAsync();

                var invoiceHtml = GenerateInvoiceHtml(subscription);

                var invoicePath = Path.Combine("wwwroot", "invoices", $"Invoice_{subscription.SubscriptionId}.pdf");

                SaveInvoiceAsPdf(invoiceHtml, invoicePath);

                subscription.InvoicePath = $"/invoices/Invoice_{subscription.SubscriptionId}.pdf";
                _context.Subscriptions.Update(subscription);
                await _context.SaveChangesAsync();

                await SendInvoiceEmail(subscription.UserId, invoicePath);

                return RedirectToAction("Invoice", new { subscriptionId = subscription.SubscriptionId });
            }

            return RedirectToAction(nameof(Subscribe));
        }

        private async Task SendInvoiceEmail(decimal userId, string invoicePath)
        {
            var user = _context.Userrs.Find(userId);
            var message = $"Dear {user.FirstName} {user.LastName},<br>Your subscription invoice is attached.";

            await _emailService.SendEmailAsync(user.Email, "Subscription Invoice", message, invoicePath);
        }


        // Generates HTML for the invoice
        private string GenerateInvoiceHtml(Subscription subscription)
        {
            var user = _context.Userrs.Find(subscription.UserId);
            var plan = _context.MembershipPlans.Find(subscription.PlanId);

            return $@"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                margin: 20px;
                            }}
                            h1 {{
                                color: #333;
                            }}
                            table {{
                                width: 100%;
                                border-collapse: collapse;
                                margin-top: 20px;
                            }}
                            th, td {{
                                border: 1px solid #ddd;
                                padding: 8px;
                                text-align: left;
                            }}
                            th {{
                                background-color: #f4f4f4;
                            }}
                        </style>
                    </head>
                    <body>
                        <h1>Invoice for Subscription #{subscription.SubscriptionId}</h1>
                        <table>
                            <tr><th>User Name</th><td>{user.FirstName} {user.LastName}</td></tr>
                            <tr><th>Plan Name</th><td>{plan.PlanName}</td></tr>
                            <tr><th>Start Date</th><td>{subscription.StartDate:yyyy-MM-dd}</td></tr>
                            <tr><th>End Date</th><td>{subscription.EndDate:yyyy-MM-dd}</td></tr>
                            <tr><th>Total Price</th><td>${plan.Price}</td></tr>
                        </table>
                    </body>
                    </html>";
        }

        // Saves the invoice as a PDF
        private void SaveInvoiceAsPdf(string html, string filePath)
        {
            // Ensure the directory exists
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Convert the HTML string to a PDF document
            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
            SelectPdf.PdfDocument pdf = converter.ConvertHtmlString(html);

            // Save the PDF document to the specified file path
            pdf.Save(filePath);

            // Close the PDF document to release resources
            pdf.Close();
        }

        public IActionResult Invoice(decimal subscriptionId)
        {
            // Fetch the subscription details
            var subscription = _context.Subscriptions
                .Include(s => s.User)
                .Include(s => s.Plan)
                .FirstOrDefault(s => s.SubscriptionId == subscriptionId);

            if (subscription == null)
            {
                return NotFound();
            }

            // Generate the invoice HTML and define the PDF path
            var invoiceHtml = GenerateInvoiceHtml(subscription);
            var invoicePath = Path.Combine("wwwroot", "invoices", $"Invoice_{subscription.SubscriptionId}.pdf");

            // Save the invoice as a PDF if it doesn't already exist
            if (!System.IO.File.Exists(invoicePath))
            {
                SaveInvoiceAsPdf(invoiceHtml, invoicePath);
            }

            // Pass the invoice path and details to the view
            ViewBag.InvoicePath = $"/invoices/Invoice_{subscription.SubscriptionId}.pdf";
            return View(subscription);
        }



    }
}
