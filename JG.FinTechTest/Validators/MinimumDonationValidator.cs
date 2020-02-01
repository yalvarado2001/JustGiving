using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JG.FinTechTest.Validators
{
    public class MinimumDonationValidator : IValidatonRule<decimal>
    {
        public ValidationError Error { get; private set; }

        public bool Validates(decimal arg)
        {
            if(arg < 2m)
            {
                this.Error = new ValidationError(1, "Minimum donation amount is £2.00");
                return false;
            }

            return true;
        }
    }
}
