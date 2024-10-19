using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.DTOs;

public record ReservationDTO(
    Guid Id,
    [Required] int CustomerId,
    [Required] int CarId,
    [Required] DateTime ReservationStartDateTime,
    [Required] DateTime ReservationEndDateTime,
    string Note);