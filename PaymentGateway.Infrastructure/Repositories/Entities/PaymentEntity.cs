namespace PaymentGateway.Infrastructure.Repositories.Entities
{
    public class PaymentEntity
    {
        public string PaymentId { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Ccv { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
