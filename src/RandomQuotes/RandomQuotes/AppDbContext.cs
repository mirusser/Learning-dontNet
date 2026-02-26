using RandomQuotes.Models;

namespace RandomQuotes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class AppDbContext : DbContext
{
    public DbSet<Quote> Quotes => Set<Quote>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Favorite> Favorites => Set<Favorite>();

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     // Very simple config for now: local SQLite file
    //     optionsBuilder.UseSqlite("Data Source=app.db");
    // }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //uses db from project directory not from bin/debug
        var dbPath = Path.Combine(AppContext.BaseDirectory, "app.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Composite key for Favorite
        modelBuilder.Entity<Favorite>()
            .HasKey(f => new { f.UserId, f.QuoteId });

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(f => f.UserId);

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.Quote)
            .WithMany(q => q.Favorites)
            .HasForeignKey(f => f.QuoteId);

        // Value converter for Categories (string[] ⇔ string)
        var categoriesConverter = new ValueConverter<string[], string>(
            v => string.Join(";", v ?? Array.Empty<string>()),
            v => (v ?? string.Empty)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
        );

        modelBuilder.Entity<Quote>()
            .Property(q => q.Categories)
            .HasConversion(categoriesConverter);
    }
}
