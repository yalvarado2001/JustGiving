using JG.FinTechTest.Models;
using JG.FinTechTest.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JG.FinTechTest.Services
{
    public interface IGiftAidDonorService
    {
        GiftAidDonorResponse SaveDonorDetails(GiftAidDonorRequest request);
    }

    public class GiftAidDonorService : IGiftAidDonorService
    {
        private readonly IGiftAidCalculatorService _giftAidCalculatorService;
        private readonly IGiftAidDonorRepository _giftAidDonorRepository;

        public GiftAidDonorService(IGiftAidCalculatorService giftAidCalculatorService, IGiftAidDonorRepository giftAidDonorRepository)
        {
            this._giftAidCalculatorService = giftAidCalculatorService;
            this._giftAidDonorRepository = giftAidDonorRepository;
        }

        public GiftAidDonorResponse SaveDonorDetails(GiftAidDonorRequest request)
        {
            decimal giftAidAmount = this._giftAidCalculatorService.Calculate(request.DonationAmount);

            int declarationId = this._giftAidDonorRepository.SaveGiftAidDonor(request);

            return new GiftAidDonorResponse
            {
                DeclarationId = declarationId,
                DonationAmount = request.DonationAmount,
                GiftAidAmount = giftAidAmount,
                Name = request.Name,
                Postcode = request.Postcode
            };
        }
    }
}
