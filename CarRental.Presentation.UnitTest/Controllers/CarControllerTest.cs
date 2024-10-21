using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using CarRental.Application.DTOs;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Presentation.Controllers;
using CarRental.SharedLibrary.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CrRental.Presentation.UnitTest.Controllers
{
    public class CarControllerTest
    {
        private readonly Mock<ICar> _mockCarService;
        private readonly CarController _controller;
        private List<CarDTO> cars;
        
        public CarControllerTest()
        {
            cars = new List<CarDTO>
            {
                new CarDTO(
                    Id: 1,
                    Brand: "Toyota",
                    Model: "Corolla",
                    Year: 2022,
                    LicensePlate: "ABC123",
                    Category: CarCategory.Sedan,
                    DateCreated: DateTime.Now,
                    DateUpdated: DateTime.Now
                ),
                new CarDTO(
                    Id: 2,
                    Brand: "Honda",
                    Model: "Civic",
                    Year: 2022,
                    LicensePlate: "HOND12",
                    Category: CarCategory.Sedan,
                    DateCreated: DateTime.Now,
                    DateUpdated: DateTime.Now
                )
            };

            _mockCarService = new Mock<ICar>(cars);
            _controller = new CarController(_mockCarService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfCars()
        {
            // Arrange
           
            var httpContext = new DefaultHttpContext(); // or mock a `HttpContext`
            //httpContext.Request.Headers["token"] = "fake_token_here"; //Set header
            //Controller needs a controller context 
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
//assign context to controller
            var controller = new CarController(_mockCarService.Object)
            {
                ControllerContext = controllerContext,
            };
            
            // _mockCarService.Setup(service => service.GetAllAsync()).ReturnsAsync(() => new HttpResponseMessage()
            // {
            //     StatusCode = HttpStatusCode.OK,
            //     Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(cars), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json),
            // });

            // Act
            var result = await controller.GetAllCars();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCars = Assert.IsType<List<CarDTO>>(okResult.Value);
            Assert.Equal(2, returnedCars.Count);
        }
/***
        [Fact]
        public async Task GetById_ReturnsOkResult_WithCar_WhenCarExists()
        {
            // Arrange
            var carId = 1;
            var car = new CarDTO { Id = carId, Brand = "Toyota", Model = "Corolla", LicensePlate = "ABC123" };
            _mockCarService.Setup(service => service.GetByIdAsync(carId)).ReturnsAsync(car);

            // Act
            var result = await _controller.GetCarById(carId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCar = Assert.IsType<CarDTO>(okResult.Value);
            Assert.Equal(carId, returnedCar.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenCarDoesNotExist()
        {
            // Arrange
            var carId = 999;
            _mockCarService.Setup(service => service.GetCarByIdAsync(carId)).ReturnsAsync((CarDTO)null);

            // Act
            var result = await _controller.GetById(carId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenModelStateIsValid()
        {
            // Arrange
            var carToCreate = new CarDTO { Brand = "Ford", Model = "Focus", LicensePlate = "NEW123" };
            var createdCar = new CarDTO { Id = 3, Brand = "Ford", Model = "Focus", LicensePlate = "NEW123" };
            _mockCarService.Setup(service => service.CreateCarAsync(carToCreate)).ReturnsAsync(createdCar);

            // Act
            var result = await _controller.Create(carToCreate);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(CarController.GetById), createdAtActionResult.ActionName);
            Assert.Equal(3, createdAtActionResult.RouteValues["id"]);
            var returnedCar = Assert.IsType<CarDTO>(createdAtActionResult.Value);
            Assert.Equal(3, returnedCar.Id);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenModelStateIsValid()
        {
            // Arrange
            var carId = 1;
            var carToUpdate = new CarDTO { Id = carId, Brand = "Toyota", Model = "Camry", LicensePlate = "UPD123" };
            _mockCarService.Setup(service => service.UpdateCarAsync(carId, carToUpdate)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(carId, carToUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenCarDoesNotExist()
        {
            // Arrange
            var carId = 999;
            var carToUpdate = new CarDTO { Id = carId, Brand = "Toyota", Model = "Camry", LicensePlate = "UPD123" };
            _mockCarService.Setup(service => service.UpdateCarAsync(carId, carToUpdate)).ReturnsAsync(false);

            // Act
            var result = await _controller.Update(carId, carToUpdate);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenCarExists()
        {
            // Arrange
            var carId = 1;
            _mockCarService.Setup(service => service.DeleteCarAsync(carId)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(carId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenCarDoesNotExist()
        {
            // Arrange
            var carId = 999;
            _mockCarService.Setup(service => service.DeleteCarAsync(carId)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(carId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
  ***/      
    }
}