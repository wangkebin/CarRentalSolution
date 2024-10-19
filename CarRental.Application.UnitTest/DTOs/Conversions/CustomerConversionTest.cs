using System;
using System.Collections.Generic;
using CarRental.Application.DTOs;
using CarRental.Application.DTOs.Conversions;
using CarRental.Domain.Entities;
using Xunit;

namespace CarRental.Application.UnitTest.DTOs.Conversions
{
    public class CustomerConversionTest
    {
        [Fact]
        public void ToCustomer_ShouldConvertCustomerDTOToCustomer()
        {
            // Arrange
            var customerDto = new CustomerDTO(
                Id: 1,
                FirstName: "John",
                LastName: "Doe",
                Email: "john.doe@example.com",
                PhoneNumber: "1234567890",
                Address: "123 Main St",
                DateCreated: DateTime.Now,
                DateUpdated: DateTime.Now
            );

            // Act
            var customer = customerDto.ToCustomer();

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customerDto.Id, customer.Id);
            Assert.Equal(customerDto.FirstName, customer.FirstName);
            Assert.Equal(customerDto.LastName, customer.LastName);
            Assert.Equal(customerDto.Email, customer.Email);
            Assert.Equal(customerDto.PhoneNumber, customer.PhoneNumber);
            Assert.Equal(customerDto.Address, customer.Address);
            Assert.Equal(customerDto.DateCreated, customer.DateCreated);
            Assert.Equal(customerDto.DateUpdated, customer.DateUpdated);
        }

        [Fact]
        public void FromCustomer_ShouldConvertCustomerToCustomerDTO()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                Address = "123 Main St",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            // Act
            var customerDto = customer.FromCustomer();

            // Assert
            Assert.NotNull(customerDto);
            Assert.Equal(customer.Id, customerDto.Id);
            Assert.Equal(customer.FirstName, customerDto.FirstName);
            Assert.Equal(customer.LastName, customerDto.LastName);
            Assert.Equal(customer.Email, customerDto.Email);
            Assert.Equal(customer.PhoneNumber, customerDto.PhoneNumber);
            Assert.Equal(customer.Address, customerDto.Address);
            Assert.Equal(customer.DateCreated, customerDto.DateCreated);
            Assert.Equal(customer.DateUpdated, customerDto.DateUpdated);
        }

        [Fact]
        public void FromCustomer_ShouldReturnNullWhenCustomerIsNull()
        {
            // Arrange
            Customer customer = null;

            // Act
            var customerDto = customer.FromCustomer();

            // Assert
            Assert.Null(customerDto);
        }

        [Fact]
        public void FromCustomer_ShouldConvertListOfCustomersToListOfCustomerDTOs()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890", Address = "123 Main St" },
                new Customer { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", PhoneNumber = "0987654321", Address = "456 Elm St" }
            };

            // Act
            var customerDtos = customers.FromCustomer();

            // Assert
            Assert.NotNull(customerDtos);
            Assert.Equal(customers.Count, customerDtos.Count());
            for (int i = 0; i < customers.Count; i++)
            {
                Assert.Equal(customers[i].Id, customerDtos.ElementAt(i).Id);
                Assert.Equal(customers[i].FirstName, customerDtos.ElementAt(i).FirstName);
                Assert.Equal(customers[i].LastName, customerDtos.ElementAt(i).LastName);
                Assert.Equal(customers[i].Email, customerDtos.ElementAt(i).Email);
                Assert.Equal(customers[i].PhoneNumber, customerDtos.ElementAt(i).PhoneNumber);
                Assert.Equal(customers[i].Address, customerDtos.ElementAt(i).Address);
            }
        }

        [Fact]
        public void FromCustomer_ShouldReturnNullWhenListOfCustomersIsNull()
        {
            // Arrange
            List<Customer> customers = null;

            // Act
            var customerDtos = customers.FromCustomer();

            // Assert
            Assert.Null(customerDtos);
        }
    }
}