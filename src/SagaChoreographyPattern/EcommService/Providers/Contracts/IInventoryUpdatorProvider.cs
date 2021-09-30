using System.Threading.Tasks;

namespace EcommService.Providers
{
    public interface IInventoryUpdatorProvider
    {
        Task<int> UpdateAsync(int productId, int quantity);
    }
}