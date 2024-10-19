using System.Collections;
using CarRental.Application.DTOs;

namespace CarRental.Application.Services;

public interface IReservationService
{
    Task<IEnumerable<ReservationDTO>> GetReservationsByCustomerIdAsync(int customerId);
}