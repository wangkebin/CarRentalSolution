using Microsoft.EntityFrameworkCore;
using CarRental.Domain.Entities;

namespace CarRental.Infrastructure.Data;

public class CarDbContext(DbContextOptions<CarDbContext> options) : DbContext(options)
{
    public DbSet<Car> Cars { get; set; }   
}