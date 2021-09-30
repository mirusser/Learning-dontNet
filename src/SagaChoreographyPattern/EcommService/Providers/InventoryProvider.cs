using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using EcommService.Settings;
using Microsoft.Extensions.Options;
using Shared.DbModels;

namespace EcommService.Providers
{
    public class InventoryProvider : IInventoryProvider
    {
        private readonly ConnectionStrings _connectionStrings;

        public InventoryProvider(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionStrings.DefaultConnection);

            const string? sqlQuery = "SELECT Id, Name, Quantity, ProductId FROM Inventory";

            return await connection.QueryAsync<Inventory>(sqlQuery);
        }
    }
}