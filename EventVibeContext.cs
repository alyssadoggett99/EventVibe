using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using EventVibe.Models;

public class EventVibeContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public EventVibeContext(DbContextOptions<EventVibeContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<Registration> Registrations { get; set; }
    public DbSet<Survey> Surveys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ensure all navigation properties and foreign key relationships are correctly configured
        modelBuilder.Entity<Event>()
        .HasOne(e => e.Organizer)
        .WithMany()
        .HasForeignKey(e => e.OrganizerId)
        .IsRequired();

        modelBuilder.Entity<Registration>()
            .HasOne(r => r.Event)
            .WithMany(e => e.Registrations)
            .HasForeignKey(r => r.EventId);

        modelBuilder.Entity<Registration>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Survey>()
            .HasOne(s => s.Event)
            .WithMany(e => e.Surveys)
            .HasForeignKey(s => s.EventId);

        modelBuilder.Entity<Survey>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId);
    }
}
