using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BaloncestoAPI.Data;
using BaloncestoAPI.Models;
using System.Security.Cryptography;

namespace BaloncestoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(LoginRequest request)
    {
        // Verificar si el usuario ya existe
        if (await _context.Usuarios.AnyAsync(u => u.Username == request.Username))
        {
            return BadRequest("El usuario ya existe");
        }

        // Crear hash de la contraseña
        var passwordHash = HashPassword(request.Password);

        // Crear nuevo usuario
        var usuario = new Usuario
        {
            Username = request.Username,
            PasswordHash = passwordHash,
            Nombre = request.Username // Por defecto, se puede cambiar después
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        // Generar token JWT
        var token = GenerateJwtToken(usuario);

        return new AuthResponse
        {
            Token = token,
            Username = usuario.Username,
            Nombre = usuario.Nombre,
            Expiracion = DateTime.UtcNow.AddMinutes(60)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        // Buscar usuario
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (usuario == null || !VerifyPassword(request.Password, usuario.PasswordHash))
        {
            return Unauthorized("Usuario o contraseña incorrectos");
        }

        // Generar token JWT
        var token = GenerateJwtToken(usuario);

        return new AuthResponse
        {
            Token = token,
            Username = usuario.Username,
            Nombre = usuario.Nombre,
            Expiracion = DateTime.UtcNow.AddMinutes(60)
        };
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.Username),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim("Nombre", usuario.Nombre)
        };

        var tokenOptions = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationMinutes"])),
            signingCredentials: signinCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == storedHash;
    }
}