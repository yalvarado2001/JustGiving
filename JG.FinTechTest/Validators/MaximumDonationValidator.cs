using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JG.FinTechTest.Validators
{
    public class MaximumDonationValidator : IValidationRule<decimal>
    {
        public ValidationError Error { get; private set; }

        public bool Validates(decimal arg)
        {
            if (arg > 100000m)
            {
                this.Error = new ValidationError(2, "Maximum donation amount is £100,000.00");
                return false;
            }

            return true;
        }
    }
}
