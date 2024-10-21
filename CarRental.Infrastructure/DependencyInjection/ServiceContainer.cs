using CarRental.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CarRental.Application.Interfaces;
using CarRental.Application.Services;
using CarRental.Infrastructure.Data;
using CarRental.Infrastructure.Repositories;

namespace CarRental.Infrastructure.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        // Add database connectivity
        // Add auth
        SharedServiceContainer.AddSharedServices<CarDbContext>(services, config, config["MySerilog:FineName"]!);
        SharedServiceContainer.AddSharedServices<CustomerDbContext>(services, config, config["MySerilog:FineName"]!);
        SharedServiceContainer.AddSharedServices<PaymentDbContext>(services, config, config["MySerilog:FineName"]!);
        SharedServiceContainer.AddSharedServices<PaymentMethodDbContext>(services, config, config["MySerilog:FineName"]!);
        SharedServiceContainer.AddSharedServices<ReservationDbContext>(services, config, config["MySerilog:FineName"]!);
    
        // crete Dependency Injection
        services.AddScoped<ICar, CarRepository>();
        services.AddScoped<ICustomer, CustomerRepository>();
        services.AddScoped<IPayment, PaymentRepository>();        
        services.AddScoped<IPaymentMethod, PaymentMethodRepository>();
        services.AddScoped<IReservation, ReservationRepository>();

        services.AddScoped<IPaymentMethodService, PaymentMethodService>();
        services.AddScoped<IReservationService, ReservationService>();

        return services;
    }

    public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
    {
        // Register middlewares
        // Global Exception
        // Listen to Only API Gateway: blocks all outside calls
        SharedServiceContainer.UseSharedPolicies(app);

        return app;
    }
}