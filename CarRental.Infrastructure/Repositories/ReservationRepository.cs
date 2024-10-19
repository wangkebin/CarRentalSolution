using System.Linq.Expressions;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Data;
using CarRental.SharedLibrary.Logs;
using CarRental.SharedLibrary.Response;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

public class ReservationRepository(ReservationDbContext context) : IReservation
{
    public async Task<Response> CreateAsync(Reservation entity)
    {
        try
        {
            var currentReservation = context.Reservations.Add(entity).Entity;
            await context.SaveChangesAsync();

            if (currentReservation.Id != Guid.Empty)
            {
                return new Response(true, $"Reservation with id: {currentReservation.Id} has been created");
            }
            else
            {
                return new Response(false, $"Reservation for car: {entity.CarId} failed to create");
            }
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Failed to create reservation");
        }
    }

    public async Task<Response> UpdateAsync(Reservation entity)
    {
        try
        {
            var existingReservation = (await GetByAsync(r => r.Id == entity.Id)).FirstOrDefault();
            if (existingReservation!.Id== Guid.Empty)
            {
                return new Response(false, $"Reservation with id: {entity.Id} not found");
            }

            _ = context.Reservations.Update(entity).Entity;
            var updatedRows = await context.SaveChangesAsync();

            if (updatedRows > 0)
            {
                return new Response(true, $"Reservation with id: {entity.Id} has been updated");
            }
            else
            {
                return new Response(false, $"Failed to update reservation with id: {entity.Id}");
            }
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Failed to update reservation");
        }
    }

    public async Task<Response> DeleteAsync(Guid id)
    {
        try
        {
            var existingReservation = (await GetByAsync(r => r.Id == id)).FirstOrDefault();
            if (existingReservation!.Id == Guid.Empty)
            {
                return new Response(false, $"Reservation with id: {id} not found");
            }
            _ = context.Reservations.Remove(existingReservation).Entity;
            var deletedRows = await context.SaveChangesAsync();
            if (deletedRows > 0)
            {
                return new Response(true, $"Reservation with id: {id} has been deleted");
            }
            else
            {
                return new Response(false, $"Failed to delete reservation with id: {id}");
            }
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Failed to delete reservation");
        }
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        try
        {
            return await context.Reservations.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Failed to fetch reservations");
        }
    }

    public async Task<Reservation> GetByIdAsync(Guid id)
    {
        try
        {
            var reservation = await context.Reservations.FindAsync(id);
            if (reservation is null) return null!;
            context.Entry(reservation).State = EntityState.Detached;
            return reservation;
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Failed to fetch reservation by id");
        }
    }

    public async Task<IEnumerable<Reservation>> GetByAsync(Expression<Func<Reservation, bool>> predicate)
    {
        try
        {
            return await context.Reservations.Where(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Failed to fetch reservation by predicate");
        }
    }
}