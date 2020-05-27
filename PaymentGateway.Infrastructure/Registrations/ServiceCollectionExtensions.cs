using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Application.Providers;
using PaymentGateway.Application.Repositories;
using PaymentGateway.Infrastructure.Providers;
using PaymentGateway.Infrastructure.Repositories;

namespace PaymentGateway.Infrastructure.Registrations
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureAdapters(this IServiceCollection services)
        {
            services.AddSingleton<IPaymentRepository, InMemoryPaymentRepository>();
            services.AddSingleton<IAcquiringBankProvider, AcquiringBankClient>();

            // Uncomment this for using Acquiring Bank stub instead of calling API
            //services.AddSingleton<IAcquiringBankProvider, AcquiringBankStub>();
        }
    }
}
