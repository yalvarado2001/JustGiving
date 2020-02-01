using JG.FinTechTest.Services;
using JG.FinTechTest.Validators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JG.FinTechTest.Tests.Services
{
    [TestFixture]
    class GiftAidCalculatorServiceTests
    {
        GiftAidValidator _validator = new GiftAidValidator(new List<IValidatonRule<decimal>>());
        IGiftAidCalculatorService _giftAidCalculator;

        [SetUp]
        public void Setup()
        {
            this._giftAidCalculator = new GiftAidCalculatorService();
        }


        [TestCase(100, 25)]
        [TestCase(111, 27.75)]
        public void ShouldCalculateTheCorrectGiftAid(decimal donatedAmount, decimal expectedGiftAid)
        {
            decimal calculatedGiftAid = this._giftAidCalculator.Calculate(donatedAmount);

            Assert.That(calculatedGiftAid, Is.EqualTo(expectedGiftAid));
        }
    }
}
