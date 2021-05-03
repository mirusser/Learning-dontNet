using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperSqlProgrammability.Parameters
{
    public class ProcedureParameters
    {
        public string ProcedureName { get; set; }
        public Dictionary<string, string> ParamsDictionary { get; set; }
    }
}
