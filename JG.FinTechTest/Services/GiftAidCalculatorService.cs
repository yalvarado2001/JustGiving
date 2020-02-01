using JG.FinTechTest.Validators;
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
        private static readonly decimal BasicTaxRate = 20m;

        private readonly decimal _taxRate;
        private readonly decimal _giftAidRate;

        public GiftAidCalculatorService() :this(BasicTaxRate)
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
