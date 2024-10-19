using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.DTOs;

public record CustomerDTO(
    int Id,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] string Address,
    [Required] string PhoneNumber,
    DateTime DateCreated,
    DateTime DateUpdated
    );