using Microsoft.EntityFrameworkCore;
using CarRental.Domain.Entities;

namespace CarRental.Infrastructure.Data;

public class CarDbContext : DbContext
{
    public CarDbContext(): base(){}
    public CarDbContext(DbContextOptions<CarDbContext> options) : base(options)
    {
    }
    public virtual DbSet<Car> Cars { get; set; }  
}