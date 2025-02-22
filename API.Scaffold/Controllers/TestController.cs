using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Scaffold.Controllers;

[Authorize]
[ApiController]
public class TestController(ILogger<TestController> logger) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("test")]
    public ActionResult TestUnauthenticated()
    {
        logger.LogInformation("Test");
        return Ok();
    }
    
    [HttpGet("test/auth")]
    public ActionResult TestAuthenticated()
    {
        logger.LogInformation("Authenticated Test");
        return Ok();
    }
}