using CarRepairReservation.Application.DTOs.Auth;
using CarRepairReservation.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRepairReservation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto model)
    {
        try
        {
            var result = await _authService.LoginAsync(model.Email, model.Password);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<TokenDto>> Register([FromBody] RegisterDto model)
    {
        try
        {
            var result = await _authService.RegisterAsync(
                model.FirstName,
                model.LastName,
                model.Email,
                model.Password,
                model.ConfirmPassword);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("verify-email")]
    public async Task<ActionResult> VerifyEmail([FromQuery] string userId, [FromQuery] string token)
    {
        try
        {
            var result = await _authService.VerifyEmailAsync(userId, token);
            if (result)
                return Ok();

            return BadRequest("Email doğrulama başarısız oldu.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("resend-verification")]
    public async Task<ActionResult> ResendVerification([FromBody] ResendVerificationDto model)
    {
        try
        {
            await _authService.ResendVerificationEmailAsync(model.Email);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("check-email")]
    public async Task<ActionResult<bool>> CheckEmail([FromQuery] string email)
    {
        try
        {
            var result = await _authService.IsEmailUniqueAsync(email);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
} 