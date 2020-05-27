using FluentValidation.TestHelper;
using NUnit.Framework;
using PaymentGateway.Web.Api.Validators;

namespace PaymentGateway.UnitTests.Web.Api.Validators
{
    [TestFixture]
    public class CreatePaymentRequestValidatorTests
    {
        private CreatePaymentRequestValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CreatePaymentRequestValidator();
        }

        [Test]
        public void Fail_GivenCardNumberIsInvalid()
        {
            // Assert
            _sut.ShouldHaveValidationErrorFor(p => p.CardNumber, "4658582263620044");
        }

        [Test]
        public void Pass_GivenCardNumberIsValid()
        {
            // Assert
            _sut.ShouldNotHaveValidationErrorFor(p => p.CardNumber, "4658582263620043");
        }

        [TestCase("0021")]
        [TestCase("1321")]
        [TestCase("121")]
        [TestCase("122020")]
        public void Fail_GivenExpiryDateIsInvalid(string inputExpiryDate)
        {
            // Assert
            _sut.ShouldHaveValidationErrorFor(p => p.ExpiryDate, inputExpiryDate).WithErrorMessage($"Expiry Date '{inputExpiryDate}' is invalid. Required format is 'mmyy'.");
        }

        [Test]
        public void Pass_GivenExpiryDateIsValid()
        {
            // Assert
            _sut.ShouldNotHaveValidationErrorFor(p => p.ExpiryDate, "1221");
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Fail_GivenAmountIsInvalid(decimal inputAmount)
        {
            // Assert
            _sut.ShouldHaveValidationErrorFor(p => p.Amount, inputAmount).WithErrorMessage($"Amount '{inputAmount}' to pay is invalid. Required format is positive amount.");
        }

        [Test]
        public void Pass_GivenAmountIsValid()
        {
            // Assert
            _sut.ShouldNotHaveValidationErrorFor(p => p.Amount, 10.50m);
        }

        [TestCase("NI0")]
        [TestCase("EURO")]
        [TestCase("EC")]
        public void Fail_GivenCurrencyCodeIsInvalid(string inputCurrencyCode)
        {
            // Assert
            _sut.ShouldHaveValidationErrorFor(p => p.CurrencyCode, inputCurrencyCode).WithErrorMessage($"Currency Code '{inputCurrencyCode}' is invalid. Required format is ISO 4217 3 letter alphabetic code.");
        }

        [Test]
        public void Pass_GivenCurrencyCodeIsValid()
        {
            // Assert
            _sut.ShouldNotHaveValidationErrorFor(p => p.CurrencyCode, "GBP");
        }

        [TestCase("l00")]
        [TestCase("1234")]
        [TestCase("12")]
        public void Fail_GivenCcvIsInvalid(string inputCcv)
        {
            // Assert
            _sut.ShouldHaveValidationErrorFor(p => p.Ccv, inputCcv).WithErrorMessage($"Ccv '{inputCcv}' is invalid. Required format is 3 digits.");
        }

        [Test]
        public void Pass_GivenCcvIsValid()
        {
            // Assert
            _sut.ShouldNotHaveValidationErrorFor(p => p.Ccv, "000");
        }
    }
}
