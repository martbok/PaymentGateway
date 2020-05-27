using AutoMapper;
using PaymentGateway.Application.Models;
using PaymentGateway.Infrastructure.Repositories.Entities;

namespace PaymentGateway.Infrastructure.Registrations.AutoMapperProfiles
{
    public class PaymentRepositoryProfile : Profile
    {
        public PaymentRepositoryProfile()
        {
            CreateMap<CompletedPaymentDto, PaymentEntity>().ReverseMap();
        }
    }
}
