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
    public class GrandchildManager : IGrandchildManager
    {
        private readonly IMemoryCache _cache;
        private readonly IRepository<Grandchild> _repo;
        private readonly int _cacheTime; // Minutes

        public GrandchildManager(IMemoryCache memoryCache, IRepository<Grandchild> Repository)
        {
            _repo = Repository;
            _cache = memoryCache;
            _cacheTime = 10; //TODO: Get this from config.
        }

        public async Task<(int, long)> CreateGrandchildAsync(ProcessData pData, SettingsOnly payload)
        {
            long createdRecordId = await _repo.CreateAsync(pData, payload);

            // Assume this retuened zero because a data issue prevented it from being successful.
            if (createdRecordId <= 0) return (400, 0);

            // Remove cached items this change will affect.
            _cache.Remove($"GrandchildList_{pData.AccountId}");

            return (201, createdRecordId);
        }

        public async Task<int> DeleteGrandchildAsync(ProcessData pData)
        {
            int deletedAmount = await _repo.DeleteAsync(pData);

            // Assume this returned zero because no rows were affected.
            if (deletedAmount <= 0) return 404;

            // Remove cached items this change will affect.
            _cache.Remove($"GrandchildList_{pData.AccountId}");
            _cache.Remove($"Grandchild_{pData.Gcid}");

            return 200;
        }

        public async Task<(int, Grandchild)> GetGrandchildAsync(ProcessData pData)
        {
            Grandchild cachedValue;
            string accountkey = $"GrandchildList_{pData.AccountId}";
            string recordKey = $"Grandchild_{pData.Gcid}";

            cachedValue = await _cache.GetOrCreateAsync<Grandchild>(recordKey, entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_cacheTime);

                IEnumerable<Grandchild> cachedList;
                // Check if cached Grandchild settings list for this account already exists and take it from there instead.
                if (_cache.TryGetValue(accountkey, out cachedList))
                {
                    var temp = cachedList.FirstOrDefault(c => c.Id == pData.Gcid);
                    return Task.FromResult(temp);
                }

                // May need to work on async since await may be needed here. Not sure how though.
                else return _repo.GetSingleAsync(pData);
            });

            // Assume null means update failed due to bad data.
            if (cachedValue == null) return (404, null);

            return (200, cachedValue);
        }

        public async Task<(int, IEnumerable<Grandchild>)> GetGrandchildsAsync(ProcessData pData)
        {
            string key = $"GrandchildList_{pData.AccountId}";

            IEnumerable<Grandchild> cachedValue = await _cache.GetOrCreateAsync(key, entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_cacheTime);
                var result = _repo.GetManyAsync(pData);
                return result;
            });

            // No need to return 404 as an empty Ienumerable will give the '[]' that we want.
            return (200, cachedValue);
        }

        public async Task<int> UpdateGrandchildAsync(ProcessData pData, SettingsOnly payload)
        {
            long updatedRecordId = await _repo.UpdateAsync(pData, payload);
            if (updatedRecordId <= 0) return 404;

            // Remove cached items this change will affect.
            _cache.Remove($"GrandchildList_{pData.AccountId}");
            _cache.Remove($"Grandchild_{pData.Gcid}");

            return 204;
        }
    }
}
