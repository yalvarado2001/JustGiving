using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JG.FinTechTest.Validators
{
    public interface IValidator<T>
    {
        List<ValidationError> Errors { get; }

        bool IsValid(T arg);
    }
}
