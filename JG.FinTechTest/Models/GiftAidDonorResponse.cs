using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JG.FinTechTest.Models
{
    public class GiftAidDonorResponse
    {
        [JsonProperty("declarationId")]
        public int DeclarationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("donationAmount")]
        public decimal DonationAmount { get; set; }

        [JsonProperty("giftAidAmount")]
        public decimal GiftAidAmount { get; set; }
    }
}
