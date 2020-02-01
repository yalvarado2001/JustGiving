using JG.FinTechTest.Controllers;
using JG.FinTechTest.Models;
using JG.FinTechTest.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Http;
using JG.FinTechTest.Validators;
using System.Collections.Generic;
using System;

namespace JG.FinTechTest.Tests.Controllers
{
    [TestFixture]
    public class GiftAidControllerTests
    {
        Mock<IGiftAidCalculatorService> _giftAidCalculatorServiceMock;
        Mock<IValidator<decimal>> _giftAidValidator;

        GiftAidController _controller;

        [SetUp]
        public void Seup()
        {
            this._giftAidCalculatorServiceMock = new Mock<IGiftAidCalculatorService>();
            this._giftAidValidator = new Mock<IValidator<decimal>>();

            this._controller = new GiftAidController(this._giftAidCalculatorServiceMock.Object, this._giftAidValidator.Object);
        }

        [Test]
        public void ShouldReturnDonationAmountAndCalculatedGiftAid()
        {
            decimal donatedAmount = 100m;
            decimal calculatedGiftAid = 25m;

            this._giftAidCalculatorServiceMock.Setup(_ => _.Calculate(donatedAmount)).Returns(calculatedGiftAid);
            this._giftAidValidator.Setup(_ => _.IsValid(donatedAmount)).Returns(true);

            var response = this._controller.Get(donatedAmount) as OkObjectResult;
            var giftAidResponse = response.Value as GiftAidResponse;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(giftAidResponse.DonationAmount, Is.EqualTo(donatedAmount));
            Assert.That(giftAidResponse.GiftAidAmount, Is.EqualTo(calculatedGiftAid));
        }

        [Test]
        public void ShouldReturnValidationError()
        {
            int validationErrorCode = 1;
            string validationErrorMessage = "test error";

            decimal donatedAmount = 1.99m;
            var validationError = new ValidationError(validationErrorCode, validationErrorMessage);
            var errors = new List<ValidationError> { validationError };

            this._giftAidCalculatorServiceMock.Setup(_ => _.Calculate(donatedAmount)).Verifiable();
            this._giftAidValidator.Setup(_ => _.IsValid(donatedAmount)).Returns(false);
            this._giftAidValidator.Setup(_ => _.Errors).Returns(errors);

            var response = this._controller.Get(donatedAmount) as BadRequestObjectResult;
            var resultErrors = response.Value as List<ValidationError>;

            this._giftAidCalculatorServiceMock.Verify(_ => _.Calculate(It.IsAny<decimal>()), Times.Never);
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(resultErrors.Count, Is.EqualTo(1));
            Assert.That(resultErrors, Is.EqualTo(errors));

        }

        [Test]
        public void ShouldReturnInternalServerError()
        {
            decimal donatedAmount = 100m;

            this._giftAidValidator.Setup(_ => _.IsValid(donatedAmount)).Returns(true);
            this._giftAidCalculatorServiceMock.Setup(_ => _.Calculate(donatedAmount)).Throws<Exception>();

            var response = this._controller.Get(donatedAmount) as StatusCodeResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }
    }
}
