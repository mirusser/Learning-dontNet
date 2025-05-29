using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public string TenantId { get; set; }
    private readonly ITenantService tenantService;
    
    public ApplicationDbContext(DbContextOptions options, ITenantService tenantService) : base(options)
    {
        this.tenantService = tenantService;
        TenantId = this.tenantService.GetTenant()?.Id;
    }
    
    public DbSet<Product> Products { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasQueryFilter(a => a.TenantId == TenantId);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var tenantConnectionString = tenantService.GetConnectionString();
        if (!string.IsNullOrEmpty(tenantConnectionString))
        {
            var dbProvider = tenantService.GetDatabaseProvider();
            if (dbProvider.ToLower() == "mssql")
            {
                optionsBuilder.UseSqlServer(tenantService.GetConnectionString());
            }
        }
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<ITenant>().ToList())
        {
            entry.Entity.TenantId = entry.State switch
            {
                EntityState.Added or EntityState.Modified => TenantId,
                _ => entry.Entity.TenantId
            };
        }
        
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}