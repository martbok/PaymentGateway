using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using PaymentGateway.Application.Configurations;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Providers;
using PaymentGateway.Infrastructure.Providers.Models.Requests.V1;
using PaymentGateway.Infrastructure.Providers.Models.Responses.V1;

namespace PaymentGateway.Infrastructure.Providers
{
    public class AcquiringBankClient : IAcquiringBankProvider
    {
        private const string PaymentsUrlPath = "/acquiringbankapi/v1/payments";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAcquiringBankClientConfiguration _configuration;
        private readonly IMapper _mapper;

        public AcquiringBankClient(IHttpClientFactory httpClientFactory, IAcquiringBankClientConfiguration configuration, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<IPaymentResponseDto> ProcessPaymentAsync(IPaymentRequestDto acquiringBankPaymentRequest, CancellationToken cancellationToken)
        {
            var requestUri = new Uri(_configuration.BaseUri, PaymentsUrlPath);
            var requestBody = _mapper.Map<AcquiringBankPaymentRequestV1>(acquiringBankPaymentRequest);
            var jsonRequestBody = JsonSerializer.Serialize(requestBody, JsonSerializerOptions);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(jsonRequestBody, Encoding.UTF8, MediaTypeNames.Application.Json)
            };

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();

            var jsonResponseBody = await httpResponse.Content.ReadAsStringAsync();
            var responseBody = JsonSerializer.Deserialize<AcquiringBankPaymentResponseV1>(jsonResponseBody, JsonSerializerOptions);
            return _mapper.Map<CompletedPaymentDto>(responseBody);
        }

        private static JsonSerializerOptions JsonSerializerOptions =>
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
    }
}
