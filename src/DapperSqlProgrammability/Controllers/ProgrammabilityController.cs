using DapperSqlProgrammability.Parameters;
using DapperSqlProgrammability.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperSqlProgrammability.Controllers
{
    /// <summary>
    /// Controller to execute stored procedures, scalar functions, table functions
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProgrammabilityController : ControllerBase
    {
        private readonly IGenericProgrammabilityRepositoryAsync _dapperRepo;

        public ProgrammabilityController(IGenericProgrammabilityRepositoryAsync dapperRepo)
        {
            _dapperRepo = dapperRepo;
        }

        /// <summary>
        /// Executes stored procedure
        /// </summary>
        /// <param name="procedureParameters"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ExecuteStoredProcedure(ProcedureParameters procedureParameters)
        {
            var result = await _dapperRepo.ExecuteStoredProcedure(
                procedureParameters.ProcedureName,
                procedureParameters.ParamsDictionary);

            return Ok(result);
        }

        /// <summary>
        /// Executes scalar function
        /// </summary>
        /// <param name="functionParameters"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ExecuteScalarFunction(ScalarFunctionParameters functionParameters)
        {
            var result = await _dapperRepo.ExecuteScalarFunction(
                functionParameters.FunctionName,
                functionParameters.ParamsDictionary);

            return Ok(result);
        }

        /// <summary>
        /// Executes table function
        /// </summary>
        /// <param name="functionParameters"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ExecuteTableFunction(TableFunctionParameters functionParameters)
        {
            var result = await _dapperRepo.ExecuteTableFunction(
                functionParameters.FunctionName,
                functionParameters.ParamsDictionary);

            return Ok(result);
        }
    }
}
