using CarRental.Domain.Entities;

namespace CarRental.Application.DTOs.Conversions;

public static class CustomerConversion
{
    public static Customer ToCustomer(this CustomerDTO customerDto) => new Customer
    {
        Id = customerDto.Id,
        FirstName = customerDto.FirstName,
        LastName = customerDto.LastName,
        Email = customerDto.Email,
        Address = customerDto.Address,
        PhoneNumber = customerDto.PhoneNumber,
        DateCreated = customerDto.DateCreated,
        DateUpdated = customerDto.DateUpdated
    };

    public static CustomerDTO? FromCustomer(this Customer? customer)
    {
        if (customer == null) return null;
        return new CustomerDTO(
            Id: customer.Id,
            FirstName: customer.FirstName,
            LastName: customer.LastName,
            Email: customer.Email,
            Address: customer.Address,
            PhoneNumber:customer.PhoneNumber,
            DateCreated:customer.DateCreated,
            DateUpdated:customer.DateUpdated
        );
    }

    public static IEnumerable<CustomerDTO>? FromCustomer(this IEnumerable<Customer>? customers)
    {
        if (customers == null) return null;
        return customers!.Select(c=>c.FromCustomer()!).ToList();
    }
}