using CarRepairReservation.Application.DTOs.Auth;

namespace CarRepairReservation.Application.Interfaces;

public interface IAuthService
{
    Task<TokenDto> LoginAsync(string email, string password);
    Task<TokenDto> RegisterAsync(string firstName, string lastName, string email, string password, string confirmPassword);
    Task<bool> IsEmailUniqueAsync(string email);
    Task<bool> VerifyEmailAsync(string userId, string token);
    Task ResendVerificationEmailAsync(string email);
} 