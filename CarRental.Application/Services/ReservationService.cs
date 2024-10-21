using CarRental.Application.DTOs;
using CarRental.Application.DTOs.Conversions;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using Polly.Registry;

namespace CarRental.Application.Services;

public class ReservationService(
    IReservation reservationInterface) : IReservationService
{
    public async Task<IEnumerable<ReservationDTO>> GetReservationsByCustomerIdAsync(int customerId)
    {
        var reservations = await reservationInterface.GetByAsync(p => p.CustomerId == customerId);
        if (!reservations.Any()) return null!;
        return ReservationConversion.FromReservation(reservations)!; 
    }
}
