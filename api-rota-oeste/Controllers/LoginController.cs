using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_rota_oeste.Models.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace api_rota_oeste.Controllers;

[ApiController]
[Route("login")]
public class LoginController : ControllerBase
{

    private IConfiguration _config;

    public LoginController(IConfiguration config)
    {
        _config = config;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult login([FromBody] UsuarioLogin userLogin)
    {
        var user = Authenticate(userLogin);

        if (user != null)
        {
            var token = Generate(user);
            return Ok(token);
        }
        return NotFound("Usuario nao encontrado");
    }

    private string Generate(UsuarioModel user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Nome),
            //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private UsuarioModel Authenticate(UsuarioLogin userLogin)
    {
        var usuarioAtual = UsuariosTestes.usuarios.FirstOrDefault(o => o.Nome.ToLower()
            == userLogin.Login.ToLower() && o.Senha == userLogin.Senha);
        
        if(usuarioAtual != null) return usuarioAtual;

        return null;

    }
}