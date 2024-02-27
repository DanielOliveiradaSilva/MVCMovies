using System.Text;
using MvcMovie.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
namespace MvcMovie.Service;

public class TokenService
{
    public static Object GenerateToken(User user)
    {
        var key = Encoding.ASCII.GetBytes("123456");
        var tokenConfig = new SecurityTokenDescriptor
        {
           Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]{
               new Claim("UserId", user.Id.ToString()),
           }),
           Expires = DateTime.UtcNow.AddHours(1),
           SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenConfig);
        var tokenString = tokenHandler.WriteToken(token);
        return new { token = tokenString };
    }
}
