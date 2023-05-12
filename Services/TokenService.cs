using Bamboo.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bamboo.Services
{
    public class TokenService
    {
        public string GenerateToken(User user)
        {
            Claim[] claims = new Claim[] 
            { 
                new Claim("username", user.UserName),
                new Claim("id", user.Id),
                new Claim("userFirstName", user.userFirstName),
                new Claim("isActive", user.isActive.ToString()),
                new Claim("loginTimeStamp", DateTime.UtcNow.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("9ASHDA98H9ah9ha9H9A89n0f"));

            var signingCredentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(1),
                claims: claims,
                signingCredentials : signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
