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
        Mock<IValidator<decimal>> _giftAidValidatorMock;
        Mock<IGiftAidDonorService> _giftAidDonorServiceMock;

        GiftAidController _controller;

        [SetUp]
        public void Seup()
        {
            this._giftAidCalculatorServiceMock = new Mock<IGiftAidCalculatorService>();
            this._giftAidValidatorMock = new Mock<IValidator<decimal>>();
            this._giftAidDonorServiceMock = new Mock<IGiftAidDonorService>();

            this._controller = new GiftAidController(this._giftAidCalculatorServiceMock.Object, this._giftAidValidatorMock.Object, this._giftAidDonorServiceMock.Object);
        }

        [Test]
        public void ShouldReturnDonationAmountAndCalculatedGiftAidWhenCallingGiftAid()
        {
            decimal donatedAmount = 100m;
            decimal calculatedGiftAid = 25m;

            this._giftAidCalculatorServiceMock.Setup(_ => _.Calculate(donatedAmount)).Returns(calculatedGiftAid);
            this._giftAidValidatorMock.Setup(_ => _.IsValid(donatedAmount)).Returns(true);

            var response = this._controller.Get(donatedAmount) as OkObjectResult;
            var giftAidResponse = response.Value as GiftAidResponse;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(giftAidResponse.DonationAmount, Is.EqualTo(donatedAmount));
            Assert.That(giftAidResponse.GiftAidAmount, Is.EqualTo(calculatedGiftAid));
        }

        [Test]
        public void ShouldReturnValidationErrorWhenCallingGiftAid()
        {
            int validationErrorCode = 1;
            string validationErrorMessage = "test error";

            decimal donatedAmount = 1.99m;
            var validationError = new ValidationError(validationErrorCode, validationErrorMessage);
            var errors = new List<ValidationError> { validationError };

            this._giftAidCalculatorServiceMock.Setup(_ => _.Calculate(donatedAmount)).Verifiable();
            this._giftAidValidatorMock.Setup(_ => _.IsValid(donatedAmount)).Returns(false);
            this._giftAidValidatorMock.Setup(_ => _.Errors).Returns(errors);

            var response = this._controller.Get(donatedAmount) as BadRequestObjectResult;
            var resultErrors = response.Value as List<ValidationError>;

            this._giftAidCalculatorServiceMock.Verify(_ => _.Calculate(It.IsAny<decimal>()), Times.Never);
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(resultErrors.Count, Is.EqualTo(1));
            Assert.That(resultErrors, Is.EqualTo(errors));

        }

        [Test]
        public void ShouldReturnInternalServerErrorWhenCallingGiftAid()
        {
            decimal donatedAmount = 100m;

            this._giftAidValidatorMock.Setup(_ => _.IsValid(donatedAmount)).Returns(true);
            this._giftAidCalculatorServiceMock.Setup(_ => _.Calculate(donatedAmount)).Throws<Exception>();

            var response = this._controller.Get(donatedAmount) as StatusCodeResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }


        [Test]
        public void ShouldReturnGiftAidDonorResponse()
        {
            var giftAidDonorRequest = this.GetGiftAidDonorRequest();
            decimal donatedAmount = 100m;
            decimal calculatedGiftAid = 25m;

            var expectedGiftAidDonorResponse = this.GetExpectedGiftDonorResponse();

            this._giftAidCalculatorServiceMock.Setup(_ => _.Calculate(donatedAmount)).Returns(calculatedGiftAid);
            this._giftAidValidatorMock.Setup(_ => _.IsValid(donatedAmount)).Returns(true);
            this._giftAidDonorServiceMock.Setup(_ => _.SaveDonorDetails(giftAidDonorRequest)).Returns(expectedGiftAidDonorResponse);

            var response = this._controller.Post(giftAidDonorRequest) as OkObjectResult;
            var donorResponse = response.Value as GiftAidDonorResponse;

            Assert.That(donorResponse, Is.EqualTo(expectedGiftAidDonorResponse));

        }

        [Test]
        public void ShouldReturnValidationErrorWhenSavingDonorWithInvalidData()
        {
            int validationErrorCode = 1;
            string validationErrorMessage = "test error";

            GiftAidDonorRequest giftAidDonorRequest = this.GetGiftAidDonorRequest();
            giftAidDonorRequest.DonationAmount = 1.99m;

            var validationError = new ValidationError(validationErrorCode, validationErrorMessage);
            var errors = new List<ValidationError> { validationError };

            this._giftAidCalculatorServiceMock.Setup(_ => _.Calculate(giftAidDonorRequest.DonationAmount)).Verifiable();
            this._giftAidDonorServiceMock.Setup(_ => _.SaveDonorDetails(giftAidDonorRequest)).Verifiable();
            this._giftAidValidatorMock.Setup(_ => _.IsValid(giftAidDonorRequest.DonationAmount)).Returns(false);
            this._giftAidValidatorMock.Setup(_ => _.Errors).Returns(errors);

            var response = this._controller.Post(giftAidDonorRequest) as BadRequestObjectResult;
            var resultErrors = response.Value as List<ValidationError>;

            this._giftAidCalculatorServiceMock.Verify(_ => _.Calculate(It.IsAny<decimal>()), Times.Never);
            this._giftAidDonorServiceMock.Verify(_ => _.SaveDonorDetails(giftAidDonorRequest), Times.Never);
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(resultErrors.Count, Is.EqualTo(1));
            Assert.That(resultErrors, Is.EqualTo(errors));
        }

        [Test]
        public void ShouldReturnBadRequestWhenSuppliedModelIsInvalid()
        {
            GiftAidDonorRequest giftAidDonorRequest = this.GetGiftAidDonorRequest();

            this._giftAidCalculatorServiceMock.Setup(_ => _.Calculate(giftAidDonorRequest.DonationAmount)).Verifiable();
            this._giftAidDonorServiceMock.Setup(_ => _.SaveDonorDetails(giftAidDonorRequest)).Verifiable();
            this._giftAidValidatorMock.Setup(_ => _.IsValid(giftAidDonorRequest.DonationAmount)).Verifiable();

            this._controller.ModelState.AddModelError("test-error", "Test error");
            var response = this._controller.Post(giftAidDonorRequest) as BadRequestObjectResult;
            var resultErrors = response.Value as List<ValidationError>;

            this._giftAidCalculatorServiceMock.Verify(_ => _.Calculate(It.IsAny<decimal>()), Times.Never);
            this._giftAidDonorServiceMock.Verify(_ => _.SaveDonorDetails(giftAidDonorRequest), Times.Never);
            this._giftAidValidatorMock.Verify(_ => _.IsValid(giftAidDonorRequest.DonationAmount),Times.Never);

            Assert.That(resultErrors.Count, Is.EqualTo(1));
        }

        [Test]
        public void ShouldReturnInternalServerErrorWhenSavingDonor()
        {
            GiftAidDonorRequest giftAidDonorRequest = this.GetGiftAidDonorRequest();

            this._giftAidValidatorMock.Setup(_ => _.IsValid(giftAidDonorRequest.DonationAmount)).Returns(true);
            this._giftAidDonorServiceMock.Setup(_ => _.SaveDonorDetails(giftAidDonorRequest)).Throws<Exception>();

            var response = this._controller.Post(giftAidDonorRequest) as StatusCodeResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        private GiftAidDonorRequest GetGiftAidDonorRequest()
        {
            return new GiftAidDonorRequest
            {
                DonationAmount = 100m,
                Name = "Joe Bloggs",
                Postcode = "WC2N 5DU"
            };
        }

        private GiftAidDonorResponse GetExpectedGiftDonorResponse()
        {
            return new GiftAidDonorResponse
            {
                DeclarationId = 1,
                DonationAmount = 100m,
                GiftAidAmount = 25m,
                Name = "Joe Bloggs",
                Postcode = "WC2N 5DU"
            };
        }
    }
}
