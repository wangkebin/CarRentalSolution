using CarRental.Application.DTOs;
using CarRental.Application.DTOs.Conversions;
using CarRental.Application.Interfaces;
using CarRental.Application.Services;
using CarRental.Domain.Entities;
using CarRental.SharedLibrary.Response;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationController(IReservation reservationInterface, IReservationService reservationService) : Controller
{
    [HttpPost]
    public async Task<ActionResult<Response>> CreateReservation(ReservationDTO reservationDto)
    {
        var reservation = reservationDto.ToReservation();
        var response = await reservationInterface.CreateAsync(reservation);
        if (response is null || !response.Flag)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<Response>> GetAllReservations()
    {
        var reservations = await reservationInterface.GetAllAsync();
        if (!reservations.Any())
        {
            return NotFound("no reservations found");
        }

        var reservationsDto = reservations.FromReservation();
        return reservationsDto!.Any() ? Ok(reservationsDto) : NotFound("no reservations found");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response>> GetReservationById(Guid id)
    {
        var reservation = await reservationInterface.GetByIdAsync(id);
        if (reservation is null || reservation.Id == Guid.Empty)
        {
            return NotFound($"no reservation found with id: {id}");
        }

        var reservationDto = reservation.FromReservation();
        return reservationDto is not null ? Ok(reservationDto) : NotFound($"no reservation found with id: {id}");
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<Response>> GetReservationByCustomerId(int customerId)
    {
        var reservationsDto = await reservationService.GetReservationsByCustomerIdAsync(customerId);
        
        return reservationsDto!.Any() ? Ok(reservationsDto) : NotFound("no reservations found");
    }

    [HttpPut]
    public async Task<ActionResult<Response>> UpdateReservation(ReservationDTO reservationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var reservation = reservationDto.ToReservation();
        var response = await reservationInterface.UpdateAsync(reservation);
        if (response is null || !response.Flag)
        {
            return BadRequest(response);
        }   

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Response>> DeleteReservation(Guid id)
    {
        var response = await reservationInterface.DeleteAsync(id);  
        if (response is null || !response.Flag)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}