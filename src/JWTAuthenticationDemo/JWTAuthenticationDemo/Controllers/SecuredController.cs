using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthenticationDemo.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class SecuredController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSecuredData()
    {
        return Ok("This Secured Data is available only for Authenticated Users.");
    }
    
    [HttpPost]
    [Authorize(Roles ="Administrator")]
    public async Task<IActionResult> PostSecuredData()
    {
        return Ok("This Secured Data is available only for Authenticated and Authorized Users.");
    }
}