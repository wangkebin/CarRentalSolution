using CarRental.Application.DTOs;
using CarRental.Application.DTOs.Conversions;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.SharedLibrary.Response;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarController(ICar carInterface) : Controller
{
    [HttpPost]
    public async Task<ActionResult<Response>> CreateCar(CarDTO carDto)
    {
        var car = carDto.ToCar();
        var response = await carInterface.CreateAsync(car);
        if (response is null || !response.Flag)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<Response>> GetAllCars()
    {
        var cars = await carInterface.GetAllAsync();
        if (!cars.Any())
        {
            return NotFound("no cars found ");
        }

        var carsDto = cars.FromCar();
        return carsDto!.Any() ? Ok(carsDto) : NotFound("no cars found");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response>> GetCarById(int id)
    {
        var car = await carInterface.GetByIdAsync(id);
        if (car is null || car.Id <= 0)
        {
            return NotFound($"no car found with id: {id}");
        }

        var carDto = car.FromCar();
        return carDto is not null ? Ok(carDto) : NotFound($"no car found with id: {id}");
    }   

    [HttpPut]
    public async Task<ActionResult<Response>> UpdateCar(CarDTO carDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var car = carDto.ToCar();
        var response = await carInterface.UpdateAsync(car);
        if (response is null || !response.Flag)
        {
            return BadRequest(response);
        }   

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Response>> DeleteCar(int id)
    {
        var response = await carInterface.DeleteAsync(id);  
        if (response is null || !response.Flag)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}