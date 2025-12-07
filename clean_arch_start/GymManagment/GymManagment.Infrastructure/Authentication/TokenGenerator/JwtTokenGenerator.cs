using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GymManagment.Application.Common.Interfaces;
using GymManagment.Domain.Users;
using GymManagment.Infrastructure.Authentication.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GymManagment.Infrastructure.Authentication.TokenGenerator;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Name, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("id", user.Id.ToString()),
            new Claim("permissions", "gyms:create"),
            new Claim("permissions", "gyms:update"),
        };
        AddIds(user, claims);
        AddRoles(user, claims);

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private static void AddIds(User user, List<Claim> claims)
    {
        claims
            .AddIfValueNotNull("adminId", user.AdminId?.ToString())
            .AddIfValueNotNull("trainerId", user.TrainerId?.ToString())
            .AddIfValueNotNull("participantId", user.ParticipantId?.ToString());
    }

    private static void AddRoles(User user, List<Claim> claims)
    {
        user.GetProfileTypes().ForEach(type =>
        {
            claims.Add(new Claim("roles", type.Name));
        });
    }
}