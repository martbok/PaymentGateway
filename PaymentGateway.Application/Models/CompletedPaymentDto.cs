using System;
using System.Text.RegularExpressions;

namespace PaymentGateway.Application.Models
{
    public class CompletedPaymentDto : ICompletedPaymentDto
    {
        public CompletedPaymentDto() { }

        public CompletedPaymentDto(IPaymentRequestDto paymentRequest, IPaymentResponseDto paymentResponse)
        {
            CardNumber = paymentRequest.CardNumber;
            ExpiryDate = paymentRequest.ExpiryDate;
            Amount = paymentRequest.Amount;
            CurrencyCode = paymentRequest.CurrencyCode;
            Ccv = paymentRequest.Ccv;
            PaymentId = paymentResponse.PaymentId;
            IsSuccessful = paymentResponse.IsSuccessful;
        }

        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Ccv { get; set; }
        public string PaymentId { get; set; }
        public bool IsSuccessful { get; set; }

        public string GetMaskedCardNumber()
        {
            return Regex.Replace(CardNumber, @"^(.{6})(.+)(.{4})$", m =>
                $"{m.Groups[1].Value}{Regex.Replace(m.Groups[2].Value, ".", "*")}{m.Groups[3].Value}");
        }
    }
}
