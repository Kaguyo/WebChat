using Microsoft.AspNetCore.Mvc;
using Server.Domain.Entities;
using Server.Helpers;
using Server.UseCases;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase 
{
    private readonly IConfiguration _config;


    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserUseCase userUseCase, User user)
    
    {
        var Login = await userUseCase.GetUserByNumber(user.Number, user.Password);
        Console.WriteLine("DENTRO DO AUTH", Login);
        if (Login == null) return Unauthorized();

        var token = JwtHelper.GenerateToken(user.Number, _config["Jwt:Key"]!);
        return Ok(new { token });
    }
}
