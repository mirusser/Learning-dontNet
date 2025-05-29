using Core.Contracts;
using Core.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ProductService(ApplicationDbContext context) : IProductService
{
    public async Task<Product> CreateAsync(string name, string description, int rate)
    {
        Product product = new(name, description, rate);
        context.Products.Add(product);
        await context.SaveChangesAsync();
        
        return product;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync()
    {
        return await context.Products.ToListAsync();
    }
}