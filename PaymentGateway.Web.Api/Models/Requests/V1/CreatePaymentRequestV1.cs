namespace PaymentGateway.Web.Api.Models.Requests.V1
{
    public class CreatePaymentRequestV1
    {
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Ccv { get; set; }
    }
}
