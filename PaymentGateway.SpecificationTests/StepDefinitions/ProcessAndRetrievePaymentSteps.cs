using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PaymentGateway.Application.Configurations;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Providers;
using PaymentGateway.Application.Repositories;
using PaymentGateway.Infrastructure.Providers.Models.Responses.V1;
using PaymentGateway.Web.Api;
using PaymentGateway.Web.Api.Models.Requests.V1;
using PaymentGateway.Web.Api.Models.Responses.V1;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace PaymentGateway.SpecificationTests.StepDefinitions
{
    [Binding]
    public class ProcessAndRetrievePaymentSteps
    {
        private IPaymentRepository _fakePaymentRepository;
        private static readonly Uri BaseUri = new Uri("https://localhost:443");
        private HttpClient _testHttpClient;
        private HttpResponseMessage _testClientResponseMessage;
        private IAcquiringBankProvider _fakeAcquiringBankProvider;

        [BeforeScenario]
        public void BeforeScenarioSetUp()
        { 
            var fakeClientConfiguration = A.Fake<IAcquiringBankClientConfiguration>();
            _fakePaymentRepository = A.Fake<IPaymentRepository>();
            _fakeAcquiringBankProvider = A.Fake<IAcquiringBankProvider>();

            var webHostBuilder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    services.Replace(new ServiceDescriptor(typeof(IAcquiringBankClientConfiguration), sp => fakeClientConfiguration, ServiceLifetime.Singleton));
                    services.Replace(new ServiceDescriptor(typeof(IPaymentRepository), sp => _fakePaymentRepository, ServiceLifetime.Singleton));
                    services.Replace(new ServiceDescriptor(typeof(IAcquiringBankProvider), sp => _fakeAcquiringBankProvider, ServiceLifetime.Singleton));
                });

            var testServer = new TestServer(webHostBuilder);
            _testHttpClient = testServer.CreateClient();
        }

        [Given(@"the previous payment is stored")]
        public void GivenThePreviousPaymentIsStored(Table table)
        {
            var storedPayment = table.CreateInstance<CompletedPaymentDto>();
            A.CallTo(() => _fakePaymentRepository.GetPayment(storedPayment.PaymentId)).Returns(storedPayment);
        }

        [Given(@"the payment does not exist")]
        public void GivenThePaymentDoesNotExist()
        {
            A.CallTo(() => _fakePaymentRepository.GetPayment(A<string>._)).Returns(null);
        }

        [Then(@"the processed payment is stored")]
        public void ThenTheProcessedPaymentIsStored(Table table)
        {
            var storedPayment = table.CreateInstance<CompletedPaymentDto>();
            A.CallTo(() => _fakePaymentRepository.SavePayment(A<ICompletedPaymentDto>.That.Matches(m => IsEquivalent(m, storedPayment)))).MustHaveHappenedOnceExactly();
        }

        [Given(@"the Acquiring Bank is set to respond")]
        public void GivenTheAcquiringBankIsSetToRespond(Table table)
        {
            IPaymentResponseDto paymentResponse = table.CreateInstance<CompletedPaymentDto>();
            A.CallTo(() => _fakeAcquiringBankProvider.ProcessPaymentAsync(A<IPaymentRequestDto>._, A<CancellationToken>._)).Returns(paymentResponse);
        }

        [When(@"the call to Payment Gateway Api with PaymentId '(.*)' is made")]
        public async Task WhenICallPaymentGatewayApiWithPaymentId(string paymentId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(BaseUri, $"/paymentgatewayapi/v1/payments/{paymentId}"));
            _testClientResponseMessage = await _testHttpClient.SendAsync(request);
        }

        [When(@"the call to Payment Gateway Api with a new payment is made")]
        public async Task WhenTheCallToPaymentGatewayApiWithANewPaymentIsMade(Table table)
        {
            var createPaymentRequest = table.CreateInstance<CreatePaymentRequestV1>();
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(BaseUri, $"/paymentgatewayapi/v1/payments"))
            {
                Content = new StringContent(JsonSerializer.Serialize(createPaymentRequest, JsonSerializerOptions), Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            _testClientResponseMessage = await _testHttpClient.SendAsync(request);
        }

        [Then(@"the payment details are received")]
        public async Task ThenIReceiveThePaymentDetails(Table table)
        {
            var expectedResponseBody = table.CreateInstance<ProcessedPaymentResponseV1>();
            _testClientResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            var actualResponseBody = await GetResponseBody<ProcessedPaymentResponseV1>(_testClientResponseMessage.Content);
            actualResponseBody.Should().BeEquivalentTo(expectedResponseBody);
        }

        [Then(@"the successful payment response is received")]
        public async Task ThenTheSuccessfulPaymentResponseIsReceived(Table table)
        {
            var expectedResponseBody = table.CreateInstance<CreatePaymentResponseV1>();
            _testClientResponseMessage.StatusCode.Should().Be(HttpStatusCode.Created);
            var actualResponseBody = await GetResponseBody<CreatePaymentResponseV1>(_testClientResponseMessage.Content);
            actualResponseBody.Should().BeEquivalentTo(expectedResponseBody);
        }

        [Then(@"not the found result is received")]
        public void ThenNotTheFoundResultIsReceived()
        {
            _testClientResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private static async Task<T> GetResponseBody<T>(HttpContent httpContent)
        {
            var responseString = await httpContent.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, JsonSerializerOptions);
        }

        private static bool IsEquivalent<T>(T actualInput, T expectedInput)
        {
            try
            {
                actualInput.Should().BeEquivalentTo(expectedInput);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static JsonSerializerOptions JsonSerializerOptions =>
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
    }
}