using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;
    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(user => user.Email == dto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }
        var newUser = new User();
        newUser.Name = dto.Name;
        newUser.Email = dto.Email;
        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, dto.Password);

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        var token = GenerateJwtToken(newUser);

        return new AuthResponseDto
        {
            Token = token,
            UserId = newUser.Id,
            Name = newUser.Name,
            Email = newUser.Email
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(user => user.Email == dto.Email);
        if (existingUser == null)
        {
            throw new InvalidOperationException("A user with this email doesn't exist.");
        }
        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, dto.Password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            throw new InvalidOperationException("Incorrect password");
        }
        var token = GenerateJwtToken(existingUser);

        return new AuthResponseDto
        {
            Token = token,
            UserId = existingUser.Id,
            Name = existingUser.Name,
            Email = existingUser.Email
        };
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
        new Claim(
            ClaimTypes.NameIdentifier,
            user.Id.ToString()
        ),
        new Claim(
            ClaimTypes.Name,
            user.Name
        ),
        new Claim(
            ClaimTypes.Email,
            user.Email
        )
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!
            )
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                double.Parse(
                    _configuration["Jwt:ExpiresInMinutes"]!
                )
            ),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}