using JG.FinTechTest.Validators;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JG.FinTechTest.Tests.Validators
{
    [TestFixture]
    public class GiftAidValidatorTests
    {        
        private IValidator<decimal> _giftAidValidator;
        private Mock<IValidationRule<decimal>> _firstValidationRule;
        private Mock<IValidationRule<decimal>> _secondValidationRule;

        [SetUp]
        public void Setup()
        {
            this._firstValidationRule = new Mock<IValidationRule<decimal>>();
            this._secondValidationRule = new Mock<IValidationRule<decimal>>();

            this._giftAidValidator = new GiftAidValidator(new List<IValidationRule<decimal>>
            {
                this._firstValidationRule.Object,
                this._secondValidationRule.Object
            });
        }

        [Test]
        public void ShouldValidateWhenAllRulesValidate()
        {
            this._firstValidationRule.Setup(_ => _.Validates(It.IsAny<decimal>())).Returns(true);
            this._secondValidationRule.Setup(_ => _.Validates(It.IsAny<decimal>())).Returns(true);

            var result = this._giftAidValidator.IsValid(10m);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void ShouldFailValidationWhenAtLeastOneRuleFailsValidation()
        {
            var expectedValidationError = new ValidationError(1, "test error");

            this._firstValidationRule.Setup(_ => _.Validates(It.IsAny<decimal>())).Returns(false);
            this._firstValidationRule.Setup(_ => _.Error).Returns(expectedValidationError);

            this._secondValidationRule.Setup(_ => _.Validates(It.IsAny<decimal>())).Returns(true);

            var result = this._giftAidValidator.IsValid(1m);
            var errors = this._giftAidValidator.Errors;

            Assert.That(result, Is.EqualTo(false));
            Assert.That(errors.Count, Is.EqualTo(1));

            Assert.That(errors.First(), Is.EqualTo(expectedValidationError));

        }

    }
}
