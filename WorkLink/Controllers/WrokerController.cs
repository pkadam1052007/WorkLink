using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkLink.Data;
using WorkLink.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WorkLink.Controllers
{
    [Authorize(Roles = "Worker")]
    public class WorkerProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorkerProfilesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Create Profile
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (await _context.WorkerProfiles.AnyAsync(w => w.UserId == user.Id))
                return RedirectToAction("Edit");

            return View();
        }

        // POST: Create Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
     [Bind("FullName,City,Profession,Experience,PhoneNumber,Address,HourlyRate,RatePerKm,Latitude,Longitude")] WorkerProfile model,
     IFormFile? ProfileImageFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            model.UserId = user.Id;

            // Handle file upload
            if (ProfileImageFile != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(ProfileImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await ProfileImageFile.CopyToAsync(stream);

                model.ProfileImageUrl = "/uploads/" + fileName;
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors });

                return Content(
                    string.Join("\n",
                        errors.Select(e =>
                            $"{e.Key}: {string.Join(",", e.Errors.Select(er => er.ErrorMessage))}"
                        )
                    )
                );
            }

            _context.WorkerProfiles.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

        // GET: Edit Profile
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var worker = await _context.WorkerProfiles.FirstOrDefaultAsync(w => w.UserId == user.Id);
            if (worker == null) return RedirectToAction("Create");

            return View(worker);
        }

        // POST: Edit Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
     [Bind("WorkerProfileId,FullName,City,Profession,Experience,PhoneNumber,Address,HourlyRate,RatePerKm,Latitude,Longitude")] WorkerProfile model,
     IFormFile? ProfileImageFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var worker = await _context.WorkerProfiles.FirstOrDefaultAsync(w => w.UserId == user.Id);
            if (worker == null) return NotFound();

            // Update fields
            worker.FullName = model.FullName;
            worker.City = model.City;
            worker.Profession = model.Profession;
            worker.Experience = model.Experience;
            worker.PhoneNumber = model.PhoneNumber;
            worker.Address = model.Address;
            worker.HourlyRate = model.HourlyRate;
            worker.RatePerKm = model.RatePerKm;

            // Save location
            worker.Latitude = model.Latitude;
            worker.Longitude = model.Longitude;

            // Handle file upload
            if (ProfileImageFile != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(ProfileImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await ProfileImageFile.CopyToAsync(stream);

                worker.ProfileImageUrl = "/uploads/" + fileName;
            }

            _context.WorkerProfiles.Update(worker);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

        // GET: Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var worker = await _context.WorkerProfiles
                .Include(w => w.Jobs)
                    .ThenInclude(j => j.User)
                .FirstOrDefaultAsync(w => w.UserId == user.Id);

            if (worker == null)
                return RedirectToAction("Create");

            return View(worker);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptJob(int jobId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var worker = await _context.WorkerProfiles.FirstOrDefaultAsync(w => w.UserId == user.Id);
            if (worker == null) return Unauthorized();

            var job = await _context.Jobs.FirstOrDefaultAsync(j =>
                j.JobId == jobId &&
                j.WorkerProfileId == worker.WorkerProfileId);

            if (job == null) return NotFound();

            if (job.Status == JobStatus.Requested)
            {
                job.Status = JobStatus.Accepted;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteJob(int jobId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var worker = await _context.WorkerProfiles
                .FirstOrDefaultAsync(w => w.UserId == user.Id);

            if (worker == null) return Unauthorized();

            var job = await _context.Jobs.FirstOrDefaultAsync(j =>
                j.JobId == jobId &&
                j.WorkerProfileId == worker.WorkerProfileId);

            if (job == null)
                return NotFound();

            // ✅ Only ACCEPTED jobs can be completed
            if (job.Status != JobStatus.Accepted)
                return BadRequest("Job cannot be completed.");

            // ✅ Mark as completed
            job.Status = JobStatus.Completed;
            job.CompletedAt = DateTime.UtcNow; // OPTIONAL (recommended)

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

    }
}
