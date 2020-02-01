using JG.FinTechTest.Models;
using JG.FinTechTest.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JG.FinTechTest.Tests.Repositories
{
    [TestFixture]
    class GiftAidDonorRepositoryTests
    {
        [Test]
        public void ShouldReturnUniqueId()
        {
            var giftAidDonorRepository = new GiftAidDonorRepository();

            var giftAidDonorRequest = new GiftAidDonorRequest
            {
                DonationAmount = 100m,
                Name = "Joe Bloggs",
                Postcode = "WC2N 5DU"
            };

            int id = giftAidDonorRepository.SaveGiftAidDonor(giftAidDonorRequest);

            Assert.That(id, Is.GreaterThan(0));
        }
    }
}
