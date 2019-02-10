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
    public class ChildManager<T> : IManager<Child>
    {
        private readonly IMemoryCache _cache;
        private readonly IDbRepository<Child> _db;
        private readonly int _cacheTime; // Minutes

        public ChildManager(IMemoryCache memoryCache, IDbRepository<Child> dbRepository)
        {
            _db = dbRepository;
            _cache = memoryCache;
            _cacheTime = 10; //TODO: Get this from config.
        }

        public async Task<(int, long)> CreateSettingAsync(ProcessData pData, SettingsOnly payload)
        {
            long createdRecordId = await _db.CreateAsync(pData, payload, Resource.Child);

            // Assume this retuened zero because a data issue prevented it from being successful.
            if (createdRecordId <= 0) return (400, 0);

            // Remove cached items this change will affect.
            _cache.Remove($"ChildList_{pData.AccountId}");

            return (201, createdRecordId);
        }

        public async Task<int> DeleteSettingAsync(ProcessData pData)
        {
            int deletedAmount = await _db.DeleteAsync(pData, Resource.Child);

            // Assume this returned zero because no rows were affected.
            if (deletedAmount <= 0) return 404;

            // Remove cached items this change will affect.
            _cache.Remove($"ChildList_{pData.AccountId}");
            _cache.Remove($"Child_{pData.Cid}");

            return 200;
        }

        public async Task<(int, Child)> GetSettingAsync(ProcessData pData)
        {
            Child cachedValue;
            string accountkey = $"ChildList_{pData.AccountId}";
            string recordKey = $"Child_{pData.Cid}";

            cachedValue = await _cache.GetOrCreateAsync<Child>(recordKey, entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_cacheTime);

                IEnumerable<Child> cachedList;
                // Check if cached Child settings list for this account already exists and take it from there instead.
                if (_cache.TryGetValue(accountkey, out cachedList))
                {
                    var temp = cachedList.FirstOrDefault(c => c.Id == pData.Cid);
                    return Task.FromResult(temp);
                }

                // May need to work on async since await may be needed here. Not sure how though.
                else return _db.GetSingleAsync(pData, Resource.Child);
            });

            // Assume null means update failed due to bad data.
            if (cachedValue == null) return (404, null);

            return (200, cachedValue);
        }

        public async Task<(int, IEnumerable<Child>)> GetSettingsAsync(ProcessData pData)
        {
            string key = $"ChildList_{pData.AccountId}";

            IEnumerable<Child> cachedValue = await _cache.GetOrCreateAsync(key, entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_cacheTime);
                var result = _db.GetManyAsync(pData, Resource.Child);
                return result;
            });

            // No need to return 404 as an empty Ienumerable will give the '[]' that we want.
            return (200, cachedValue);
        }

        public async Task<int> UpdateSettingAsync(ProcessData pData, SettingsOnly payload)
        {
            long updatedRecordId = await _db.UpdateAsync(pData, payload, Resource.Child);
            if (updatedRecordId <= 0) return 404;

            // Remove cached items this change will affect.
            _cache.Remove($"ChildList_{pData.AccountId}");
            _cache.Remove($"Child_{pData.Cid}");

            return 204;
        }

    }
}
