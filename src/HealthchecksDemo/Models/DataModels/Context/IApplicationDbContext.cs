using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HealthchecksDemo.Models.DataModels.Context
{
    public interface IApplicationDbContext
    {
        DbSet<Student> Students { get; set; }

        Task<int> SaveChangesAsync();
    }
}