using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Services
{
    public interface IPaymentService
    {
        public Task<IPaymentResponseDto> ProcessPaymentAsync(IPaymentRequestDto paymentRequest, CancellationToken cancellationToken);
        public ICompletedPaymentDto RetrievePayment(string paymentId);
    }
}
