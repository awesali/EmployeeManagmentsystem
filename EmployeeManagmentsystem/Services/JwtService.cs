using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService
{
   

    public string GenerateToken(string Username)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, Username),
           
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jdfoiogfxfcghilkfjgxhciuojfhxgbnbcmvbjyf"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
