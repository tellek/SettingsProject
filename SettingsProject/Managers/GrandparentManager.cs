using Microsoft.Extensions.Caching.Memory;
using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using SettingsProject.Managers.Interfaces;
using SettingsResources.DatabaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SettingsProject.Managers
{
    public class GrandparentManager : IGrandparentManager
    {
        private readonly IMemoryCache _cache;
        private readonly IRepository<Grandparent> gpRepo;

        public GrandparentManager(IMemoryCache memoryCache, IRepository<Grandparent> gpRepo)
        {
            this.gpRepo = gpRepo;
            _cache = memoryCache;
        }

        public async Task<(int, long)> CreateGrandparentAsync(ProcessData pData, SettingsOnly payload)
        {
            long createdRecordId = await gpRepo.CreateAsync(pData, payload);
            if (createdRecordId <= 0) return (400, 0);

            _cache.Remove($"GrandparentList_{pData.AccountId}");
            return (201, createdRecordId);
        }

        public async Task<int> DeleteGrandparentAsync(ProcessData pData)
        {
            int deletedAmount = await gpRepo.DeleteAsync(pData);
            if (deletedAmount <= 0) return 404;

            _cache.Remove($"GrandparentList_{pData.AccountId}");
            _cache.Remove($"Grandparent_{pData.Gpid}");
            return 200;
        }

        public async Task<(int, Grandparent)> GetGrandparentAsync(ProcessData pData)
        {
            Grandparent cachedValue;
            string akey = $"GrandparentList_{pData.AccountId}";
            string gpkey = $"Grandparent_{pData.Gpid}";

            // Check if all Grandparent settings for this account are already in cache. Find result from there if so.

            cachedValue = await _cache.GetOrCreateAsync<Grandparent>(gpkey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(10);

                IEnumerable<Grandparent> cachedGpList;
                if (_cache.TryGetValue(akey, out cachedGpList))
                {
                    var temp = cachedGpList.FirstOrDefault(c => c.Id == pData.Gpid);
                    return Task.FromResult(temp);
                }
                else return gpRepo.GetSingleAsync(pData);
                // May need to work on async since await may be needed here.
            });


            if (cachedValue == null) return (404, null);
            return (200, cachedValue);
        }

        public async Task<(int, IEnumerable<Grandparent>)> GetGrandparentsAsync(ProcessData pData)
        {
            string key = $"GrandparentList_{pData.AccountId}";
            IEnumerable<Grandparent> cachedValue = await _cache.GetOrCreateAsync(key, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                var result = gpRepo.GetManyAsync(pData);
                return result;
            });

            return (200, cachedValue);
        }

        public async Task<int> UpdateGrandparentAsync(ProcessData pData, SettingsOnly payload)
        {
            long updatedRecordId = await gpRepo.UpdateAsync(pData, payload);
            if (updatedRecordId <= 0) return 404;

            _cache.Remove($"GrandparentList_{pData.AccountId}");
            _cache.Remove($"Grandparent_{pData.Gpid}");
            return 204;
        }
    }
}
