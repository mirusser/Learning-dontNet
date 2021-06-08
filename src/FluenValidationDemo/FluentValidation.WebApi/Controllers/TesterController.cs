using ExternalModels.Library;
using FluentValidation.Results;
using FluentValidation.WebApi.Models;
using FluentValidation.WebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentValidation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TesterController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            TesterValidator validator = new();
            List<string> ValidationMessages = new();

            var tester = new Tester
            {
                FirstName = "",
                Email = "bla!"
            };

            var validationResult = validator.Validate(tester);
            var response = new ResponseModel();

            if (!validationResult.IsValid)
            {
                response.IsValid = false;
                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    ValidationMessages.Add(failure.ErrorMessage);
                }

                response.ValidationMessages = ValidationMessages;
            }

            return Ok(response);
        }
    }
}
