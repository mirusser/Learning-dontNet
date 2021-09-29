using System.Threading.Tasks;
using OrderService.Models;

namespace OrderService.Providers
{
    public interface IOrderCreatorProvider
    {
        Task<int> Create(OrderDetail orderDetail);
    }
}