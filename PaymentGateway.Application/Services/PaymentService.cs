using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Providers;
using PaymentGateway.Application.Repositories;

namespace PaymentGateway.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAcquiringBankProvider _acquiringBankProvider;

        public PaymentService(IPaymentRepository paymentRepository, IAcquiringBankProvider acquiringBankProvider)
        {
            _paymentRepository = paymentRepository;
            _acquiringBankProvider = acquiringBankProvider;
        }

        public async Task<IPaymentResponseDto> ProcessPaymentAsync(IPaymentRequestDto paymentRequest, CancellationToken cancellationToken)
        {
            var paymentResult = await _acquiringBankProvider.ProcessPaymentAsync(paymentRequest, cancellationToken);
            var completedPaymentDto = new CompletedPaymentDto(paymentRequest, paymentResult);
            _paymentRepository.SavePayment(completedPaymentDto);
            return paymentResult;
        }

        ICompletedPaymentDto IPaymentService.RetrievePayment(string paymentId)
        {
            var paymentDetail = _paymentRepository.GetPayment(paymentId);
            if (paymentDetail != null)
            {
                paymentDetail.CardNumber = paymentDetail.GetMaskedCardNumber(); 
            }
            
            return paymentDetail;
        }
    }
}
