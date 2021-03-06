﻿namespace PaymentGateway.Web.Api.Models.Responses.V1
{
    public class ProcessedPaymentResponseV1
    {
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Ccv { get; set; }
        public string PaymentId { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
