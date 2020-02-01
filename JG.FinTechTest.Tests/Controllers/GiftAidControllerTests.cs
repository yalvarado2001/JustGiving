using JG.FinTechTest.Controllers;
using JG.FinTechTest.Models;
using JG.FinTechTest.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Http;

namespace JG.FinTechTest.Tests.Controllers
{
    [TestFixture]
    public class GiftAidControllerTests
    {
        Mock<IGiftAidCalculatorService> _giftAidCalculatorServiceMock;
        GiftAidController _controller;

        [SetUp]
        public void Seup()
        {
             this._giftAidCalculatorServiceMock = new Mock<IGiftAidCalculatorService>();

            this._controller = new GiftAidController(this._giftAidCalculatorServiceMock.Object);
        }

        [Test]
        public void ShouldReturnDonationAmountAndCalculatedGiftAid()
        {
            decimal donatedAmount = 100m;
            decimal calculatedGiftAid = 25m;

            this._giftAidCalculatorServiceMock.Setup(_ => _.Calculate(donatedAmount)).Returns(calculatedGiftAid);

            var response = this._controller.Get(donatedAmount) as OkObjectResult;
            var giftAidResponse = response.Value as GiftAidResponse;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(giftAidResponse.DonationAmount, Is.EqualTo(donatedAmount));
            Assert.That(giftAidResponse.GiftAidAmount, Is.EqualTo(calculatedGiftAid));
        }
    }
}
