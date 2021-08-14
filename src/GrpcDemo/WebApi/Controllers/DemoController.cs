using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcServer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly IDemoService demoService;

        public DemoController(IDemoService demoService)
        {
            this.demoService = demoService;
        }

        //[HttpGet]
        //public async IAsyncEnumerable<HelloReply> Get()
        //{
        //    await foreach (var helloReply in demoService.SayHellos())
        //    {
        //        yield return helloReply;
        //    }
        //}

        [HttpGet]
        public async Task Get()
        {
            await demoService.SayHello();
        }
    }
}
