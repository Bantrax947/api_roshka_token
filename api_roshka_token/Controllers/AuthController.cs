using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using api_roshka_token.Models; 

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly List<User> _users = new List<User>
    {
        new User { Username = "Bantrax", Password = "123", Id = 1, Role = "Admin", FullName = "Fabian Franco" },
        new User { Username = "Franco", Password = "123", Id = 2, Role = "User", FullName = "Usuario de prueba" }
    };

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);

        if (user == null)
        {
            return Unauthorized("Credenciales inválidas.");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("FullName", user.FullName) 
        };

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }

    [HttpGet("profile")]
    [Authorize]
    public IActionResult GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var fullName = User.FindFirst("FullName")?.Value; 

        if (userId == null)
        {
            return Forbid();
        }

        var profileData = new
        {
            Id = userId,
            Username = username,
            FullName = fullName, 
            Role = role,
            Message = "¡Acceso exitoso con token JWT!"
        };

        return Ok(profileData);
    }

    [HttpGet("admin-data")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAdminData()
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        var fullName = User.FindFirst("FullName")?.Value;
        return Ok(new { Message = $"¡Bienvenido, {fullName}! Tienes acceso a información de administrador." });
    }
}