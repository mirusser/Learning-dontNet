using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using EcommService.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.DbModels;

namespace EcommService.Providers
{
    public class OrderDetailsProvider : IOrderDetailsProvider
    {
        private readonly ConnectionStrings _connectionStrings;
        private readonly ILogger<OrderDetailsProvider> _logger;

        public OrderDetailsProvider(
            IOptions<ConnectionStrings> connectionStrings,
            ILogger<OrderDetailsProvider> logger)
        {
            _connectionStrings = connectionStrings.Value;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionStrings.DefaultConnection);

            IEnumerable<OrderDetail> orderDetails;
            const string? sqlQuery =
                @"SELECT o.UserName AS [User], od.ProductName AS Name, od.Quantity
                FROM [Order] o JOIN [OrderDetail] od on o.Id = od.OrderId";

            try
            {
                orderDetails = await connection.QueryAsync<OrderDetail>(sqlQuery);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting order details: {Exception}", ex);
                orderDetails = new List<OrderDetail>();
            }

            return orderDetails;
        }
    }
}