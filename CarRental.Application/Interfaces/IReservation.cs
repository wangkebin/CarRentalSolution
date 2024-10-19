using CarRental.Domain.Entities;
using CarRental.SharedLibrary.Interface;

namespace CarRental.Application.Interfaces;

public interface IReservation : IGenericInterface<Reservation, Guid>
{
    
}