using System.Linq.Expressions;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Data;
using CarRental.SharedLibrary.Logs;
using CarRental.SharedLibrary.Response;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

public class CarRepository(CarDbContext context) : ICar
{
    public async Task<Response> CreateAsync(Car entity)
    {
        try
        {
            var getCar = (await GetByAsync(c=>c.LicensePlate == entity.LicensePlate)).FirstOrDefault();
            if (string.IsNullOrEmpty(getCar!.LicensePlate))
            {
                return new Response(false, "License Plate already exists");
            }
            var currentCar = context.Cars.Add(entity).Entity;
            await context.SaveChangesAsync();

            if (currentCar.Id > 0)
            {
                return new Response(true, $"Car with id: {currentCar.Id} has been created");
            }
            else
            {
                return new Response(false, $"Car with license plate: {entity.LicensePlate} failed to create");
            }
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Failed to create car");
        }
    }

    public async Task<Response> UpdateAsync(Car entity)
    {
        try
        {
            // Check if the car exists
            var existingCar = (await GetByAsync(c => c.Id == entity.Id)).FirstOrDefault();
            if (existingCar!.Id <= 0)
            {
                return new Response(false, $"Car with id: {entity.Id} not found");
            }

            // Update the car properties
            _=context.Cars.Update(entity).Entity;
            // Save changes
            var updatedRows = await context.SaveChangesAsync();

            if (updatedRows > 0)
            {
                return new Response(true, $"Car with id: {entity.Id} has been updated");
            }
            else
            {
                return new Response(false, $"Failed to update car with id: {entity.Id}");
            }
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Failed to update car");
        }
    }

    public async Task<Response> DeleteAsync(int id)
    {
        try
        {
            var existingCar = (await GetByAsync(c => c.Id == id)).FirstOrDefault();
            if (existingCar!.Id <= 0)
            {
                return new Response(false, $"Car with id: {id} not found");
            }
            _= context.Cars.Remove(existingCar).Entity;
            var deletedRows = await context.SaveChangesAsync();
            if (deletedRows > 0)
            {
                return new Response(true, $"Car with id: {id} has been deleted");
            }
            else
            {
                return new Response(false, $"Failed to delete car with id: {id}");
            }
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Failed to delete car");
        }
    }

    public async Task<IEnumerable<Car>> GetAllAsync()
    {
        try
        {
            return await context.Cars.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Failed to fetch cars");
        }
    }

    public async Task<Car> GetByIdAsync(int id){
        try
        {
            var car = await context.Cars.FindAsync(id);
            if (car is null) return null!;
            context.Entry(car).State = EntityState.Detached;
            return car; 
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Failed to fetch car by id");
        }
    }

    public async Task<IEnumerable<Car>> GetByAsync(Expression<Func<Car, bool>> predicate)
    {
        try
        {
            return await context.Cars.Where(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Failed to fetch car by predicate");
        }
    }
}