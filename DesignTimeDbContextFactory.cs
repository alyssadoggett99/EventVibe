using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EventVibeContext>
{
    public EventVibeContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EventVibeContext>();

        // Configuring the DbContext to use SQLite
        optionsBuilder.UseSqlite("Data Source=EventVibe.db");

        // Returning the DbContext with options configured
        return new EventVibeContext(optionsBuilder.Options);
    }
}
