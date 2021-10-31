using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace AspNetCoreRedisDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromServices] IDistributedCache cache)
        {
            return Ok(await cache.GetStringAsync("rat"));
        }
    }
}