using Microsoft.EntityFrameworkCore;

namespace SEP_Backend;

public class AppDbContext : DbContext
{
    public DbSet<User.User> Users { get; set; }
    public DbSet<Event.Event> Events { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=3334;Database=sep;Username=admin;Password=admin");
    }
}