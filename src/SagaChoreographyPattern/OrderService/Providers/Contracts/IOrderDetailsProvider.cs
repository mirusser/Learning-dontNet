using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DbModels;

namespace OrderService.Providers
{
    public interface IOrderDetailsProvider
    {
        Task<IEnumerable<OrderDetail>> GetAll();
    }
}