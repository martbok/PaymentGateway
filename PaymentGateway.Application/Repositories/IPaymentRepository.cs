using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Repositories
{
    public interface IPaymentRepository
    {
        public void SavePayment(ICompletedPaymentDto newPayment);
        public ICompletedPaymentDto GetPayment(string paymentId);
    }
}
