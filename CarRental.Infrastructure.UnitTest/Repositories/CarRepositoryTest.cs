using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Data;
using CarRental.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CarRental.Infrastructure.UnitTest.Repositories
{
    public class CarRepositoryTest
    {
        private readonly Mock<CarDbContext> _mockContext;
        private readonly Mock<DbSet<Car>> _mockSet;
        private readonly CarRepository _repository;

        public CarRepositoryTest()
        {
            _mockContext = new Mock<CarDbContext>();
            _mockSet = new Mock<DbSet<Car>>();
            _mockContext.Setup(c => c.Cars).Returns(_mockSet.Object);
            _repository = new CarRepository(_mockContext.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnSuccessResponse_WhenCarIsCreated()
        {
            // Arrange
            var car = new Car { Id = 1, Brand = "Toyota", Model = "Corolla", LicensePlate = "ABC123" };
            _mockSet.Setup(_ => _.AddAsync(It.IsAny<Car>(), default)).ReturnsAsync(car);
            _mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _repository.CreateAsync(car);

            // Assert
            Assert.True(result.Flag);
            Assert.Contains("has been created", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnSuccessResponse_WhenCarIsUpdated()
        {
            // Arrange
            var car = new Car { Id = 1, Brand = "Toyota", Model = "Corolla", LicensePlate = "ABC123" };
            _mockSet.Setup(m => m.FindAsync(car.Id)).ReturnsAsync(car);
            _mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _repository.UpdateAsync(car);

            // Assert
            Assert.True(result.Flag);
            Assert.Contains("has been updated", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnSuccessResponse_WhenCarIsDeleted()
        {
            // Arrange
            var car = new Car { Id = 1, Brand = "Toyota", Model = "Corolla", LicensePlate = "ABC123" };
            _mockSet.Setup(m => m.FindAsync(car.Id)).ReturnsAsync(car);
            _mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _repository.DeleteAsync(car.Id);

            // Assert
            Assert.True(result.Flag);
            Assert.Contains("has been deleted", result.Message);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCars()
        {
            // Arrange
            var cars = new List<Car>
            {
                new Car { Id = 1, Brand = "Toyota", Model = "Corolla", LicensePlate = "ABC123" },
                new Car { Id = 2, Brand = "Honda", Model = "Civic", LicensePlate = "XYZ789" }
            }.AsQueryable();

            _mockSet.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(cars.Provider);
            _mockSet.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(cars.Expression);
            _mockSet.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(cars.ElementType);
            _mockSet.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(cars.GetEnumerator());

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCar_WhenCarExists()
        {
            // Arrange
            var car = new Car { Id = 1, Brand = "Toyota", Model = "Corolla", LicensePlate = "ABC123" };
            _mockSet.Setup(m => m.FindAsync(car.Id)).ReturnsAsync(car);

            // Act
            var result = await _repository.GetByIdAsync(car.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(car.Id, result.Id);
        }

        [Fact]
        public async Task GetByAsync_ShouldReturnCar_WhenCarMatchesPredicate()
        {
            // Arrange
            var cars = new List<Car>
            {
                new Car { Id = 1, Brand = "Toyota", Model = "Corolla", LicensePlate = "ABC123" },
                new Car { Id = 2, Brand = "Honda", Model = "Civic", LicensePlate = "XYZ789" }
            }.AsQueryable();

            _mockSet.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(cars.Provider);
            _mockSet.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(cars.Expression);
            _mockSet.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(cars.ElementType);
            _mockSet.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(cars.GetEnumerator());

            // Act
            var result = await _repository.GetByAsync(c => c.Brand == "Toyota");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Toyota", result.FirstOrDefault()!.Brand);
        }
    }
}