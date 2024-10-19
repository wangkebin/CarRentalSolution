namespace CarRental.Domain.Entities;

public class PaymentMethod
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string CardAddress { get; set; } = string.Empty;
    public int CustomerId { get; set; }
}