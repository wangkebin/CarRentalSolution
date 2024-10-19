using CarRental.Domain.Entities;
using CarRental.SharedLibrary.Interface;

namespace CarRental.Application.Interfaces;

public interface ICar : IGenericInterface<Car, int>
{
    
}