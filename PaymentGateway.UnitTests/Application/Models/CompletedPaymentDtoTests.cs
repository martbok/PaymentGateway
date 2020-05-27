using FluentAssertions;
using NUnit.Framework;
using PaymentGateway.Application.Models;

namespace PaymentGateway.UnitTests.Application.Models
{
    [TestFixture]
    public class CompletedPaymentDtoTests
    {
        [TestCase("4658582263620043", "465858******0043")]
        [TestCase("6269994658582263620", "626999*********3620")]
        [TestCase("675958226362", "675958**6362")]
        public void ReturnMaskedCardNumber_GivenValidCardNumber(string inputCardNumber, string expectedCardNumber)
        {
            // Arrange
            var sut = new CompletedPaymentDto {CardNumber = inputCardNumber};

            // Act
            var actualCardNumber = sut.GetMaskedCardNumber();

            // Assert
            actualCardNumber.Should().Be(expectedCardNumber);
        }
    }
}
