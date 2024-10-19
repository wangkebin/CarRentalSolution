using System.Linq.Expressions;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Data;
using CarRental.SharedLibrary.Logs;
using CarRental.SharedLibrary.Response;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

public class PaymentMethodRepository(PaymentMethodDbContext context) : IPaymentMethod
{
    public async Task<Response> CreateAsync(PaymentMethod entity)
    {
        try
        {
            var getPaymentMethod =( await GetByAsync(p => p.CardNumber == entity.CardNumber)).FirstOrDefault();
            if (getPaymentMethod is not null && !string.IsNullOrEmpty(getPaymentMethod.CardNumber))
            {
                return new Response(false, "Payment method name already exists");
            }
            var currentPaymentMethod = context.PaymentMethods.Add(entity).Entity;
            await context.SaveChangesAsync();

            if (currentPaymentMethod.Id > 0)
            {
                return new Response(true, $"Payment method with id: {currentPaymentMethod.Id} has been created");
            }
            else
            {
                return new Response(false, $"Payment method with card number: {entity.CardNumber} failed to create");
            }
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Failed to create payment method");
        }
    }

    public async Task<Response> UpdateAsync(PaymentMethod entity)
    {
        try
        {
            var existingPaymentMethod = (await GetByAsync(p => p.Id == entity.Id)).FirstOrDefault();
            if (existingPaymentMethod == null || existingPaymentMethod.Id <= 0)
            {
                return new Response(false, $"Payment method with id: {entity.Id} not found");
            }

            _ = context.PaymentMethods.Update(entity).Entity;
            var updatedRows = await context.SaveChangesAsync();

            if (updatedRows > 0)
            {
                return new Response(true, $"Payment method with id: {entity.Id} has been updated");
            }
            else
            {
                return new Response(false, $"Failed to update payment method with id: {entity.Id}");
            }
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Failed to update payment method");
        }
    }

    public async Task<Response> DeleteAsync(int id)
    {
        try
        {
            var existingPaymentMethod = (await GetByAsync(p => p.Id == id)).FirstOrDefault();
            if (existingPaymentMethod == null || existingPaymentMethod.Id <= 0)
            {
                return new Response(false, $"Payment method with id: {id} not found");
            }
            _ = context.PaymentMethods.Remove(existingPaymentMethod).Entity;
            var deletedRows = await context.SaveChangesAsync();
            if (deletedRows > 0)
            {
                return new Response(true, $"Payment method with id: {id} has been deleted");
            }
            else
            {
                return new Response(false, $"Failed to delete payment method with id: {id}");
            }
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Failed to delete payment method");
        }
    }

    public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
        try
        {
            return await context.PaymentMethods.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Failed to fetch payment methods");
        }
    }

    public async Task<PaymentMethod> GetByIdAsync(int id)
    {
        try
        {
            var paymentMethod = await context.PaymentMethods.FindAsync(id);
            context.Entry(paymentMethod!).State = EntityState.Detached;
            return paymentMethod is not null ? paymentMethod : null!;
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Failed to fetch payment method by id");
        }
    }

    public async Task<IEnumerable<PaymentMethod>> GetByAsync(Expression<Func<PaymentMethod, bool>> predicate)
    {
        try
        {
            return await context.PaymentMethods.Where(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Failed to fetch payment method by predicate");
        }
    }
}