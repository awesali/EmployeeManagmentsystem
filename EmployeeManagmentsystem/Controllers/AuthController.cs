using EmployeeManagmentsystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagmentsystem.Models
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly EmployeeDb _dbContext;

        public AuthController(JwtService jwtService, EmployeeDb dbContext)
        {
            _jwtService = jwtService;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User request)
        {
            // Fetch user from database based on username
            var user = await _dbContext.user.FirstOrDefaultAsync(u => u.Username == request.Username);
           
            if (user == null || user.PasswordHash != request.PasswordHash)
            {
                return Unauthorized("Invalid credentials");
            }

            // Generate JWT Token
            var token = _jwtService.GenerateToken(user.Username);
            return Ok(new { Token = token });
        }
    }
}
