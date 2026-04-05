using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkLink.Data;
using WorkLink.Models;
using WorkLink.Services; // ? ADD THIS

var builder = WebApplication.CreateBuilder(args);

// ===============================
// DATABASE
// ===============================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ===============================
// IDENTITY
// ===============================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

// ===============================
// EMAIL SERVICE ?? REQUIRED
// ===============================
builder.Services.AddScoped<IEmailService, EmailService>();

// ===============================
// MVC
// ===============================
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ===============================
// SEED DATA
// ===============================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.InitializeAsync(services);
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
