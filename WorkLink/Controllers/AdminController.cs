using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkLink.Data;
using WorkLink.Models;

namespace WorkLink.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 🧠 DASHBOARD
        public async Task<IActionResult> Index()
        {
            // Stats
            ViewBag.TotalUsers = await _userManager.Users.CountAsync();
            ViewBag.TotalWorkers = await _context.WorkerProfiles.CountAsync();
            ViewBag.VerifiedWorkers = await _context.WorkerProfiles.CountAsync(w => w.IsVerified);
            ViewBag.PendingWorkers = await _context.WorkerProfiles.CountAsync(w => !w.IsVerified);
            ViewBag.TotalRevenue = await _context.Jobs
                .Where(j => j.Status == JobStatus.Completed)
                .SumAsync(j => j.TotalAmount);

            // Pending verifications
            ViewBag.PendingVerifications = await _context.WorkerProfiles
                .Include(w => w.User)
                .Where(w => !w.IsVerified)
                .OrderByDescending(w => w.WorkerProfileId)
                .ToListAsync();

            // **NEW** – Users & Workers lists
            ViewBag.AllUsers = await _userManager.Users.ToListAsync();
            ViewBag.AllWorkers = await _context.WorkerProfiles
                .Include(w => w.User)
                .OrderByDescending(w => w.WorkerProfileId)
                .ToListAsync();

            return View();
        }


        // ✅ VERIFY WORKER
        [HttpPost]
        public async Task<IActionResult> VerifyWorker(int id)
        {
            var worker = await _context.WorkerProfiles.FindAsync(id);
            if (worker == null) return NotFound();

            worker.IsVerified = true;

            _context.WorkerVerifications.Add(new WorkerVerification
            {
                WorkerProfileId = worker.WorkerProfileId,
                VerifiedOn = DateTime.UtcNow,
                VerifiedByAdminId = _userManager.GetUserId(User)!
            });

            await _context.SaveChangesAsync();

            TempData["Success"] = "Worker verified successfully ✅";
            return RedirectToAction(nameof(Index));
        }

        // 🧑‍💻 USERS MANAGEMENT
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUserLock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                // Unlock
                user.LockoutEnd = null;
            }
            else
            {
                // Lock for 100 years (effectively permanent)
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
            }

            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Users));
        }

        // 🛠 WORKERS MANAGEMENT
        public async Task<IActionResult> Workers()
        {
            var workers = await _context.WorkerProfiles
                .Include(w => w.User)
                .OrderByDescending(w => w.WorkerProfileId)
                .ToListAsync();

            return View(workers);
        }
    }
}
