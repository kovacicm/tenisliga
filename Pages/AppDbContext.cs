using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Players> Players { get; set; }
    public DbSet<Groups> Groups { get; set; }
    public DbSet<Matches> Matches { get; set; }
    public DbSet<Standings> Standings { get; set; }
    public DbSet<Seasons> Seasons { get; set; }

}
