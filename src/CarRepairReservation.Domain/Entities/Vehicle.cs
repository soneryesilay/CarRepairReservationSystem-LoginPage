using CarRepairReservation.Domain.Common;

namespace CarRepairReservation.Domain.Entities;

public class Vehicle : BaseEntity
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public string LicensePlate { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
} 