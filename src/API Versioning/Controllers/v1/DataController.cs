using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Versioning.Controllers.v1
{
    //Deprecating an API Version
    [ApiVersion("1.0", Deprecated = true)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")] //versioning via URL
    [Route("api/[controller]")] //versioning via query
    public class DataController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "data from api v1";
        }
    }
}
