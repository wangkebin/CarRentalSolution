namespace CarRental.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public int CustomerId { get; set; }
    public int PaymentMethodId { get; set; }
    public int AmountCents { get; set; }
    public string Currency { get; set; } = "USD";

}