using Microsoft.Extensions.Azure;
using VCIssuance.Api.Services;
using VCIssuance.Core.Interfaces;

namespace VCIssuance.Api.Configurations;

public static class ApiConfigurations
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();

        services.AddAzureClients(clientBuilder => 
        {
            clientBuilder.AddServiceBusClient(configuration.GetConnectionString("AzureServiceBus"));
        });

        services.AddSingleton<IMessageService, AzureServiceBusService>();

        return services;
    }
}
