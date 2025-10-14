using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace SEP_Backend.User;

public class JwtTokenGenerator(JwtConfig config)
{
    public string Generate(Guid userId)
    {
        var credentials = new SigningCredentials(config.GetSigningKey(), SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString())
        };
        var token = new JwtSecurityToken(
            issuer: config.GetIssuer(),
            audience: config.GetAudience(),
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}