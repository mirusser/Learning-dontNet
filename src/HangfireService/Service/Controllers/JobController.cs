﻿using System;
using System.Threading.Tasks;
using HangfireService.Features.Commands;
using HangfireService.Features.Queries;
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

        [HttpPost]
        public async Task<IActionResult> GetAllJobTypes(GetAllJobTypeQuery request)
            => Ok(await _mediator.Send(request));
    }
}