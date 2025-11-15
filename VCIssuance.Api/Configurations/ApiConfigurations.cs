namespace VCIssuance.Api.Configurations;

public static class ApiConfigurations
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();

        return services;
    }
}
