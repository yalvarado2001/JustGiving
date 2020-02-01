using JG.FinTechTest.Validators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JG.FinTechTest.Tests.Validators
{
    [TestFixture]
    class MaximumDonationValidatorTests
    {
        private static readonly int MinimumValidationErrorCode = 2;
        private static readonly string MinimumValidationErrorDescription = "Maximum donation amount is £100,000.00";

        [TestCase(100000, true)]
        [TestCase(100000.01, false)]
        [TestCase(100, true)]
        [TestCase(100010, false)]
        public void ShouldValidateForAmountsLessThanMaximumAndFailOtherwise(decimal donationAmount, bool expectedResult)
        {
            IValidationRule<decimal> validator = new MaximumDonationValidator();

            bool result = validator.Validates(donationAmount);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void ShouldReturnTheExpectedErrorCodeAndDescriptionForFailedValidations()
        {
            IValidationRule<decimal> validator = new MaximumDonationValidator();

            bool result = validator.Validates(100000.01m);

            Assert.That(result, Is.EqualTo(false));
            Assert.That(validator.Error.ErrorCode, Is.EqualTo(MinimumValidationErrorCode));
            Assert.That(validator.Error.ErrorDescription, Is.EqualTo(MinimumValidationErrorDescription));
        }
    }
}
