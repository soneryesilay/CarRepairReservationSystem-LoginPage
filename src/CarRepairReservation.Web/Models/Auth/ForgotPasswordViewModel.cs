using System.ComponentModel.DataAnnotations;

namespace CarRepairReservation.Web.Models.Auth;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Email adresi zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
    [Display(Name = "Email")]
    public string Email { get; set; }
} 