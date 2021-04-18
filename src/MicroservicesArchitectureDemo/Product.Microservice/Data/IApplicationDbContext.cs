using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Product.Microservice.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Entities.Product> Products { get; set; }

        Task<int> SaveChangesAsync();
    }
}