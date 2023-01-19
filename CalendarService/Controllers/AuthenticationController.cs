using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CalendarService.Entities.Authentication;
using CalendarService.Models;
using CalendarService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace CalendarService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;

    public AuthenticationController(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto loginRequest)
    {
        var user = await ValidateUserCredentials(loginRequest.Username, loginRequest.Password);
        if (user == null) return Unauthorized();

        var (token, expires) = await GenerateJwtToken(user);

        return Ok(new LoginResponseDto
        {
            Token = token,
            Expires = expires
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto registerRequest)
    {
        var errors = await ValidateRegistrationRequest(registerRequest);
        if (errors.Any()) return Conflict(new { errors });

        var newUser = new AppUser
        {
            UserName = registerRequest.Username,
            NormalizedUserName = registerRequest.Username.ToUpper(),
            Email = registerRequest.Email,
            NormalizedEmail = registerRequest.Email.ToUpper()
        };

        var result = await _unitOfWork.UserManager.CreateAsync(newUser, registerRequest.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        await _unitOfWork.UserManager.AddToRoleAsync(newUser, "User");

        await _unitOfWork.CalendarRepository.AddCalendarAsync(new Calendar
        {
            Name = newUser.UserName,
            Owner = newUser
        });

        await _unitOfWork.CommitChangesAsync();

        return Ok();
    }

    private async Task<List<string>> ValidateRegistrationRequest(RegisterRequestDto registerRequest)
    {
        var errors = new List<string>();

        if (await _unitOfWork.UserManager.FindByNameAsync(registerRequest.Username) != null)
            errors.Add("Username is already taken");
        if (await _unitOfWork.UserManager.FindByEmailAsync(registerRequest.Email) != null)
            errors.Add("Email is already taken");

        return errors;
    }

    private async Task<List<Claim>> GetValidClaims(AppUser user)
    {
        var options = new IdentityOptions();
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
            new(options.ClaimsIdentity.UserNameClaimType, user.UserName)
        };
        var userClaims = await _unitOfWork.UserManager.GetClaimsAsync(user);
        var userRoles = await _unitOfWork.UserManager.GetRolesAsync(user);
        claims.AddRange(userClaims);
        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole));
            var role = await _unitOfWork.RoleManager.FindByNameAsync(userRole);
            if (role == null) continue;

            var roleClaims = await _unitOfWork.RoleManager.GetClaimsAsync(role);
            foreach (var roleClaim in roleClaims)
            {
                if (claims.Contains(roleClaim)) continue;
                claims.Add(roleClaim);
            }
        }

        return claims;
    }

    private async Task<(string token, DateTime expires)> GenerateJwtToken(AppUser user)
    {
        var claims = await GetValidClaims(user);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_unitOfWork.Configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _unitOfWork.Configuration["Jwt:Issuer"],
            _unitOfWork.Configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_unitOfWork.Configuration["Jwt:ExpiryInMinutes"])),
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
    }

    private async Task<AppUser?> ValidateUserCredentials(string username, string password)
    {
        var user = await _unitOfWork.UserManager.FindByNameAsync(username);
        if (user == null) return null;

        var result = await _unitOfWork.SignInManager.CheckPasswordSignInAsync(user, password, false);
        return !result.Succeeded ? null : user;
    }
}