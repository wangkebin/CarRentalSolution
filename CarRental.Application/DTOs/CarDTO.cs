using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CarRental.Domain.Entities;

namespace CarRental.Application.DTOs;

public record CarDTO(
    int Id,
    [Required] string Brand,
    [Required] string Model,
    
    [Required] int Year,
    [Required] string LicensePlate,
    [Required] CarCategory Category,
    DateTime DateCreated,
    DateTime DateUpdated);