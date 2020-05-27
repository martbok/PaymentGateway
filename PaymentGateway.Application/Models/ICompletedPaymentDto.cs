namespace PaymentGateway.Application.Models
{
    public interface ICompletedPaymentDto : IPaymentRequestDto, IPaymentResponseDto
    {
        public string GetMaskedCardNumber();
    }
}
