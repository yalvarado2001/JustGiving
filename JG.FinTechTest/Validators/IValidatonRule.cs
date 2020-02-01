using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JG.FinTechTest.Validators
{
    public interface IValidatonRule<T>
    {
        ValidationError Error { get; }

        bool Validates(T arg);
    }
}
