using System.ComponentModel.DataAnnotations;

namespace CarRepairReservation.Application.DTOs.Auth;

public class ForgotPasswordDto
{
    [Required(ErrorMessage = "Email adresi zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
    public string Email { get; set; }
} 