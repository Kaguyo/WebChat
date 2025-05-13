using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("secure")]
    [Authorize]
    public IActionResult SecureEndpoint()
    {
        var user = User.Identity?.Name;
        return Ok($"Olá {user}, você acessou uma rota protegida!");
    }
}
