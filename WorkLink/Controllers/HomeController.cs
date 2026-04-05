using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkLink.Data;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Credits()
    {
        return View();
    }


    // ?? SEARCH + LIST
    public IActionResult Index(string? profession, string? city)
    {
        var workers = _context.WorkerProfiles
            .Include(w => w.User)
            .OrderByDescending(w => w.Rating)
            .AsQueryable();

        // EASTER EGGS
        if (!string.IsNullOrWhiteSpace(profession))
        {
            var profLower = profession.ToLower();

            if (profLower == "prabhav")
            {
                TempData["EasterEgg"] = " Hey, you found Prabhav’s secret!";
            }
            else if (profLower == "ismail")
            {
                TempData["EasterEgg"] = " Ismail did… absolutely nothing! (Aloo khaoge?)";
            }
            else if (profLower == "purvesh")
            {
                TempData["EasterEgg"] = " Maa Ka Boshda Aggg! (Purvesh strikes again!)";
            }
        }

        if (!string.IsNullOrWhiteSpace(profession))
        {
            workers = workers.Where(w =>
                w.Profession.ToLower().Contains(profession.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            workers = workers.Where(w =>
                w.City.ToLower().Contains(city.ToLower()));
        }

        return View(workers.ToList());
    }



    // ? FIXED PROFILE ACTION (REVIEWS INCLUDED)
    public IActionResult Profile(int id)
    {
        var worker = _context.WorkerProfiles
            .Include(w => w.User) // Worker’s own user
            .Include(w => w.Reviews)
                .ThenInclude(r => r.User) // ? This loads the review authors
            .FirstOrDefault(w => w.WorkerProfileId == id);

        if (worker == null)
            return NotFound();

        return View(worker);
    }

    public IActionResult Privacy() => View();
    public IActionResult About() => View();
    public IActionResult Contact() => View();
}



