using CarRepairReservation.Application.DTOs.Auth;
using CarRepairReservation.Domain.Entities;

namespace CarRepairReservation.Application.Interfaces;

public interface ITokenService
{
    Task<TokenDto> CreateTokenAsync(ApplicationUser user, string role);
} 