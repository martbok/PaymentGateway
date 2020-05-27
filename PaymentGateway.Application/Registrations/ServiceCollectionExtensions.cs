using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Application.Services;

namespace PaymentGateway.Application.Registrations
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IPaymentService, PaymentService>();
        }
    }
}
