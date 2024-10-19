using CarRental.Application.DTOs;
using CarRental.Application.DTOs.Conversions;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using Polly.Registry;

namespace CarRental.Application.Services;

public class PaymentMethodService(IPaymentMethod paymentMethodInterface, HttpClient httpClient,
    ResiliencePipelineProvider<string> resiliencePipeline) : IPaymentMethodService
{
    public  async Task<IEnumerable<PaymentMethodDTO>> GetPaymentMethodsByCustomerIdAsync(int customerId)
    {
        var paymentMethods = await paymentMethodInterface.GetByAsync(p=>p.CustomerId == customerId);
        if (!paymentMethods.Any()) return null!;
        return PaymentMethodConversion.FromPaymentMethods(paymentMethods)!;
    }
}