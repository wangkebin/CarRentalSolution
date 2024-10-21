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
public class PaymentMethodController(IPaymentMethod paymentMethodInterface, IPaymentMethodService paymentMethodService) : Controller
{
    [HttpPost]
    public async Task<ActionResult<Response>> CreatePaymentMethod(PaymentMethodDTO paymentMethodDto)
    {
        var paymentMethod = paymentMethodDto.ToPaymentMethod();
        var response = await paymentMethodInterface.CreateAsync(paymentMethod);
        if (response is null || !response.Flag)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<Response>> GetAllPaymentMethods()
    {
        var paymentMethods = await paymentMethodInterface.GetAllAsync();
        if (!paymentMethods.Any())
        {
            return NotFound("no payment methods found");
        }

        var paymentMethodsDto = PaymentMethodConversion.FromPaymentMethod(paymentMethods);
        return paymentMethodsDto!.Any() ? Ok(paymentMethodsDto) : NotFound("no payment methods found");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response>> GetPaymentMethodById(int id)
    {
        var paymentMethod = await paymentMethodInterface.GetByIdAsync(id);
        if (paymentMethod is null || paymentMethod.Id <= 0)
        {
            return NotFound($"no payment method found with id: {id}");
        }

        var paymentMethodDto = paymentMethod.FromPaymentMethod();
        return paymentMethodDto is not null ? Ok(paymentMethodDto) : NotFound($"no payment method found with id: {id}");
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<Response>> GetPaymentMethodByCustomerId(int customerId)
    {
        var paymentMethodsDto = await paymentMethodService.GetPaymentMethodsByCustomerIdAsync(customerId);

        return paymentMethodsDto is not null ? Ok(paymentMethodsDto) : NotFound($"no payment method found with customerId: {customerId}");
    }

    [HttpPut]
    public async Task<ActionResult<Response>> UpdatePaymentMethod(PaymentMethodDTO paymentMethodDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var paymentMethod = paymentMethodDto.ToPaymentMethod();
        var response = await paymentMethodInterface.UpdateAsync(paymentMethod);
        if (response is null || !response.Flag)
        {
            return BadRequest(response);
        }   

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Response>> DeletePaymentMethod(int id)
    {
        var response = await paymentMethodInterface.DeleteAsync(id);  
        if (response is null || !response.Flag)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}