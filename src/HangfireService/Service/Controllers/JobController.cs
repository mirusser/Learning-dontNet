using System;
using System.Threading.Tasks;
using HangfireService.Features.Commands;
using HangfireService.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HangfireService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class JobController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterJob(RegisterJobCommand request)
            => Ok(await _mediator.Send(request));

        [HttpGet]
        public Array GetAllJobTypes()
        {
            return Enum.GetValues(typeof(JobType));
        }
    }
}