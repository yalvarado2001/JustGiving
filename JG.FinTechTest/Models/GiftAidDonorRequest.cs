using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JG.FinTechTest.Models
{
    public class GiftAidDonorRequest
    {
        public string Name { get; set; }
        
        [StringLength(8)]
        public string Postcode { get; set; }

        public decimal DonationAmount { get; set; }
    }
}
