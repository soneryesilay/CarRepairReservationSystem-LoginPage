using System.ComponentModel.DataAnnotations;

namespace CarRepairReservation.Application.DTOs.Auth;

public class UpdateProfileDto
{
    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Soyad alanı zorunludur.")]
    public string LastName { get; set; }
} 