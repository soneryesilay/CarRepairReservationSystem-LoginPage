using CarRepairReservation.Domain.Common;

namespace CarRepairReservation.Domain.Entities;

public class Reservation : BaseEntity
{
    public DateTime ReservationDate { get; set; }
    public string Description { get; set; }
    public ReservationStatus Status { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
}

public enum ReservationStatus
{
    Pending,
    Confirmed,
    Completed,
    Cancelled
} 