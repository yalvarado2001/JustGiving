using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JG.FinTechTest.Validators
{
    public class ValidationError
    {
        [JsonProperty("errorCode")]
        public int ErrorCode { get; }

        [JsonProperty("errorDescription")]
        public string ErrorDescription { get; }

        public ValidationError(int errorCode, string errorDescription)
        {
            this.ErrorCode = errorCode;
            this.ErrorDescription = errorDescription;
        }
    }
}
