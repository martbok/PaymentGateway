using System;
using Microsoft.Extensions.Configuration;
using PaymentGateway.Application.Configurations;

namespace PaymentGateway.Web.Api.Configurations
{
    public class AcquiringBankClientConfiguration : IAcquiringBankClientConfiguration
    {
        public AcquiringBankClientConfiguration(IConfiguration configuration)
        {
            BaseUri = new Uri(configuration["AcquiringBankClientConfiguration:BaseUri"]);
        }

        public Uri BaseUri { get; }
    }
}
