using System;

namespace PaymentGateway.Application.Configurations
{
    public interface IAcquiringBankClientConfiguration
    {
        public Uri BaseUri { get; }
    }
}
