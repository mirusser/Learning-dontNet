using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EcommService.Settings;
using Microsoft.Extensions.Options;

namespace EcommService.Providers
{
    public class InventoryUpdatorProvider : IInventoryUpdatorProvider
    {
        private readonly ConnectionStrings _connectionStrings;

        public InventoryUpdatorProvider(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
        }

        public async Task<int> UpdateAsync(int productId, int quantity)
        {
            using var connection = new SqlConnection(_connectionStrings.DefaultConnection);

            const string? sqlQuery = "UPDATE_INVENTORY";

            return await connection.ExecuteAsync(sqlQuery, new { productId, quantity }, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}