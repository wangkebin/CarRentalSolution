using CarRental.Domain.Entities;

namespace CarRental.Application.DTOs.Conversions;

public static class CarConversion
{
    public static Car ToCar(this CarDTO carDto) => new Car
    {
        Id = carDto.Id,
        Brand = carDto.Brand,
        Model = carDto.Model,
        Year = carDto.Year,
        LicensePlate = carDto.LicensePlate,
        Category = carDto.Category,
        DateCreated = carDto.DateCreated,
        DateUpdated = carDto.DateUpdated
    };

    public static CarDTO? FromCar(this Car? car)
    {
        if (car == null) return null;
        return new CarDTO(
            car!.Id,
            car!.Brand,
            car!.Model,
            car!.Year,
            car!.LicensePlate,
            car!.Category,
            car!.DateCreated,
            car!.DateUpdated);
    }

    public static IEnumerable<CarDTO>? FromCar(this IEnumerable<Car>? cars)
    {
        if (cars == null) return null;
        return cars!.Select(c => c.FromCar()!).ToList();
    }
}