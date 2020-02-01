using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JG.FinTechTest.Models
{
    public class GiftAidResponse
    {
        [JsonProperty("donationAmount")]
        public decimal DonationAmount { get; set; }
        
        [JsonProperty("giftAidAmount")]
        public decimal GiftAidAmount { get; set; }
    }
}
