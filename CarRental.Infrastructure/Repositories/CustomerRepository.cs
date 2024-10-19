using System.Linq.Expressions;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Data;
using CarRental.SharedLibrary.Logs;
using CarRental.SharedLibrary.Response;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

public class CustomerRepository(CustomerDbContext context) : ICustomer
{
    public async Task<Response> CreateAsync(Customer entity)
    {
        try
        {
            var getCustomer = (await GetByAsync(c => c.Email == entity.Email)).FirstOrDefault();
            if (!string.IsNullOrEmpty(getCustomer!.Email))
            {
                return new Response(false, "Email already exists");
            }
            var currentCustomer = context.Customers.Add(entity).Entity;
            await context.SaveChangesAsync();

            if (currentCustomer.Id <= 0)
            {
                return new Response(false, $"Customer with email: {entity.Email} failed to create");
            }

            return new Response(true, $"Customer with id: {currentCustomer.Id} has been created");

        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Failed to create customer");
        }
    }

    public async Task<Response> UpdateAsync(Customer entity)
    {
        try
        {
            var existingCustomer = (await GetByAsync(c => c.Id == entity.Id)).FirstOrDefault();
            if (existingCustomer!.Id <= 0)
            {
                return new Response(false, $"Customer with id: {entity.Id} not found");
            }

            _ = context.Customers.Update(entity).Entity;
            var updatedRows = await context.SaveChangesAsync();

            if (updatedRows <= 0)
            {
                return new Response(false, $"Failed to update customer with id: {entity.Id}");
            }
            return new Response(true, $"Customer with id: {entity.Id} has been updated");

        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Failed to update customer");
        }
    }

    public async Task<Response> DeleteAsync(int id)
    {
        try
        {
            var existingCustomer = (await GetByAsync(c => c.Id == id)).FirstOrDefault();
            
            if (existingCustomer!.Id <= 0)
            {
                return new Response(false, $"Customer with id: {id} not found");
            }
            _ = context.Customers.Remove(existingCustomer).Entity;
            var deletedRows = await context.SaveChangesAsync();
            if (deletedRows <= 0)
            {
                return new Response(false, $"Failed to delete customer with id: {id}");
            }

            return new Response(true, $"Customer with id: {id} has been deleted");
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Failed to delete customer");
        }
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        try
        {
            return await context.Customers.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Failed to fetch customers");
        }
    }

    public async Task<Customer> GetByIdAsync(int id)
    {
        try
        {
            var customer = await context.Customers.FindAsync(id);
            if (customer is null) return null!;
            context.Entry(customer).State = EntityState.Detached;
            return customer;
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Failed to fetch customer by id");
        }
    }

    public async Task<IEnumerable<Customer>> GetByAsync(Expression<Func<Customer, bool>> predicate)
    {
        try
        {
            return await context.Customers.Where(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Failed to fetch customer by predicate");
        }
    }
}