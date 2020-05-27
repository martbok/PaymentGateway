using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using PaymentGateway.Application.Configurations;
using PaymentGateway.Application.Models;
using PaymentGateway.Infrastructure.Providers;
using PaymentGateway.Infrastructure.Providers.Models.Requests.V1;
using PaymentGateway.Infrastructure.Providers.Models.Responses.V1;
using PaymentGateway.Infrastructure.Registrations.AutoMapperProfiles;

namespace PaymentGateway.UnitTests.Infrastructure.Providers
{
    [TestFixture]
    public class AcquiringBankClientTests
    {
        private AcquiringBankClient _sut;
        private FakeHttpMessageHandler _fakeHttpMessageHandler;

        [SetUp]
        public void SetUp()
        {
            _fakeHttpMessageHandler = new FakeHttpMessageHandler { ResponseMessage = SampleHttpResponseMessage };
            var fakeHttpClientFactory = new FakeHttpClientFactory(new HttpClient(_fakeHttpMessageHandler));
            var fakeClientConfiguration = A.Fake<IAcquiringBankClientConfiguration>();
            A.CallTo(() => fakeClientConfiguration.BaseUri).Returns(SampleBaseUri);
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AcquiringBankProviderProfile())));

            _sut = new AcquiringBankClient(fakeHttpClientFactory, fakeClientConfiguration, mapper);
        }

        [Test]
        public async Task RequestCorrectUrl_GivenValidPaymentRequest()
        {
            // Arrange
            var expectedUrl = "https://localhost:44311/acquiringbankapi/v1/payments";

            // Act 
            await _sut.ProcessPaymentAsync(SamplePaymentRequest, CancellationToken.None);

            // Assert
            var actualUrl = _fakeHttpMessageHandler.RequestMessage.RequestUri.AbsoluteUri;
            actualUrl.Should().Be(expectedUrl);
        }

        [Test]
        public async Task RequestHttpMethod_GivenValidPaymentRequest()
        {
            // Arrange
            var expectedHttpMethod = HttpMethod.Post;

            // Act 
            await _sut.ProcessPaymentAsync(SamplePaymentRequest, CancellationToken.None);

            // Assert
            var actualHttpMethod = _fakeHttpMessageHandler.RequestMessage.Method;
            actualHttpMethod.Should().Be(expectedHttpMethod);
        }

        [Test]
        public async Task SendValidRequestBody_GivenValidPaymentRequest()
        {
            // Arrange
            var inputPayment = SamplePaymentRequest;
            var expectedRequestBody = new AcquiringBankPaymentRequestV1()
            {
                CardNumber = "4658582263620043",
                ExpiryDate = "0824",
                Amount = 10m,
                CurrencyCode = "GBP",
                Ccv = "001"
            };

            // Act 
            await _sut.ProcessPaymentAsync(inputPayment, CancellationToken.None);

            // Assert
            var jsonResponse = await _fakeHttpMessageHandler.RequestMessage.Content.ReadAsStringAsync();
            var actualResponseBody = JsonSerializer.Deserialize<AcquiringBankPaymentRequestV1>(jsonResponse, JsonSerializerOptions);
            actualResponseBody.Should().BeEquivalentTo(expectedRequestBody);
        }

        [Test]
        public async Task ReturnValidPaymentResponse_GivenSuccessfulApiResponse()
        {
            // Arrange
            IPaymentResponseDto expectedPaymentResponse = new CompletedPaymentDto()
            {
                PaymentId = "AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE",
                IsSuccessful = true
            };

            // Act 
            var actualPaymentResponse = await _sut.ProcessPaymentAsync(SamplePaymentRequest, CancellationToken.None);

            // Assert
            actualPaymentResponse.Should().BeEquivalentTo(expectedPaymentResponse);
        }

        [Test]
        public void Throw_GivenUnsuccessfulApiResponse()
        {
            // Arrange
            _fakeHttpMessageHandler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            // Act 
            Func<Task> action = async () => await _sut.ProcessPaymentAsync(SamplePaymentRequest, CancellationToken.None);

            // Assert
            action.Should().Throw<HttpRequestException>();
        }

        private static Uri SampleBaseUri => new Uri("https://localhost:44311");

        private static HttpResponseMessage SampleHttpResponseMessage =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new AcquiringBankPaymentResponseV1()
                {
                    PaymentId = "AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE",
                    IsSuccessful = true
                }, JsonSerializerOptions), Encoding.UTF8, MediaTypeNames.Application.Json)
            };

        private static IPaymentRequestDto SamplePaymentRequest =>
            new CompletedPaymentDto()
            {
                CardNumber = "4658582263620043",
                ExpiryDate = "0824",
                Amount = 10m,
                CurrencyCode = "GBP",
                Ccv = "001"
            };

        private static JsonSerializerOptions JsonSerializerOptions =>
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
    }

    public class FakeHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _httpClient;

        public FakeHttpClientFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HttpClient CreateClient(string name)
        {
            return _httpClient;
        }
    }

    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public HttpRequestMessage RequestMessage { get; set; }
        public HttpResponseMessage ResponseMessage { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            RequestMessage = requestMessage;
            return Task.FromResult(ResponseMessage);
        }
    }
}
