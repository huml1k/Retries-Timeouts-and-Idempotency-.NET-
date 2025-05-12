using APIGateway.IdempotencyDb.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIGateway.Infrastructure
{
    public class JwtProvider(IOptions<JwtOption> options)
    {
        private readonly JwtOption _option = options.Value;

        public string GenerateToken(UserEntity userEntity)
        {
            Claim[] claims = [new("userid", userEntity.Id.ToString())];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_option.SercretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_option.ExpiresHours));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
