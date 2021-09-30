using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using OrderService.Settings;

namespace OrderService.Providers
{
    public class OrderDeletorProvider : IOrderDeletorProvider
    {
        private readonly ConnectionStrings _connectionStrings;

        public OrderDeletorProvider(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
        }

        public async Task DeleteAsync(int orderId)
        {
            using var connection = new SqlConnection(_connectionStrings.DefaultConnection);

            connection.Open();
            using var transaction = connection.BeginTransaction();

            const string? sqlDeleteOrderDetailQuery = "DELETE FROM OrderDetail WHERE OrderId = @orderId";
            const string? sqlDeleteOrderQuery = "DELETE FROM [Order] WHERE Id = @orderId";

            try
            {
                await connection.ExecuteAsync(sqlDeleteOrderDetailQuery, new { orderId }, transaction: transaction);
                await connection.ExecuteAsync(sqlDeleteOrderQuery, new { orderId }, transaction: transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }
    }
}