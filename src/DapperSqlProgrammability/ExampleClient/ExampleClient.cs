using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DapperSqlProgrammability.ExampleClient
{
    public class ExampleClient 
    {
        public ExampleClient() 
        {
        }

        #region Queries
        #endregion

        #region Commands

        public async Task<List<dynamic>> ExecuteStoredProcedure(string procedureName, Dictionary<string, string> paramsDictionary)
        {
            var requestUri = $"<REQUEST_URL_HERE>";

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("<BASE_ADDRESS_HERE>");
                var response = await httpClient.PostAsJsonAsync(
                    requestUri,
                    new
                    {
                        procedureName,
                        paramsDictionary
                    });

                var contents = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<dynamic>>(contents);

                return result;
            }
        }

        public async Task<dynamic> ExecuteScalarFunction(string functionName, Dictionary<string, string> paramsDictionary)
        {
            var requestUri = $"<REQUEST_URL_HERE>";

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("<BASE_ADDRESS_HERE>");
                var response = await httpClient.PostAsJsonAsync(
                    requestUri,
                    new
                    {
                        functionName,
                        paramsDictionary
                    });

                var contents = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(contents);

                return result;
            }
        }

        public async Task<List<dynamic>> ExecuteTableFunction(string functionName, Dictionary<string, string> paramsDictionary)
        {
            var requestUri = $"<REQUEST_URL_HERE>";

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("<BASE_ADDRESS_HERE>");
                var response = await httpClient.PostAsJsonAsync(
                    requestUri,
                    new
                    {
                        functionName,
                        paramsDictionary
                    });

                var contents = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<dynamic>>(contents);

                return result;
            }
        }
        #endregion
    }
}
