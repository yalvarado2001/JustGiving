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

        public GiftAidController(IGiftAidCalculatorService giftAidCalculatorService, IValidator<decimal> giftAidValidator)
        {
            this._giftAidCalculatorService = giftAidCalculatorService;
            this._giftAidValidator = giftAidValidator;
        }


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

        private GiftAidResponse GetGiftAidResponse(decimal amount)
        {
            decimal calculatedGiftAid = this._giftAidCalculatorService.Calculate(amount);

            return new GiftAidResponse
            {
                DonationAmount = amount,
                GiftAidAmount = calculatedGiftAid
            };
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
