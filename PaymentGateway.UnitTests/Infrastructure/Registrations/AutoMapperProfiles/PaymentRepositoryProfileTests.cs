using AutoMapper;
using NUnit.Framework;
using PaymentGateway.Infrastructure.Registrations.AutoMapperProfiles;

namespace PaymentGateway.UnitTests.Infrastructure.Registrations.AutoMapperProfiles
{
    [TestFixture]
    public class PaymentRepositoryProfileTests
    { 
        [Test]
        public void AllPropertiesHaveMappings()
        {
            // Arrange
            var sut = new MapperConfiguration(cfg => cfg.AddProfile(new PaymentRepositoryProfile()));

            // Assert
            sut.AssertConfigurationIsValid();
        }
    }
}
