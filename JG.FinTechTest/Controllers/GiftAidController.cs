using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JG.FinTechTest.Models;
using JG.FinTechTest.Services;
using JG.FinTechTest.Validators;
using Microsoft.AspNetCore.Mvc;

namespace JG.FinTechTest.Controllers
{
    [Route("api/giftaid")]
    [ApiController]
    public class GiftAidController : ControllerBase
    {
        private readonly IGiftAidCalculatorService _giftAidCalculatorService;
        private readonly IValidator<decimal> _giftAidValidator;
        private readonly IGiftAidDonorService _giftAidDonorService;

        public GiftAidController(IGiftAidCalculatorService giftAidCalculatorService, IValidator<decimal> giftAidValidator, IGiftAidDonorService giftAidDonorService)
        {
            this._giftAidCalculatorService = giftAidCalculatorService;
            this._giftAidValidator = giftAidValidator;
            this._giftAidDonorService = giftAidDonorService;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Get([FromQuery(Name ="amount")] decimal amount)
        {
            return this.ExecuteAndHandleErrors(() =>
           {
               if (this._giftAidValidator.IsValid(amount))
                   return Ok(this.GetGiftAidResponse(amount));

               return BadRequest(this._giftAidValidator.Errors);
           });

        }

        [Route("donor")]
        [HttpPost]
        public IActionResult Post([FromBody] GiftAidDonorRequest request)
        {
            return this.ExecuteAndHandleErrors(() =>
            {
                if (ModelState.IsValid == false)
                    return BadRequest(this.GetErrorsFromModelState());

                if (this._giftAidValidator.IsValid(request.DonationAmount))
                    return Ok(this._giftAidDonorService.SaveDonorDetails(request));

                return BadRequest(this._giftAidValidator.Errors);

            });
        }


        private GiftAidResponse GetGiftAidResponse(decimal amount)
        {
            decimal calculatedGiftAid = this._giftAidCalculatorService.Calculate(amount);

            return new GiftAidResponse
            {
                DonationAmount = amount,
                GiftAidAmount = calculatedGiftAid
            };
        }

        private List<ValidationError> GetErrorsFromModelState()
        {
            var modelStateErrors = ModelState.Values.SelectMany(key => key.Errors);

            return new List<ValidationError>(modelStateErrors.Select(error => new ValidationError(3, error.ErrorMessage)));
        }

        private IActionResult ExecuteAndHandleErrors(Func<IActionResult> action)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                // The exception should be logged here
                return StatusCode(500);
            }
        }
    }
}
