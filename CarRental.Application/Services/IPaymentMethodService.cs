using CarRental.Application.DTOs;

namespace CarRental.Application.Services;

public interface IPaymentMethodService
{
    Task<IEnumerable<PaymentMethodDTO>> GetPaymentMethodsByCustomerIdAsync(int customerId);
}