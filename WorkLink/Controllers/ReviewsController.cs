using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkLink.Data;
using WorkLink.Models;

[Authorize]
public class ReviewsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReviewsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET
    public async Task<IActionResult> Create(int jobId)
    {
        var user = await _userManager.GetUserAsync(User);

        var job = await _context.Jobs
            .FirstOrDefaultAsync(j =>
                j.JobId == jobId &&
                j.UserId == user.Id &&
                j.Status == JobStatus.Completed);

        if (job == null) return Unauthorized();

        if (await _context.Reviews.AnyAsync(r => r.JobId == jobId))
            return RedirectToAction("MyBookings", "Jobs");

        return View(new Review
        {
            JobId = job.JobId,
            WorkerProfileId = job.WorkerProfileId
        });
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Review model)
    {
        var user = await _userManager.GetUserAsync(User);

        var job = await _context.Jobs
            .FirstOrDefaultAsync(j =>
                j.JobId == model.JobId &&
                j.UserId == user.Id &&
                j.Status == JobStatus.Completed);

        if (job == null) return Unauthorized();

        model.UserId = user.Id;
        model.WorkerProfileId = job.WorkerProfileId;

        _context.Reviews.Add(model);
        await _context.SaveChangesAsync();

        await UpdateWorkerRating(job.WorkerProfileId);

        return RedirectToAction("Profile", "Home",
            new { id = job.WorkerProfileId });
    }

    // 🗑 DELETE REVIEW
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int reviewId)
    {
        var user = await _userManager.GetUserAsync(User);

        var review = await _context.Reviews
            .FirstOrDefaultAsync(r => r.ReviewId == reviewId && r.UserId == user.Id);

        if (review == null) return Unauthorized();

        int workerId = review.WorkerProfileId;

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        await UpdateWorkerRating(workerId);

        return RedirectToAction("Profile", "Home", new { id = workerId });
    }

    private async Task UpdateWorkerRating(int workerProfileId)
    {
        var avg = await _context.Reviews
            .Where(r => r.WorkerProfileId == workerProfileId)
            .Select(r => (double?)r.Rating)
            .AverageAsync() ?? 0;

        var worker = await _context.WorkerProfiles.FindAsync(workerProfileId);
        if (worker != null)
        {
            worker.Rating = Math.Round(avg, 1);
            await _context.SaveChangesAsync();
        }
    }
}
