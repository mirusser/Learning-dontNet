using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperSqlProgrammability.Parameters
{
    public class ScalarFunctionParameters
    {
        public string FunctionName { get; set; }
        public Dictionary<string, string> ParamsDictionary { get; set; }
    }
}
