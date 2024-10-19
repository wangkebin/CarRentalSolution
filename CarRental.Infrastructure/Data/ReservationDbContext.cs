using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Data;

public class ReservationDbContext(DbContextOptions<ReservationDbContext> options) : DbContext(options)
{
    public DbSet<Reservation> Reservations { get; set; }
}