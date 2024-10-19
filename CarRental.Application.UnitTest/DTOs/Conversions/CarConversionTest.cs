using System;
using System.Collections.Generic;
using CarRental.Application.DTOs;
using CarRental.Application.DTOs.Conversions;
using CarRental.Domain.Entities;
using Xunit;

namespace CarRental.Tests.Conversions
{
    public class CarConversionTests
    {
        [Fact]
        public void ToCar_ShouldConvertCarDTOToCar()
        {
            // Arrange
            var carDto = new CarDTO(
                Id: 1,
                Brand: "Toyota",
                Model: "Corolla",
                Year: 2022,
                LicensePlate: "ABC123",
                Category: CarCategory.Sedan,
                DateCreated: DateTime.Now,
                DateUpdated: DateTime.Now
            );

            // Act
            var car = carDto.ToCar();

            // Assert
            Assert.NotNull(car);
            Assert.Equal(carDto.Id, car.Id);
            Assert.Equal(carDto.Brand, car.Brand);
            Assert.Equal(carDto.Model, car.Model);
            Assert.Equal(carDto.Year, car.Year);
            Assert.Equal(carDto.LicensePlate, car.LicensePlate);
            Assert.Equal(carDto.Category, car.Category);
            Assert.Equal(carDto.DateCreated, car.DateCreated);
            Assert.Equal(carDto.DateUpdated, car.DateUpdated);
        }

        [Fact]
        public void FromCar_ShouldConvertCarToCarDTO()
        {
            // Arrange
            var car = new Car
            {
                Id = 1,
                Brand = "Toyota",
                Model = "Corolla",
                Year = 2022,
                LicensePlate = "ABC123",
                Category = CarCategory.Sedan,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            // Act
            var carDto = car.FromCar();

            // Assert
            Assert.NotNull(carDto);
            Assert.Equal(car.Id, carDto.Id);
            Assert.Equal(car.Brand, carDto.Brand);
            Assert.Equal(car.Model, carDto.Model);
            Assert.Equal(car.Year, carDto.Year);
            Assert.Equal(car.LicensePlate, carDto.LicensePlate);
            Assert.Equal(car.Category, carDto.Category);
            Assert.Equal(car.DateCreated, carDto.DateCreated);
            Assert.Equal(car.DateUpdated, carDto.DateUpdated);
        }

        [Fact]
        public void FromCar_ShouldReturnNullWhenCarIsNull()
        {
            // Arrange
            Car car = null;

            // Act
            var carDto = car.FromCar();

            // Assert
            Assert.Null(carDto);
        }

        [Fact]
        public void FromCar_ShouldConvertListOfCarsToListOfCarDTOs()
        {
            // Arrange
            var cars = new List<Car>
            {
                new Car { Id = 1, Brand = "Toyota", Model = "Corolla", Year = 2022, LicensePlate = "ABC123", Category =
                    CarCategory.Sedan
                },
                new Car { Id = 2, Brand = "Honda", Model = "Civic", Year = 2021, LicensePlate = "XYZ789", Category =
                    CarCategory.Sedan
                }
            };

            // Act
            var carDtos = cars.FromCar();

            // Assert
            Assert.NotNull(carDtos);
            Assert.Equal(cars.Count, carDtos.Count());
            for (int i = 0; i < cars.Count; i++)
            {
                Assert.Equal(cars[i].Id, carDtos.ElementAt(i).Id);
                Assert.Equal(cars[i].Brand, carDtos.ElementAt(i).Brand);
                Assert.Equal(cars[i].Model, carDtos.ElementAt(i).Model);
                Assert.Equal(cars[i].Year, carDtos.ElementAt(i).Year);
                Assert.Equal(cars[i].LicensePlate, carDtos.ElementAt(i).LicensePlate);
                Assert.Equal(cars[i].Category, carDtos.ElementAt(i).Category);
            }
        }

        [Fact]
        public void FromCar_ShouldReturnNullWhenListOfCarsIsNull()
        {
            // Arrange
            List<Car> cars = null;

            // Act
            var carDtos = cars.FromCar();

            // Assert
            Assert.Null(carDtos);
        }
    }
}