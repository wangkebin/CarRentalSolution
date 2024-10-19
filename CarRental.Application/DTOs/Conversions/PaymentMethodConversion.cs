using CarRental.Domain.Entities;

namespace CarRental.Application.DTOs.Conversions;

public static class PaymentMethodConversion
{
    public static PaymentMethod ToPaymentMethod(this PaymentMethodDTO paymentMethodDto) => new PaymentMethod
    {
        Id = paymentMethodDto.Id,
        CustomerId = paymentMethodDto.CustomerId,
        CardNumber = paymentMethodDto.CardNumber,
        FirstName = paymentMethodDto.FirstName,
        LastName = paymentMethodDto.LastName,
        CardAddress = paymentMethodDto.CardAddress
    };

    public static PaymentMethodDTO? FromPaymentMethod(this PaymentMethod? paymentMethod)
    {
        if (paymentMethod == null) return null;
        return new PaymentMethodDTO(
            paymentMethod!.Id,
            paymentMethod!.FirstName,
            paymentMethod!.LastName,
            paymentMethod!.CardNumber,
            paymentMethod!.CardAddress,
            paymentMethod!.CustomerId);
    }

    public static IEnumerable<PaymentMethodDTO>? FromPaymentMethods(this IEnumerable<PaymentMethod>? paymentMethods)
    {
        if (paymentMethods == null) return null;
        return paymentMethods.Select(p => p.FromPaymentMethod()!).ToList();
    }
}