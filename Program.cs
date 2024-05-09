using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using EventVibe.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configuration from appsettings.json for the database connection
var connectionString = builder.Configuration.GetConnectionString("EventVibeContext");

// Add DbContext using SQLite Provider
builder.Services.AddDbContext<EventVibeContext>(options =>
    options.UseSqlite(connectionString));

// Add Identity services and configure identity options
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddRoles<IdentityRole<int>>()
.AddEntityFrameworkStores<EventVibeContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

// Seeding database with roles and users
await SeedDatabaseAsync(app);

app.Run();

async Task SeedDatabaseAsync(IHost app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var context = services.GetRequiredService<EventVibeContext>();

    // Ensure the database is created
    await context.Database.EnsureCreatedAsync();

    if (!await roleManager.RoleExistsAsync("Administrator"))
    {
        await DataSeeder.SeedRoles(roleManager);
    }
    if (await userManager.FindByEmailAsync("admin@eventvibe.com") == null)
    {
        await DataSeeder.SeedUsers(userManager);
    }
    await DataSeeder.SeedRoles(services.GetRequiredService<RoleManager<IdentityRole<int>>>());
    await DataSeeder.SeedUsers(userManager);
    await DataSeeder.SeedEvents(context, userManager);  
    await DataSeeder.SeedRegistrations(context, userManager);
    await DataSeeder.SeedSurveys(context, userManager);
}
