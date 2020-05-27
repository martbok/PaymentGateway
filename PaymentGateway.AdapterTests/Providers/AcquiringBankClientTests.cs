using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using PaymentGateway.Application.Models;
using PaymentGateway.Infrastructure.Providers;
using PaymentGateway.Infrastructure.Registrations.AutoMapperProfiles;
using PaymentGateway.Web.Api.Configurations;

namespace PaymentGateway.AdapterTests.Providers
{
    [TestFixture]
    public class AcquiringBankClientTests
    {
        private const string AppSettingsFileName = "appsettings.json";
        private AcquiringBankClient _sut;

        [SetUp]
        public void SetUp()
        {
            IHttpClientFactory httpClientFactory = new HttpClientFactory();
            var appSettings = new ConfigurationBuilder().AddJsonFile(AppSettingsFileName).Build();
            var clientConfiguration = new AcquiringBankClientConfiguration(appSettings);
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AcquiringBankProviderProfile())));

            _sut = new AcquiringBankClient(httpClientFactory, clientConfiguration, mapper);
        }

        [Test]
        public async Task ReturnPaymentResult_GivenValidPaymentRequest()
        {
            // Arrange
            IPaymentRequestDto requestPayment = new CompletedPaymentDto()
            {
                CardNumber = "4658582263620043",
                ExpiryDate = "0824",
                Amount = 10m,
                CurrencyCode = "GBP",
                Ccv = "001"
            };

            IPaymentResponseDto expectedPaymentResponse = new CompletedPaymentDto()
            {
                PaymentId = "AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE",
                IsSuccessful = true
            };

            // Act
            var actualPaymentResponse = await _sut.ProcessPaymentAsync(requestPayment, CancellationToken.None);

            // Assert
            Assert.That(actualPaymentResponse, Is.Not.Null);
            Assert.That(actualPaymentResponse.PaymentId, Is.Not.Empty);
        }

        private class HttpClientFactory : IHttpClientFactory
        {
            public HttpClient CreateClient(string name)
            {
                return new HttpClient();
            }
        }
    }
}
