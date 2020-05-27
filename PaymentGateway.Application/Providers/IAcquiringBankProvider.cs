using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Providers
{
    public interface IAcquiringBankProvider
    {
        public Task<IPaymentResponseDto> ProcessPaymentAsync(IPaymentRequestDto acquiringBankPaymentRequest, CancellationToken cancellationToken);
    }
}
