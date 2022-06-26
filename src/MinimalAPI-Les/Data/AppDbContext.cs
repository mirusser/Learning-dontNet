using Microsoft.EntityFrameworkCore;
using MinimalAPI_Les.Models;

namespace MinimalAPI_Les.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Command> Commands => Set<Command>();
}