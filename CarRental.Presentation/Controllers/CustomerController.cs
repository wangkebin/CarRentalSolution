using CarRental.Application.DTOs;
using CarRental.Application.DTOs.Conversions;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.SharedLibrary.Response;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController(ICustomer customerInterface) : Controller
{
    [HttpPost]
    public async Task<ActionResult<Response>> CreateCustomer(CustomerDTO customerDto)
    {
        var customer = customerDto.ToCustomer();
        var response = await customerInterface.CreateAsync(customer);
        if (response is null || !response.Flag)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<Response>> GetAllCustomers()
    {
        var customers = await customerInterface.GetAllAsync();
        if (!customers.Any())
        {
            return NotFound("No customers found");
        }

        var customersDto = customers.FromCustomer();
        return customersDto!.Any() ? Ok(customersDto) : NotFound("No customers found");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response>> GetCustomerById(int id)
    {
        var customer = await customerInterface.GetByIdAsync(id);
        if (customer is null)
        {
            return NotFound($"No customer found with id: {id}");
        }

        var customerDto = customer.FromCustomer();
        return customerDto is not null ? Ok(customerDto) : NotFound($"No customer found with id: {id}");
    }   

    [HttpPut]
    public async Task<ActionResult<Response>> UpdateCustomer(CustomerDTO customerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var customer = customerDto.ToCustomer();
        var response = await customerInterface.UpdateAsync(customer);
        if (response is null || !response.Flag)
        {
            return BadRequest(response);
        }   

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Response>> DeleteCustomer(int id)
    {
        var response = await customerInterface.DeleteAsync(id);  
        if (response is null || !response.Flag)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}