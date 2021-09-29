using System.Threading.Tasks;

namespace OrderService.Providers
{
    public interface IOrderDeletorProvider
    {
        Task Delete(int orderId);
    }
}