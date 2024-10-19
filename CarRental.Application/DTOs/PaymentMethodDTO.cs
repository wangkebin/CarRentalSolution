using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.DTOs;

public record PaymentMethodDTO(
    int Id,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string CardNumber,
    [Required] string CardAddress,
    [Required] int CustomerId);