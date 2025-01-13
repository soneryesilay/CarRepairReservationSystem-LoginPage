namespace CarRepairReservation.Application.DTOs.Auth;

public class TokenDto
{
    public string AccessToken { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string UserName { get; set; }
    public string UserRole { get; set; }
} 