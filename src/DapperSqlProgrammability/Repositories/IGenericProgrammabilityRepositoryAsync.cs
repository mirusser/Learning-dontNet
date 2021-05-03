using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperSqlProgrammability.Repositories
{
    public interface IGenericProgrammabilityRepositoryAsync
    {
        Task<dynamic> ExecuteScalarFunction(string functionName, Dictionary<string, string> paramsDictionary);
        Task<List<dynamic>> ExecuteStoredProcedure(string procedureName, Dictionary<string, string> paramsDictionary);
        Task<List<dynamic>> ExecuteTableFunction(string functionName, Dictionary<string, string> paramsDictionary);
    }
}