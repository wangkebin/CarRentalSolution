using CarRental.Domain.Entities;
using CarRental.SharedLibrary.Interface;

namespace CarRental.Application.Interfaces;

public interface IPaymentMethod : IGenericInterface<PaymentMethod, int>
{
    
}