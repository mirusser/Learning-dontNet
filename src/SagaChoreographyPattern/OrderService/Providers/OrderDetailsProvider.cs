using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using OrderService.Models;
using OrderService.Settings;

namespace OrderService.Providers
{
    public class OrderDetailsProvider : IOrderDetailsProvider
    {
        private readonly ConnectionStrings _connectionStrings;

        public OrderDetailsProvider(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
        }

        public async Task<IEnumerable<OrderDetail>> GetAll()
        {
            using var connection = new SqlConnection(_connectionStrings.DefaultConnection);

            const string? sqlQuery =
                @"SELECT o.UserName AS [User], od.ProductName AS Name, od.Quantity
                FROM [Order] o JOIN [OrderDetail] od on o.Id = od.OrderId";

            return await connection.QueryAsync<OrderDetail>(sqlQuery);
        }
    }
}