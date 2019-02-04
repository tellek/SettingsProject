using Microsoft.Extensions.Caching.Memory;
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

        public async Task<(int, long)> CreateGrandparentAsync(int accountId, Grandparent payload)
        {
            long createdRecordId = await gpRepo.CreateAsync(accountId, payload);
            if (createdRecordId <= 0) return (400, 0);

            _cache.Remove($"GrandparentList_{accountId}");
            return (201, createdRecordId);
        }

        public async Task<int> DeleteGrandparentAsync(int accountId, long gpid)
        {
            int deletedAmount = await gpRepo.DeleteAsync(accountId, gpid);
            if (deletedAmount <= 0) return 404;

            _cache.Remove($"GrandparentList_{accountId}");
            _cache.Remove($"Grandparent_{gpid}");
            return 200;
        }

        public async Task<(int, Grandparent)> GetGrandparentAsync(int accountId, long gpid)
        {
            Grandparent cachedValue;
            string akey = $"GrandparentList_{accountId}";
            string gpkey = $"Grandparent_{gpid}";

            // Check if all Grandparent settings for this account are already in cache. Find result from there if so.

            cachedValue = await _cache.GetOrCreateAsync<Grandparent>(gpkey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(10);

                IEnumerable<Grandparent> cachedGpList;
                if (_cache.TryGetValue(akey, out cachedGpList))
                {
                    var temp = cachedGpList.FirstOrDefault(c => c.Id == gpid);
                    return Task.FromResult(temp);
                }
                else return gpRepo.GetSingleAsync(accountId, gpid);
                // May need to work on async since await may be needed here.
            });


            if (cachedValue == null) return (404, null);
            return (200, cachedValue);
        }

        public async Task<(int, IEnumerable<Grandparent>)> GetGrandparentsAsync(int accountId)
        {
            string key = $"GrandparentList_{accountId}";
            IEnumerable<Grandparent> cachedValue = await _cache.GetOrCreateAsync(key, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                var result = gpRepo.GetManyAsync(accountId, 0);
                return result;
            });

            return (200, cachedValue);
        }

        public async Task<int> UpdateGrandparentAsync(int accountId, long gpid, Grandparent payload)
        {
            long updatedRecordId = await gpRepo.UpdateAsync(accountId, gpid, payload);
            if (updatedRecordId <= 0) return 404;

            _cache.Remove($"GrandparentList_{accountId}");
            _cache.Remove($"Grandparent_{gpid}");
            return 204;
        }
    }
}
