using System.Collections.Generic;
using System.Threading.Tasks;
using OrderService.Models;

namespace OrderService.Providers
{
    public interface IOrderDetailsProvider
    {
        Task<IEnumerable<OrderDetail>> GetAll();
    }
}