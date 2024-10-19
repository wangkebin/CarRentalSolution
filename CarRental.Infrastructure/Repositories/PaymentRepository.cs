using System.Linq.Expressions;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Data;
using CarRental.SharedLibrary.Logs;
using CarRental.SharedLibrary.Response;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

public class PaymentRepository(PaymentDbContext context) : IPayment
{
    public async Task<Response> CreateAsync(Payment entity)
    {
        try
        {
            var currentPayment = context.Payments.Add(entity).Entity;
            await context.SaveChangesAsync();

            if (currentPayment.Id == Guid.Empty)
            {
                return new Response(false, $"Payment for id: {entity.Id} failed to create");
            }

            return new Response(true, $"Payment with id: {currentPayment.Id} has been created");

        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Failed to create payment");
        }
    }

    public async Task<Response> UpdateAsync(Payment entity)
    {
        try
        {
            var existingPayment = (await GetByAsync(p => p.Id == entity.Id)).FirstOrDefault();
            if (existingPayment!.Id == Guid.Empty)
            {
                return new Response(false, $"Payment with id: {entity.Id} not found");
            }

            _ = context.Payments.Update(entity).Entity;
            var updatedRows = await context.SaveChangesAsync();

            if (updatedRows <= 0)
            {
                return new Response(false, $"Failed to update payment with id: {entity.Id}");
            }

            return new Response(true, $"Payment with id: {entity.Id} has been updated");

        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Failed to update payment");
        }
    }

    public async Task<Response> DeleteAsync(Guid id)
    {
        try
        {
            var existingPayment = (await GetByAsync(p => p.Id == id)).FirstOrDefault();
            if (existingPayment!.Id == Guid.Empty)
            {
                return new Response(false, $"Payment with id: {id} not found");
            }
            _ = context.Payments.Remove(existingPayment).Entity;
            var deletedRows = await context.SaveChangesAsync();
            if (deletedRows <= 0)
            {
                return new Response(false, $"Failed to delete payment with id: {id}");
            }

            return new Response(true, $"Payment with id: {id} has been deleted");

        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Failed to delete payment");
        }
    }

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
        try
        {
            return await context.Payments.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Failed to fetch payments");
        }
    }

    public async Task<Payment> GetByIdAsync(Guid id)
    {
        try
        {
            var payment = await context.Payments.FindAsync(id);
            if (payment is null) return null!;
            context.Entry(payment).State = EntityState.Detached;
            return payment;
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Failed to fetch payment by id");
        }
    }

    public async Task<IEnumerable<Payment>> GetByAsync(Expression<Func<Payment, bool>> predicate)
    {
        try
        {
            return await context.Payments.Where(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Failed to fetch payment by predicate");
        }
    }
}