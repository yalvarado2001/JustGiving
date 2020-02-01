using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JG.FinTechTest.Validators
{
    public interface IValidationRule<T>
    {
        ValidationError Error { get; }

        bool Validates(T arg);
    }
}
