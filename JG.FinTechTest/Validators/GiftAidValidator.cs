using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JG.FinTechTest.Validators
{
    public class GiftAidValidator : IValidator<decimal>
    {
        private readonly List<IValidatonRule<decimal>> _validatonRules;

        public GiftAidValidator(List<IValidatonRule<decimal>> validatonRules)
        {
            this._validatonRules = validatonRules;
            this.Errors = new List<ValidationError>();
        }

        public List<ValidationError> Errors { get; private set; }

        public bool IsValid(decimal arg)
        {
            var failedValidationRules = this._validatonRules.Where(rule => rule.Validates(arg) == false);

            if(failedValidationRules.Any())
            {
                foreach (var rule in failedValidationRules)
                {
                    this.Errors.Add(rule.Error);
                }

                return false;
            }

            return true;
        }
    }
}
