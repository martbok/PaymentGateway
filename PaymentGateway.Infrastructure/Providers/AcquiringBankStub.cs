using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Providers;
using PaymentGateway.Infrastructure.Providers.Models.Requests.V1;
using PaymentGateway.Infrastructure.Providers.Models.Responses.V1;

namespace PaymentGateway.Infrastructure.Providers
{
    public class AcquiringBankStub : IAcquiringBankProvider
    {
        private readonly IMapper _mapper;
        private readonly Random _randomGenerator = new Random();

        public AcquiringBankStub(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<IPaymentResponseDto> ProcessPaymentAsync(IPaymentRequestDto acquiringBankPaymentRequest, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<AcquiringBankPaymentRequestV1>(acquiringBankPaymentRequest);
            if (request == null)
            {
                throw new InvalidOperationException("Mapping to AcquiringBankPaymentRequestV1 is misconfigured.");
            }

            return Task.FromResult((IPaymentResponseDto)_mapper.Map<CompletedPaymentDto>(GeneratePaymentResponse()));
        }

        private AcquiringBankPaymentResponseV1 GeneratePaymentResponse()
        {
            var paymentId = Guid.NewGuid().ToString("N");
            var isSuccessful = _randomGenerator.Next(4) != 0; // 75% success rate
            var response = new AcquiringBankPaymentResponseV1()
            {
                PaymentId = paymentId,
                IsSuccessful = isSuccessful
            };
            return response;
        }
    }
}
