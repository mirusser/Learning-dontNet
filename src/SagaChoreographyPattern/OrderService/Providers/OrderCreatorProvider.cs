using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderService.Settings;
using Shared.DbModels;

namespace OrderService.Providers
{
    public class OrderCreatorProvider : IOrderCreatorProvider
    {
        private readonly ConnectionStrings _connectionStrings;
        private readonly ILogger<OrderCreatorProvider> _logger;

        public OrderCreatorProvider(
            IOptions<ConnectionStrings> connectionStrings,
            ILogger<OrderCreatorProvider> logger)
        {
            _connectionStrings = connectionStrings.Value;
            _logger = logger;
        }

        public async Task<int> CreateAsync(OrderDetail orderDetail)
        {
            using var connection = new SqlConnection(_connectionStrings.DefaultConnection);

            connection.Open();

            using var transaction = await connection.BeginTransactionAsync();
            try
            {
                var id =
                    await connection.QuerySingleAsync<int>(
                        "CREATE_ORDER",
                        new { userId = 1, userName = orderDetail.User },
                        transaction: transaction,
                        commandType: System.Data.CommandType.StoredProcedure);

                await connection.ExecuteAsync(
                    "CREATE_ORDER_DETAILS",
                    new { orderId = id, productId = orderDetail.ProductId, quantity = orderDetail.Quantity, productName = orderDetail.Name },
                    transaction: transaction,
                    commandType: System.Data.CommandType.StoredProcedure);

                transaction.Commit();

                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: {Exception}", ex);
                transaction.Rollback();

                return -1;
            }
        }
    }
}