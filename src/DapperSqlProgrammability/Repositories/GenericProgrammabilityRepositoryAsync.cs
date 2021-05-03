using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperSqlProgrammability.Repositories
{
    public class GenericProgrammabilityRepositoryAsync : IGenericProgrammabilityRepositoryAsync
    {
        private readonly IConfiguration _config;
        private IDbConnection _connection => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        public GenericProgrammabilityRepositoryAsync(IConfiguration config)
        {
            _config = config;
        }

        private Dictionary<string, object> CastParamsDictionary(Dictionary<string, string> paramsDictionary)
        {
            //Casting 'paramsDictionary' string values to object values is necessery
            //because argument 'paramsDictionary' comes here from reqeust as a serialized JSON,
            //but serializing object (in this example values of paramsDictionaty) do weird stuff to it (you can't really properly serialize 'object'),
            //thats why its values are passed as a string not as an object,
            //but they can't be of string type for 'connection.Query(...)' methods, thus that's why casting

            //I know it's a bit wacky, but I can't come up with a better solution as of now

            Dictionary<string, object> properParamsDictionary = new();

            foreach (var item in paramsDictionary)
            {
                properParamsDictionary.Add(item.Key, (object)item.Value);
            }

            return properParamsDictionary;
        }

        public async Task<List<dynamic>> ExecuteTableFunction(string functionName, Dictionary<string, string> paramsDictionary)
        {
            List<dynamic> result = null;

            if (!string.IsNullOrEmpty(functionName) && paramsDictionary != null)
            {
                var properParamsDictionary = CastParamsDictionary(paramsDictionary);

                var listOfParams = new List<string>(paramsDictionary.Keys);
                var sql = $"SELECT * FROM {functionName}({string.Join(",", listOfParams)})";

                using var connection = _connection;

                result = (await connection.QueryAsync(
                     sql,
                     new DynamicParameters(properParamsDictionary),
                     commandType: CommandType.Text)).ToList();
            }

            return result;
        }

        public async Task<dynamic> ExecuteScalarFunction(string functionName, Dictionary<string, string> paramsDictionary)
        {
            dynamic result = null;

            if (!string.IsNullOrEmpty(functionName) && paramsDictionary != null)
            {
                var properParamsDictionary = CastParamsDictionary(paramsDictionary);

                var listOfParams = new List<string>(paramsDictionary.Keys);
                var sql = $"SELECT {functionName}({string.Join(",", listOfParams)})";

                using var connection = _connection;

                result = await connection.ExecuteScalarAsync(
                    sql,
                    new DynamicParameters(properParamsDictionary),
                    commandType: CommandType.Text);
            }

            return result;
        }

        public async Task<List<dynamic>> ExecuteStoredProcedure(string procedureName, Dictionary<string, string> paramsDictionary)
        {
            List<dynamic> result = null;

            if (!string.IsNullOrEmpty(procedureName) && paramsDictionary != null)
            {
                var properParamsDictionary = CastParamsDictionary(paramsDictionary);

                using var connection = _connection;

                result = (await connection.QueryAsync(procedureName, new DynamicParameters(properParamsDictionary), commandType: CommandType.StoredProcedure)).ToList();
            }

            return result;
        }
    }
}
