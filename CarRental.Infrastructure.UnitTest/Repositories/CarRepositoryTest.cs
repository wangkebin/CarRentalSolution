using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Data;
using CarRental.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MockQueryable.Moq;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace CarRental.Infrastructure.UnitTest.Repositories
{
    public class CarRepositoryTest
    {
        private readonly Mock<DbSet<Car>> _mockSet;
        private readonly Mock<CarDbContext> _mockContext;
        private readonly CarRepository _repository;
        private readonly IQueryable _fakeCars;
        private readonly List<Car> cars;

        public CarRepositoryTest()
        {
            
            _fakeCars = GetFakeCars().AsQueryable();
            _mockSet = cars.BuildMockDbSet();
            _mockContext = new Mock<CarDbContext>();
            _mockContext.Setup(c => c.Cars).Returns(_mockSet.Object);

            _mockSet.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(_fakeCars.Provider);
            _mockSet.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(_fakeCars.Expression);
            _mockSet.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(_fakeCars.ElementType);
            //_mockSet.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(_fakeCars.GetEnumerator());
            _mockSet.Setup(x => x.FindAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => cars.FirstOrDefault(d => d.Id == id));
            
            _repository = new CarRepository(_mockContext.Object);
        }

        private static List<Car> GetFakeCars()
        {
            return new List<Car>()
            {
                new Car { Id = 1, Brand = "Toyota", Model = "Corolla", LicensePlate = "ABC123" },
                new Car { Id = 2, Brand = "Honda", Model = "Civic", LicensePlate = "HON123" }
            };
        }
        
        [Fact]
        public async Task CreateAsync_ShouldReturnSuccessResponse_WhenCarIsCreated()
        {
            // Arrange
            var car = new Car { Id = 3, Brand = "Toyota", Model = "Corolla", LicensePlate = "CREATE" };

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
            // var cars = new List<Car>
            // {
            //     new Car { Id = 1, Brand = "Toyota", Model = "Corolla", LicensePlate = "ABC123" },
            //     new Car { Id = 2, Brand = "Honda", Model = "Civic", LicensePlate = "XYZ789" }
            // }.AsQueryable();
            //
            // _mockSet.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(cars.Provider);
            // _mockSet.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(cars.Expression);
            // _mockSet.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(cars.ElementType);
            // _mockSet.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(cars.GetEnumerator());

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCar_WhenCarExists()
        {
            // Arrange
            //var car = new Car { Id = 1, Brand = "Toyota", Model = "Corolla", LicensePlate = "ABC123" };
            
           var car = cars.First();

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

    public class AddHelper : IAddHelper
    {

        public void AddEntity<TEntity>(DbContext db, TEntity entities) where TEntity : class
        {
            db.Set<TEntity>().Add(entities);
        }
    }

    public interface IAddHelper
    {
        void AddEntity<TEntity>(DbContext db, TEntity entities) where TEntity : class;
    }
    public static class DbContextMock
    {
        public static TContext GetMock<TData, TContext>(List<TData> lstData,
            Expression<Func<TContext, DbSet<TData>>> dbSetSelectionExpression)
            where TData : Car where TContext : DbContext
        {
            IQueryable<TData> lstDataQueryable = lstData.AsQueryable();
            Mock<DbSet<TData>> dbSetMock = new Mock<DbSet<TData>>();
            Mock<TContext> dbContext = new Mock<TContext>();

            dbSetMock.As<IQueryable<TData>>().Setup(s => s.Provider).Returns(lstDataQueryable.Provider);
            dbSetMock.As<IQueryable<TData>>().Setup(s => s.Expression).Returns(lstDataQueryable.Expression);
            dbSetMock.As<IQueryable<TData>>().Setup(s => s.ElementType).Returns(lstDataQueryable.ElementType);
            dbSetMock.As<IQueryable<TData>>().Setup(s => s.GetEnumerator())
                .Returns(() => lstDataQueryable.GetEnumerator());
            dbSetMock.Setup(x => x.Add(It.IsAny<TData>())).Callback<TData>(lstData.Add);
            dbSetMock.Setup(x => x.AddRange(It.IsAny<IEnumerable<TData>>()))
                .Callback<IEnumerable<TData>>(lstData.AddRange);
            dbSetMock.Setup(x => x.FindAsync(It.IsAny<int>())).
                ReturnsAsync((int id) => lstData.FirstOrDefault(d => d.Id == id));
                
            dbSetMock.Setup(x => x.Remove(It.IsAny<TData>())).Callback<TData>(t => lstData.Remove(t));
            dbSetMock.Setup(x => x.RemoveRange(It.IsAny<IEnumerable<TData>>())).Callback<IEnumerable<TData>>(ts =>
            {
                foreach (var t in ts)
                {
                    lstData.Remove(t);
                }
            });


            dbContext.Setup(dbSetSelectionExpression).Returns(dbSetMock.Object);

            return dbContext.Object;
        }
    }
}