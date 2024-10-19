using CarRental.Domain.Entities;

namespace CarRental.Application.DTOs.Conversions;

public static class PaymentConversion
{
    public static Payment ToPayment(this PaymentDTO paymentDto) => new Payment
    {
        Id = paymentDto.Id,
        CustomerId = paymentDto.CustomerId,
        PaymentMethodId = paymentDto.PaymentMethodId,
        AmountCents = paymentDto.AmountCents,
        Currency = paymentDto.Currency
    };

    public static PaymentDTO? FromPayment(this Payment? payment)
    {
        if (payment == null) return null;
        return new PaymentDTO(
            payment!.Id,
            payment!.CustomerId,
            payment!.PaymentMethodId,
            payment!.AmountCents,
            payment!.Currency
        );
    }

    public static IEnumerable<PaymentDTO>? FromPayment(this IEnumerable<Payment>? payments)
    {
        if (payments == null) return null;
        return payments!.Select(p => p.FromPayment()!).ToList();
    }
}