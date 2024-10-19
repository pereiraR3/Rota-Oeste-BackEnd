using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using api_rota_oeste.Data;
using api_rota_oeste.Models.Token;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApiDbContext _context;
    public AuthController(IConfiguration configuration, ApiDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        var validate = _context.Usuarios.FirstOrDefault(u 
            => u.Nome == login.Username && u.Senha == login.Password);
        if (validate != null)
        {
            var accessToken = GenerateAccessToken(login.Username);
            var refreshToken = GenerateRefreshToken();
    
            // Salvar o refresh token no banco de dados
            var refreshTokenEntity = new TokenModel()
            {
                Token = refreshToken,
                Username = login.Username,
                Expiration = DateTime.UtcNow.AddDays(_configuration.GetSection("TokenSettings").Get<TokenSettings>().RefreshTokenExpirationInDays),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            var response = new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return Ok(response);
        }

        return Unauthorized();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);
        
        if (refreshToken == null || refreshToken.IsRevoked || refreshToken.Expiration.ToUniversalTime() < DateTime.UtcNow.ToUniversalTime()) // ||  refreshToken.Expiration < DateTime.UtcNow
        {
            return Unauthorized("Invalid or expired refresh token.");
        }
        
        // Revogar o refresh token atual
        refreshToken.IsRevoked = true;
        _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync();

        // Gerar novos tokens
        var accessToken = GenerateAccessToken(refreshToken.Username);
        var newRefreshToken = GenerateRefreshToken();

        var newRefreshTokenEntity = new TokenModel()
        {
            Token = newRefreshToken,
            Username = refreshToken.Username,
            Expiration = DateTime.UtcNow.AddDays(_configuration.GetSection("TokenSettings").Get<TokenSettings>().RefreshTokenExpirationInDays),
            IsRevoked = false
        };
        
        _context.RefreshTokens.Add(newRefreshTokenEntity);
        await _context.SaveChangesAsync();

        var response = new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };

        return Ok(response);
    }

    private string GenerateAccessToken(string username)
    {
        var tokenSettings = _configuration.GetSection("TokenSettings").Get<TokenSettings>();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Definir as claims do token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        // Criar o token
        var token = new JwtSecurityToken(
            issuer: tokenSettings.Issuer,
            audience: tokenSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(tokenSettings.TokenExpirationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}

public class RefreshRequest
{
    public string RefreshToken { get; set; }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class AuthResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}