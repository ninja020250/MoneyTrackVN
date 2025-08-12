using Microsoft.AspNetCore.Mvc;

namespace MoneyTrack.Api.Controllers;

[Route("api/health")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    [HttpGet("check")]
    public ActionResult<string> HealthCheck()
    {
        return Ok("Welcome to MoneyTrack API");
    }
}