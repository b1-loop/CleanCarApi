using CleanCarApi.Application.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanCarApi.Application.Auth.Handlers;

// Hanterar inloggning och generering av JWT-token
public class LoginHandler : IRequestHandler<LoginCommand, string>
{
    private readonly UserManager<IdentityUser> _userManager;

    // Configuration används för att läsa JWT-inställningar från appsettings.json
    private readonly IConfiguration _configuration;

    public LoginHandler(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        // Kontrollerar att användaren finns
        var user = await _userManager.FindByNameAsync(request.Dto.Username);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid username or password");

        // Kontrollerar att lösenordet stämmer
        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Dto.Password);
        if (!passwordValid)
            throw new UnauthorizedAccessException("Invalid username or password");

        // Hämtar användarens roller för att inkludera i token
        var roles = await _userManager.GetRolesAsync(user);

        // Bygger upp claims — information som läggs in i JWT-token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        // Lägger till varje roll som ett eget claim
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // Skapar signeringsnyckeln från appsettings.json
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Skapar själva JWT-token med claims och utgångstid
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);

        // Serialiserar token till en sträng som returneras till klienten
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

