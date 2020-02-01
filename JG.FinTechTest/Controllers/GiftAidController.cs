using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JG.FinTechTest.Models;
using JG.FinTechTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace JG.FinTechTest.Controllers
{
    [Route("api/giftaid")]
    [ApiController]
    public class GiftAidController : ControllerBase
    {
        private readonly IGiftAidCalculatorService _giftAidCalculatorService;

        public GiftAidController(IGiftAidCalculatorService giftAidCalculatorService)
        {
            this._giftAidCalculatorService = giftAidCalculatorService;
        }



        [HttpGet]
        public IActionResult Get([FromQuery(Name ="amount")] decimal amount)
        {
            decimal calculatedGiftAid = this._giftAidCalculatorService.Calculate(amount);

            return Ok(new GiftAidResponse
            {
                DonationAmount = amount,
                GiftAidAmount = calculatedGiftAid
            });
        }
    }
}
