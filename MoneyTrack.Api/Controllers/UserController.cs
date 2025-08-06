using Microsoft.AspNetCore.Mvc;

namespace MoneyTrack.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<string>> GetUsers()
    {
        // Add your implementation here
        return Ok("Users endpoint working");
    }
}