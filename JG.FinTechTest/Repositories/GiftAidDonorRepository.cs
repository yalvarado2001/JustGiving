using JG.FinTechTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JG.FinTechTest.Repositories
{
    public interface IGiftAidDonorRepository
    {
        int SaveGiftAidDonor(GiftAidDonorRequest donor);
    }
    public class GiftAidDonorRepository : IGiftAidDonorRepository
    {
        private Dictionary<int, GiftAidDonorRequest> _database = new Dictionary<int, GiftAidDonorRequest>();

        public int SaveGiftAidDonor(GiftAidDonorRequest donor)
        {
            int id = this._database.Count + 1;

            this._database.Add(1, donor);

            return id;

        }
    }
}
