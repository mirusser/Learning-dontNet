﻿using Core.Features.CustomerFeatures.Queries;
using Core.Features.CustomerFetures.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        var customer = await _mediator.Send(new GetCustomerQuery { Id = id, BypassCache = false });
        return Ok(customer);
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomerList()
    {
        var customers = await _mediator.Send(new GetCustomerListQuery { BypassCache = false });
        return Ok(customers);
    }
}