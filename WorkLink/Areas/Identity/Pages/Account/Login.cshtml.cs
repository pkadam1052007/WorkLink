using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkLink.Models;

namespace WorkLink.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [TempData]
        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // 🔑 Role selected from UI toggle (User / Worker)
            var selectedRole = Request.Form["SelectedRole"].ToString();

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            // 🔐 Password check
            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(
                user,
                Input.Password,
                lockoutOnFailure: false);

            if (!passwordCheck.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            var roles = await _userManager.GetRolesAsync(user);

            // 🧠 ADMIN BYPASS (NO UI TOGGLE REQUIRED)
            if (roles.Contains("Admin"))
            {
                await _signInManager.SignInAsync(user, Input.RememberMe);
                return RedirectToAction("Index", "Admin");
            }

            // 🔎 Normal role validation (User / Worker)
            if (!roles.Contains(selectedRole))
            {
                ModelState.AddModelError(string.Empty,
                    $"You are not registered as a {selectedRole}.");
                return Page();
            }

            // ✅ Sign in after role validation
            await _signInManager.SignInAsync(user, Input.RememberMe);

            // 🔁 Redirect by role
            if (selectedRole == "Worker")
                return RedirectToAction("Dashboard", "WorkerProfiles");

            return RedirectToAction("Index", "Home");
        }
    }
}
