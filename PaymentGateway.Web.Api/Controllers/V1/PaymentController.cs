using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Services;
using PaymentGateway.Web.Api.Models.Requests.V1;
using PaymentGateway.Web.Api.Models.Responses.V1;

namespace PaymentGateway.Web.Api.Controllers.V1
{
    [ApiController]
    [Route("/paymentgatewayapi/v1/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<CreatePaymentResponseV1>> CreatePayment([FromBody] CreatePaymentRequestV1 newPaymentRequestBodyV1, CancellationToken cancellationToken)
        {
            var newPaymentDto = _mapper.Map<CompletedPaymentDto>(newPaymentRequestBodyV1);
            var paymentResult = await _paymentService.ProcessPaymentAsync(newPaymentDto, cancellationToken);
            var paymentResultResponse = _mapper.Map<CreatePaymentResponseV1>(paymentResult);
            return CreatedAtRoute(nameof(GetPayment), new { paymentId = paymentResultResponse.PaymentId }, paymentResultResponse);
        }

        [HttpGet("{paymentId}", Name = nameof(GetPayment))]
        public ActionResult<ProcessedPaymentResponseV1> GetPayment(string paymentId)
        {
            var processedPayment = _paymentService.RetrievePayment(paymentId);
            if (processedPayment == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProcessedPaymentResponseV1>(processedPayment));
        }
    }
}
