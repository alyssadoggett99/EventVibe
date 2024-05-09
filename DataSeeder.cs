using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using EventVibe.Models;


public class DataSeeder
{
    public static async Task SeedRoles(RoleManager<IdentityRole<int>> roleManager)
    {
        string[] roleNames = { "Administrator", "EventOrganizer", "GeneralUser" };
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                if (!roleResult.Succeeded)
                {
                    throw new Exception($"Failed to create role {roleName}");
                }
            }
        }
    }

    public static async Task SeedUsers(UserManager<ApplicationUser> userManager)
    {
        await CreateUser(userManager, "admin@eventvibe.com", "Admin123!", "Administrator");
        await CreateUser(userManager, "organizer@eventvibe.com", "Organizer123!", "EventOrganizer");
        await CreateUser(userManager, "user@eventvibe.com", null, "GeneralUser");
    }

    private static async Task CreateUser(UserManager<ApplicationUser> userManager, string email, string password, string role)
    {
        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            IdentityResult result;
            if (password != null)
            {
                result = await userManager.CreateAsync(user, password);
            }
            else
            {
                result = await userManager.CreateAsync(user);
            }

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
            else
            {
                throw new Exception($"Failed to create user {email} with role {role}: {result.Errors.FirstOrDefault()?.Description}");
            }
        }
    }
    public static async Task SeedEvents(EventVibeContext context, UserManager<ApplicationUser> userManager)
    {
        var organizer = await userManager.FindByEmailAsync("organizer@eventvibe.com");
        if (organizer == null)
        {

            throw new InvalidOperationException("Organizer must exist before seeding events.");
        }

        if (!context.Events.Any())
        {
            for (int i = 1; i <= 30; i++)
            {
                context.Events.Add(new Event
                {
                    OrganizerId = organizer.Id,
                    EventName = $"Event {i}",
                    Description = "This is a sample description for event " + i,
                    Location = "Location " + i,
                    Date = DateTime.Now.AddDays(i)
                });
            }
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedRegistrations(EventVibeContext context, UserManager<ApplicationUser> userManager)
    {
        if (!context.Registrations.Any())
        {
            var firstUser = await userManager.FindByEmailAsync("user@example.com");
            if (firstUser != null)
            {
                for (int i = 1; i <= 5; i++)
                {
                    context.Registrations.Add(new Registration
                    {
                        EventId = i,
                        UserId = firstUser.Id,
                        DateRegistered = DateTime.Now.AddDays(-i)
                    });
                }
                await context.SaveChangesAsync();
            }
        }
    }

    public static async Task SeedSurveys(EventVibeContext context, UserManager<ApplicationUser> userManager)
    {
        if (!context.Surveys.Any())
        {
            var firstUser = await userManager.FindByEmailAsync("user@example.com");
            if (firstUser != null)
            {
                for (int i = 1; i <= 5; i++)
                {
                    context.Surveys.Add(new Survey
                    {
                        EventId = i,
                        UserId = firstUser.Id,
                        CommentDetails = "This was a great event " + i,
                        Rating = i % 5 + 1
                    });
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
