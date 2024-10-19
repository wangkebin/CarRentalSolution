using CarRental.Application.DTOs;
using CarRental.Application.DTOs.Conversions;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.SharedLibrary.Response;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController(IPayment paymentInterface) : Controller
{
    [HttpPost]
    public async Task<ActionResult<Response>> CreatePayment(PaymentDTO paymentDto)
    {
        var payment = paymentDto.ToPayment();
        var response = await paymentInterface.CreateAsync(payment);
        if (response is null || !response.Flag)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<Response>> GetAllPayments()
    {
        var payments = await paymentInterface.GetAllAsync();
        if (payments.Any())
        {
            return NotFound("no payments found");
        }

        var paymentsDto = payments.FromPayment();
        return paymentsDto!.Any() ? Ok(paymentsDto) : NotFound("no payments found");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response>> GetPaymentById(Guid id)
    {
        var payment = await paymentInterface.GetByIdAsync(id);
        if (payment is null)
        {
            return NotFound($"no payment found with id: {id}");
        }

        var paymentDto = payment.FromPayment();
        return paymentDto is not null ? Ok(paymentDto) : NotFound($"no payment found with id: {id}");
    }   

    [HttpPut]
    public async Task<ActionResult<Response>> UpdatePayment(PaymentDTO paymentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var payment = paymentDto.ToPayment();
        var response = await paymentInterface.UpdateAsync(payment);
        if (response is null || !response.Flag)
        {
            return BadRequest(response);
        }   

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Response>> DeletePayment(Guid id)
    {
        var response = await paymentInterface.DeleteAsync(id);  
        if (response is null || !response.Flag)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}