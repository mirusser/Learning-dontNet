using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using EcommService.Settings;
using Microsoft.Extensions.Options;
using Shared.DbModels;

namespace EcommService.Providers
{
    public class ProductProvider : IProductProvider
    {
        private readonly ConnectionStrings _connectionStrings;

        public ProductProvider(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionStrings.DefaultConnection);

            const string? sqlQuery = "SELECT Id, Name, Description, Type FROM Product";

            return await connection.QueryAsync<Product>(sqlQuery);
        }
    }
}