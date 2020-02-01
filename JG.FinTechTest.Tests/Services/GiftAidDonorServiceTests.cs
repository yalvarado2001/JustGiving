using JG.FinTechTest.Models;
using JG.FinTechTest.Repositories;
using JG.FinTechTest.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JG.FinTechTest.Tests.Services
{
    [TestFixture]
    class GiftAidDonorServiceTests
    {
        Mock<IGiftAidCalculatorService> _giftAidCalculatorServiceMock;
        Mock<IGiftAidDonorRepository> _giftAidDonorRepositoryMock;
        IGiftAidDonorService _giftAidDonorService;

        [SetUp]
        public void Setup()
        {
            this._giftAidCalculatorServiceMock = new Mock<IGiftAidCalculatorService>();
            this._giftAidDonorRepositoryMock = new Mock<IGiftAidDonorRepository>();

            this._giftAidDonorService = new GiftAidDonorService(this._giftAidCalculatorServiceMock.Object, this._giftAidDonorRepositoryMock.Object);
        }

        [Test]
        public void ShouldReturnAGiftAidDonorResponseWithAllTheDetails()
        {
            var giftAidDonorRequest = new GiftAidDonorRequest
            {
                DonationAmount = 100m,
                Name = "Joe Bloggs",
                Postcode = "WC2N 5DU"
            };
            var expectedGiftAid = 25m;
            int expectedId = 1;

            this._giftAidCalculatorServiceMock.Setup(_ => _.Calculate(giftAidDonorRequest.DonationAmount)).Returns(expectedGiftAid);
            this._giftAidDonorRepositoryMock.Setup(_ => _.SaveGiftAidDonor(giftAidDonorRequest)).Returns(expectedId);

            GiftAidDonorResponse response = this._giftAidDonorService.SaveDonorDetails(giftAidDonorRequest);
                
            Assert.That(response.DeclarationId, Is.EqualTo(expectedId));
            Assert.That(response.GiftAidAmount, Is.EqualTo(expectedGiftAid));
            Assert.That(response.DonationAmount, Is.EqualTo(giftAidDonorRequest.DonationAmount));
            Assert.That(response.Name, Is.EqualTo(giftAidDonorRequest.Name));
            Assert.That(response.Postcode, Is.EqualTo(giftAidDonorRequest.Postcode));

        }
    }
}
