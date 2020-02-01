using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JG.FinTechTest.Services
{
    public interface IGiftAidCalculatorService
    {
        decimal Calculate(decimal donatedAmount);
    }

    public class GiftAidCalculatorService : IGiftAidCalculatorService
    {
        private const decimal BASIC_TAX_RATE = 20m;
        private readonly decimal _taxRate;
        private readonly decimal _giftAidRate;

        public GiftAidCalculatorService():this(BASIC_TAX_RATE)
        {}

        public GiftAidCalculatorService(decimal taxRate)
        {
            this._taxRate = taxRate;
            this._giftAidRate = this._taxRate / (100m - this._taxRate);
        }
        public decimal Calculate(decimal donatedAmount)
        {
            return donatedAmount * this._giftAidRate;
        }
    }
}
