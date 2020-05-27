using System;

namespace PaymentGateway.Application.Models
{
    public interface IPaymentRequestDto
    {
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Ccv { get; set; }
    }
}
