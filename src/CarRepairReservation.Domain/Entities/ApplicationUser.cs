using Microsoft.AspNetCore.Identity;

namespace CarRepairReservation.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
} 