namespace CarRental.Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public string Note { get; set; } = string.Empty;
    public int CarId { get; set; }
    public int CustomerId { get; set; }
    public DateTime ReservationStartDateTime { get; set; }
    public DateTime ReservationEndDateTime { get; set; }
}