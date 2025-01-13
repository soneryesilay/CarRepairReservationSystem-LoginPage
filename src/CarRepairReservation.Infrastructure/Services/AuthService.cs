using System.Net;
using CarRepairReservation.Application.DTOs.Auth;
using CarRepairReservation.Application.Interfaces;
using CarRepairReservation.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace CarRepairReservation.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<TokenDto> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            throw new Exception("Email veya şifre hatalı");

        if (!user.IsActive)
            throw new Exception("Hesabınız aktif değil");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "User";

        return await _tokenService.CreateTokenAsync(user, role);
    }

    public async Task<TokenDto> RegisterAsync(string firstName, string lastName, string email, string password, string confirmPassword)
    {
        if (password != confirmPassword)
            throw new Exception("Şifreler eşleşmiyor");

        if (await IsEmailUniqueAsync(email))
            throw new Exception("Bu email adresi zaten kayıtlı");

        // Email formatını kontrol et
        if (!IsValidEmail(email))
            throw new Exception("Geçerli bir email adresi giriniz");

        var user = new ApplicationUser
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = email,
            IsActive = true,
            EmailConfirmed = true // Otomatik olarak doğrulanmış kabul et
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, "User");

        return await _tokenService.CreateTokenAsync(user, "User");
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user != null;
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email && (
                email.EndsWith("@gmail.com") || 
                email.EndsWith("@hotmail.com") || 
                email.EndsWith("@outlook.com") || 
                email.EndsWith("@yahoo.com")
            );
        }
        catch
        {
            return false;
        }
    }

    // Artık kullanılmayan metodlar
    public Task<bool> VerifyEmailAsync(string userId, string token) => Task.FromResult(true);
    public Task ResendVerificationEmailAsync(string email) => Task.CompletedTask;
} 