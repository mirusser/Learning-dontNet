using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DbModels;

namespace EcommService.Providers
{
    public interface IProductProvider
    {
        Task<IEnumerable<Product>> GetAllAsync();
    }
}