using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Application.Configurations;
using PaymentGateway.Web.Api.Configurations;

namespace PaymentGateway.Web.Api.Registrations
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConfigurations(this IServiceCollection services)
        {
            services.AddSingleton<IAcquiringBankClientConfiguration, AcquiringBankClientConfiguration>();
        }
    }
}
