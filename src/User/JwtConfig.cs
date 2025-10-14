using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SEP_Backend.User;

public class JwtConfig(IConfiguration config)
{
    public string GetIssuer() => config["Jwt:Issuer"] ?? string.Empty;
    public string GetAudience() => config["Jwt:Audience"] ?? string.Empty;
    public SymmetricSecurityKey GetSigningKey() => new(Encoding.UTF8.GetBytes(config["Jwt:Secret"] ?? string.Empty));
}