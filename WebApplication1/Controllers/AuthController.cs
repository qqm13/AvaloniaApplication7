using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.DB;
using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApplication1.Auth;
using WebApplication1.DTO;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]

public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly ZzzadaniePrContext _context;
    
    public AuthController(ILogger<AuthController> logger, ZzzadaniePrContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpPost(Name = "api/auth/login")]
    public async Task<ActionResult> Login(string username, string password)
    {
        var user = await _context.Credentials.FirstOrDefaultAsync(s=>s.Username ==  username);
        if (user == null)
            return new NotFoundResult();
        
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        string token = "";
        if (user.PasswordHash == hashedPassword)
        {
        
            var claims = new List<Claim> {
                //Кладём Id (если нужно)
                new Claim(ClaimValueTypes.Integer32, user.Id.ToString())
            };
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                //кладём полезную нагрузку
                claims: claims,
                //устанавливаем время жизни токена 2 минуты
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(60)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                    
            token = new JwtSecurityTokenHandler().WriteToken(jwt);    
        }
        
        return new OkObjectResult(token);
    }

    [HttpGet(Name = "api/auth/profile")]
    public EmployeeDTO Profile(string token)
    {
        var jwtService = new AuthOptions();
        var tokenh = jwtService.ValidateToken(token);
        
        if (tokenh == null)
            return null;
        
        EmployeeDTO employee = new EmployeeDTO     {
            Id = Convert.ToInt32(tokenh.FindFirst(ClaimTypes.Sid)?.Value),
            FirstName = Convert.ToString(tokenh.FindFirst(ClaimTypes.GivenName)?.Value),
            LastName = Convert.ToString(tokenh.FindFirst(ClaimTypes.Surname)?.Value),
            Position = Convert.ToString(tokenh.FindFirst(ClaimTypes.Role)?.Value),
            HireDate = Convert.ToDateTime(tokenh.FindFirst(ClaimTypes.DateOfBirth)?.Value),
            IsActive = Convert.ToBoolean(tokenh.FindFirst(ClaimTypes.Role)?.Value)
        };
        
        return employee;
    }
}