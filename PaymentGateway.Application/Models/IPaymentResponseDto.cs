namespace PaymentGateway.Application.Models
{
    public interface IPaymentResponseDto
    {
        public string PaymentId { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
