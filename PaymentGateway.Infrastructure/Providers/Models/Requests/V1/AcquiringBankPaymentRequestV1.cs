namespace PaymentGateway.Infrastructure.Providers.Models.Requests.V1
{
    public class AcquiringBankPaymentRequestV1
    {
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Ccv { get; set; }
    }
}
