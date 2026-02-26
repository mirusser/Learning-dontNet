using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RandomQuotesApi.Models;

namespace RandomQuotesApi.Repos;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Quote> Quotes => Set<Quote>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<Seen> SeenQuotes { get; set; } = null!;
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserPermission> UserPermissions => Set<UserPermission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        var categoriesConverter = new ValueConverter<string[], string>(
            v => string.Join(";", v ?? Array.Empty<string>()),
            v => (v ?? string.Empty)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
        );

        modelBuilder.Entity<Quote>()
            .Property(q => q.Categories)
            .HasConversion(categoriesConverter);

        modelBuilder.Entity<Seen>(b =>
        {
            b.HasKey(s => new { s.UserId, s.QuoteId });

            b.HasOne(s => s.User)
                .WithMany(u => u.SeenQuotes)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(s => s.Quote)
                .WithMany(q => q.SeenByUsers)
                .HasForeignKey(s => s.QuoteId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Property(s => s.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
        
        modelBuilder.Entity<Permission>(b =>
        {
            b.HasIndex(p => p.Name).IsUnique();
        });
        
        modelBuilder.Entity<UserPermission>(b =>
        {
            b.HasKey(up => new { up.UserId, up.PermissionId });

            b.HasOne(up => up.User)
                .WithMany(u => u.Permissions)
                .HasForeignKey(up => up.UserId);

            b.HasOne(up => up.Permission)
                .WithMany()
                .HasForeignKey(up => up.PermissionId);
        });
    }
}