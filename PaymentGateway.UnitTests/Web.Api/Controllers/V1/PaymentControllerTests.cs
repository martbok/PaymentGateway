using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Services;
using PaymentGateway.Web.Api.Controllers.V1;
using PaymentGateway.Web.Api.Models.Requests.V1;
using PaymentGateway.Web.Api.Models.Responses.V1;
using PaymentGateway.Web.Api.Registrations.AutoMapperProfiles;

namespace PaymentGateway.UnitTests.Web.Api.Controllers.V1
{
    [TestFixture]
    public class PaymentControllerTests
    {
        private IPaymentService _fakePaymentService;
        private PaymentController _sut;

        [SetUp]
        public void SetUp()
        {
            _fakePaymentService = A.Fake<IPaymentService>();
            A.CallTo(() => _fakePaymentService.ProcessPaymentAsync(A<IPaymentRequestDto>.That.Matches(m => IsEquivalent(m, SamplePaymentRequestDto)), CancellationToken.None)).Returns(SamplePaymentResponseDto);
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new PaymentProfile())));
            _sut = new PaymentController(_fakePaymentService, mapper);
        }

        [Test]
        public async Task CallPaymentServiceWithNewPayment_GivenValidPaymentRequest_WhenCallingCreatePayment()
        {
            // Arrange
            var createPaymentRequest = new CreatePaymentRequestV1()
            {
                CardNumber = "4658582263620043",
                ExpiryDate = "0824",
                Amount = 10m,
                CurrencyCode = "GBP",
                Ccv = "001"
            };
            var expectedPaymentResponse = new CreatePaymentResponseV1()
            {
                PaymentId = "AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE",
                IsSuccessful = true
            };
            
            // Act 
            var actualResponse = await _sut.CreatePayment(createPaymentRequest, CancellationToken.None);

            // Assert
            actualResponse.Result.Should().BeOfType<CreatedAtRouteResult>();
            ((ObjectResult)actualResponse.Result).Value.Should().BeEquivalentTo(expectedPaymentResponse);
        }

        [Test]
        public void ReturnPaymentDetails_GivenPaymentExits_WhenCallingGetPayment()
        {
            // Arrange
            var paymentId = "ExistentPaymentId";
            var paymentServiceResponse = new CompletedPaymentDto()
            {
                CardNumber = "4658582263620043",
                ExpiryDate = "0824",
                Amount = 10m,
                CurrencyCode = "GBP",
                Ccv = "001",
                PaymentId = "AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE",
                IsSuccessful = true
            };
            A.CallTo(() => _fakePaymentService.RetrievePayment(paymentId)).Returns(paymentServiceResponse);
            var expectedPaymentResponse = new ProcessedPaymentResponseV1()
            {
                CardNumber = "4658582263620043",
                ExpiryDate = "0824",
                Amount = 10m,
                CurrencyCode = "GBP",
                Ccv = "001",
                PaymentId = "AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE",
                IsSuccessful = true
            };

            // Act 
            var actualResponse = _sut.GetPayment(paymentId);

            // Assert
            actualResponse.Result.Should().BeOfType<OkObjectResult>();
            ((ObjectResult)actualResponse.Result).Value.Should().BeEquivalentTo(expectedPaymentResponse);
        }


        [Test]
        public void ReturnNotFound_GivenPaymentDoesNotExit_WhenCallingGetPayment()
        {
            // Arrange
            var paymentId = "NonExistentPaymentId";
            A.CallTo(() => _fakePaymentService.RetrievePayment(paymentId)).Returns(null);

            // Act 
            var actualResponse = _sut.GetPayment(paymentId);

            // Assert
            actualResponse.Result.Should().BeOfType<NotFoundResult>();
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

        private static IPaymentRequestDto SamplePaymentRequestDto =>
            new CompletedPaymentDto()
            {
                CardNumber = "4658582263620043",
                ExpiryDate = "0824",
                Amount = 10m,
                CurrencyCode = "GBP",
                Ccv = "001",
                PaymentId = "AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE",
                IsSuccessful = true
            };

        private static IPaymentResponseDto SamplePaymentResponseDto =>
            new CompletedPaymentDto()
            {
                PaymentId = "AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE",
                IsSuccessful = true
            };

    }
}
