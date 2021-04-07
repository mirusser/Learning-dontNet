using CQRS_with_MediatR.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CQRS_with_MediatR.Context
{
    public interface IApplicationContext
    {
        DbSet<Product> Products { get; set; }

        Task<int> SaveChangesAsync();
    }
}