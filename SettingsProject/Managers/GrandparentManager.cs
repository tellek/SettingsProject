using Microsoft.Extensions.Caching.Memory;
using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using SettingsResources.DatabaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SettingsProject.Managers
{
    //TODO: See about making this whole class a generic.
    public class GrandparentManager<T> : IManager<Grandparent>
    {
        private readonly IMemoryCache _cache;
        private readonly IRepository<Grandparent> _repo;
        private readonly int _cacheTime; // Minutes

        public GrandparentManager(IMemoryCache memoryCache, IRepository<Grandparent> Repository)
        {
            _repo = Repository;
            _cache = memoryCache;
            _cacheTime = 10; //TODO: Get this from config.
        }

        public async Task<(int, long)> CreateSettingAsync(ProcessData pData, SettingsOnly payload)
        {
            long createdRecordId = await _repo.CreateAsync(pData, payload);

            // Assume this retuened zero because a data issue prevented it from being successful.
            if (createdRecordId <= 0) return (400, 0);

            // Remove cached items this change will affect.
            _cache.Remove($"GrandparentList_{pData.AccountId}");

            return (201, createdRecordId);
        }

        public async Task<int> DeleteSettingAsync(ProcessData pData)
        {
            int deletedAmount = await _repo.DeleteAsync(pData);

            // Assume this returned zero because no rows were affected.
            if (deletedAmount <= 0) return 404;

            // Remove cached items this change will affect.
            _cache.Remove($"GrandparentList_{pData.AccountId}");
            _cache.Remove($"Grandparent_{pData.Gpid}");

            return 200;
        }

        public async Task<(int, Grandparent)> GetSettingAsync(ProcessData pData)
        {
            Grandparent cachedValue;
            string accountkey = $"GrandparentList_{pData.AccountId}";
            string recordKey = $"Grandparent_{pData.Gpid}";

            cachedValue = await _cache.GetOrCreateAsync<Grandparent>(recordKey, entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_cacheTime);

                IEnumerable<Grandparent> cachedList;
                // Check if cached Grandparent settings list for this account already exists and take it from there instead.
                if (_cache.TryGetValue(accountkey, out cachedList))
                {
                    var temp = cachedList.FirstOrDefault(c => c.Id == pData.Gpid);
                    return Task.FromResult(temp);
                }

                // May need to work on async since await may be needed here. Not sure how though.
                else return _repo.GetSingleAsync(pData);
            });

            // Assume null means update failed due to bad data.
            if (cachedValue == null) return (404, null);

            return (200, cachedValue);
        }

        public async Task<(int, IEnumerable<Grandparent>)> GetSettingsAsync(ProcessData pData)
        {
            string key = $"GrandparentList_{pData.AccountId}";

            IEnumerable<Grandparent> cachedValue = await _cache.GetOrCreateAsync(key, entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_cacheTime);
                var result = _repo.GetManyAsync(pData);
                return result;
            });

            // No need to return 404 as an empty Ienumerable will give the '[]' that we want.
            return (200, cachedValue);
        }

        public async Task<int> UpdateSettingAsync(ProcessData pData, SettingsOnly payload)
        {
            long updatedRecordId = await _repo.UpdateAsync(pData, payload);
            if (updatedRecordId <= 0) return 404;

            // Remove cached items this change will affect.
            _cache.Remove($"GrandparentList_{pData.AccountId}");
            _cache.Remove($"Grandparent_{pData.Gpid}");

            return 204;
        }
    }
}
