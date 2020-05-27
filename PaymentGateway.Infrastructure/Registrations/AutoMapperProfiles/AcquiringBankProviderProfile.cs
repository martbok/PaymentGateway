using AutoMapper;
using PaymentGateway.Application.Models;
using PaymentGateway.Infrastructure.Providers.Models.Requests.V1;
using PaymentGateway.Infrastructure.Providers.Models.Responses.V1;

namespace PaymentGateway.Infrastructure.Registrations.AutoMapperProfiles
{
    public class AcquiringBankProviderProfile : Profile
    {
        public AcquiringBankProviderProfile()
        {
            CreateMap<CompletedPaymentDto, AcquiringBankPaymentRequestV1>();
            CreateMap<AcquiringBankPaymentResponseV1, CompletedPaymentDto>();
        }
    }
}
