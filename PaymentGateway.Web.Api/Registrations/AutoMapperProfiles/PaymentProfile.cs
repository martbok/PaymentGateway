using AutoMapper;
using PaymentGateway.Application.Models;
using PaymentGateway.Web.Api.Models.Requests.V1;
using PaymentGateway.Web.Api.Models.Responses.V1;

namespace PaymentGateway.Web.Api.Registrations.AutoMapperProfiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<CreatePaymentRequestV1, CompletedPaymentDto>();
            CreateMap<CompletedPaymentDto, CreatePaymentResponseV1>();
            CreateMap<CompletedPaymentDto, ProcessedPaymentResponseV1>();
        }
    }
}
