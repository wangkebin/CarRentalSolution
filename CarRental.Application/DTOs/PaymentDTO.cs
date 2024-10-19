using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.DTOs;

public record PaymentDTO(
    Guid Id,
    [Required] int CustomerId,
    [Required] int PaymentMethodId,
    [Required] int AmountCents,
    [Required] string Currency
    );