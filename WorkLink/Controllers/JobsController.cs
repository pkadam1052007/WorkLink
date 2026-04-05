using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkLink.Data;
using WorkLink.Models;
using WorkLink.Services;
using WorkLink.Helpers;

namespace WorkLink.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public JobsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        // ===============================
        // USER BOOKINGS LIST
        // ===============================
        public async Task<IActionResult> MyBookings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var bookings = await _context.Jobs
                .Include(j => j.WorkerProfile)
                .Include(j => j.User)
                .Include(j => j.WorkerProfile.Reviews)
                .Where(j => j.UserId == user.Id)
                .OrderByDescending(j => j.JobDate)
                .ToListAsync();

            return View(bookings);
        }

        // ===============================
        // CANCEL BOOKING
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.JobId == id && j.UserId == user.Id);

            if (job == null) return NotFound();

            if (job.Status == JobStatus.Requested)
            {
                job.Status = JobStatus.Cancelled;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(MyBookings));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelByWorker(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var job = await _context.Jobs
                .Include(j => j.WorkerProfile)
                .FirstOrDefaultAsync(j =>
                    j.JobId == id &&
                    j.WorkerProfile.UserId == user.Id);

            if (job == null)
                return NotFound();

            if (job.Status == JobStatus.Requested || job.Status == JobStatus.Accepted)
            {
                job.Status = JobStatus.Cancelled;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Dashboard", "WorkerProfiles");
        }


        // ===============================
        // STEP 1: OPEN BOOKING PAGE
        // ===============================
        public async Task<IActionResult> Create(int workerId)
        {
            var worker = await _context.WorkerProfiles
                .FirstOrDefaultAsync(w => w.WorkerProfileId == workerId);

            if (worker == null)
                return NotFound();

            return View("~/Views/Jobs/Create.cshtml", worker);
        }

        // ===============================
        // STEP 2: SAVE BOOKING + EMAIL
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
     int workerProfileId,
     DateTime jobDate,
     int hours,
     double distanceKm,
     double employerLatitude,
     double employerLongitude)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var worker = await _context.WorkerProfiles
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.WorkerProfileId == workerProfileId);

            if (worker == null) return NotFound();

            // 🔹 AUTO CALCULATE DISTANCE
            distanceKm = WorkLink.Helpers.DistanceHelper.CalculateDistance(
                employerLatitude,
                employerLongitude,
                worker.Latitude,
                worker.Longitude);

            var totalAmount =
                (worker.HourlyRate ?? 0) * hours +
                (worker.RatePerKm ?? 0) * (decimal)distanceKm;

            var job = new Job
            {
                UserId = user.Id,
                WorkerProfileId = worker.WorkerProfileId,
                JobDate = jobDate,
                Hours = hours,
                DistanceKm = distanceKm,
                EmployerLatitude = employerLatitude,
                EmployerLongitude = employerLongitude,
                TotalAmount = totalAmount,
                Status = JobStatus.Requested
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            // ===============================
            // 📧 SEND EMAIL TO WORKER
            // ===============================
            if (!string.IsNullOrEmpty(worker.User.Email))
            {
                var subject = "New Job Request on WorkLink 🚀";
                var body = $@"
            <h3>Hello {worker.FullName},</h3>
            <p>You have received a new job request.</p>

            <b>Date:</b> {jobDate:dd MMM yyyy}<br/>
            <b>Hours:</b> {hours}<br/>
            <b>Distance:</b> {distanceKm:F2} km<br/>
            <b>Total:</b> ₹{totalAmount}

            <br/><br/>
            <p>Please log in to your dashboard to accept or reject this job.</p>
            <br/>
            <p>– WorkLink Team</p>
        ";

                await _emailService.SendEmailAsync(
                    worker.User.Email,
                    subject,
                    body
                );
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
