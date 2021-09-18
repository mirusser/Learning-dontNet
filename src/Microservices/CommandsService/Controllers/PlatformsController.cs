using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> inbound post # command service");

            return Ok("Inbound test ok from Platforms Controller");
        }
    }
}