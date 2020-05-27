using FluentValidation;
using PaymentGateway.Web.Api.Models.Requests.V1;

namespace PaymentGateway.Web.Api.Validators
{
    public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequestV1>
    {
        public CreatePaymentRequestValidator()
        {
            RuleFor(p => p.CardNumber).CreditCard();
            RuleFor(p => p.ExpiryDate).Matches("^(?:0[1-9]|1[0-2])[0-9]{2}$").WithMessage("{PropertyName} '{PropertyValue}' is invalid. Required format is 'mmyy'.");
            RuleFor(p => p.Amount).GreaterThan(0m).WithMessage("{PropertyName} '{PropertyValue}' to pay is invalid. Required format is positive amount.");
            RuleFor(p => p.CurrencyCode).Matches("^[a-zA-Z]{3}$").WithMessage("{PropertyName} '{PropertyValue}' is invalid. Required format is ISO 4217 3 letter alphabetic code.");
            RuleFor(p => p.Ccv).Matches(@"^\d{3}$").WithMessage("{PropertyName} '{PropertyValue}' is invalid. Required format is 3 digits.");
        }
    }
}
