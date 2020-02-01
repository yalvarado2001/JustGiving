using JG.FinTechTest.Validators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JG.FinTechTest.Tests.Validators
{
    [TestFixture]
    public class MinimumDonationValidatorTests
    {
        private static readonly int MinimumValidationErrorCode = 1;
        private static readonly string MinimumValidationErrorDescription = "Minimum donation amount is £2.00";

        [TestCase(10,true)]
        [TestCase(1.99,false)]
        [TestCase(2, true)]
        public void ShouldValidateForAmountsGreaterThanMinimumAndFailOtherwise(decimal donationAmount, bool expectedResult)
        {
            IValidatonRule<decimal> validator = new MinimumDonationValidator();

            bool result = validator.Validates(donationAmount);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void ShouldReturnTheExpectedErrorCodeAndDescriptionForFailedValidations()
        {
            IValidatonRule<decimal> validator = new MinimumDonationValidator();

            bool result = validator.Validates(1m);

            Assert.That(result, Is.EqualTo(false));
            Assert.That(validator.Error.ErrorCode, Is.EqualTo(MinimumValidationErrorCode));
            Assert.That(validator.Error.ErrorDescription, Is.EqualTo(MinimumValidationErrorDescription));
        }
    }
}
