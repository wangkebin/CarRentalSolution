using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Data;

public class PaymentMethodDbContext(DbContextOptions<PaymentMethodDbContext> options) : DbContext(options)
{
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
}