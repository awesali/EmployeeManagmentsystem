using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;

    public AuthController(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Username == "admin" && request.Password == "password")
        {
            var token = _jwtService.GenerateToken(request.Username, "Admin");
            return Ok(new { Token = token });
        }

        return Unauthorized("Invalid credentials");
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
