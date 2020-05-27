using System;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Infrastructure.Providers.Models.Responses.V1;

namespace AcquiringBank.Web.Api.Controllers.V1
{
    [ApiController]
    [Route("/acquiringbankapi/v1/payments")]
    public class AcquiringBankPaymentController : ControllerBase
    {
        private readonly Random _randomGenerator = new Random();

        [HttpPost]
        public IActionResult CreatePayment()
        {
            return Ok(GeneratePaymentResponse());
        }

        private AcquiringBankPaymentResponseV1 GeneratePaymentResponse()
        {
            var paymentId = Guid.NewGuid().ToString("N");
            var isSuccessful = _randomGenerator.Next(4) != 0; // 75% success rate
            var response = new AcquiringBankPaymentResponseV1()
            {
                PaymentId = paymentId,
                IsSuccessful = isSuccessful
            };
            return response;
        }
    }
}
