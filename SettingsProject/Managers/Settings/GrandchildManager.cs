using Microsoft.Extensions.Caching.Memory;
using Serilog;
using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using SettingsResources.DatabaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SettingsProject.Managers.Settings
{
    public class GrandchildManager<T> : IManager<Grandchild>
    {
        private readonly IMemoryCache _cache;
        private readonly IDbRepository<Grandchild> _db;
        private readonly int _cacheTime; // Minutes

        public GrandchildManager(IMemoryCache memoryCache, IDbRepository<Grandchild> dbRepository)
        {
            _db = dbRepository;
            _cache = memoryCache;
            _cacheTime = 10; //TODO: Get this from config.
        }

        public async Task<(int, long)> CreateSettingAsync(ProcessData pData, SettingsOnly payload)
        {
            long createdRecordId = await _db.CreateAsync(pData, payload, Resource.Grandchild);

            // Assume this retuened zero because a data issue prevented it from being successful.
            if (createdRecordId <= 0) return (400, 0);

            // Remove cached items this change will affect.
            _cache.Remove($"GrandchildList_{pData.AccountId}");

            Log.Debug($"Grandchild setting {createdRecordId} created for Child {pData.Cid}.");
            return (201, createdRecordId);
        }

        public async Task<int> DeleteSettingAsync(ProcessData pData)
        {
            int deletedAmount = await _db.DeleteAsync(pData, Resource.Grandchild);

            // Assume this returned zero because no rows were affected.
            if (deletedAmount <= 0) return 404;

            // Remove cached items this change will affect.
            _cache.Remove($"GrandchildList_{pData.AccountId}");
            _cache.Remove($"Grandchild_{pData.Gcid}");

            Log.Debug($"Deleted Grandchild setting {pData.Gcid} from Child {pData.Cid}.");
            return 200;
        }

        public async Task<(int, Grandchild)> GetSettingAsync(ProcessData pData)
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
                else return _db.GetSingleAsync(pData, Resource.Grandchild);
            });

            // Assume null means update failed due to bad data.
            if (cachedValue == null) return (404, null);

            Log.Debug($"Retrieved Grandchild setting {pData.Gcid} from Child {pData.Cid}.");
            return (200, cachedValue);
        }

        public async Task<(int, IEnumerable<Grandchild>)> GetSettingsAsync(ProcessData pData)
        {
            string key = $"GrandchildList_{pData.AccountId}";

            IEnumerable<Grandchild> cachedValue = await _cache.GetOrCreateAsync(key, entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_cacheTime);
                var result = _db.GetManyAsync(pData, Resource.Grandchild);
                return result;
            });

            Log.Debug($"Retrieved Grandchild list from Child {pData.Cid}.");
            // No need to return 404 as an empty Ienumerable will give the '[]' that we want.
            return (200, cachedValue);
        }

        public async Task<int> UpdateSettingAsync(ProcessData pData, SettingsOnly payload)
        {
            long updatedRecordId = await _db.UpdateAsync(pData, payload, Resource.Grandchild);
            if (updatedRecordId <= 0) return 404;

            // Remove cached items this change will affect.
            _cache.Remove($"GrandchildList_{pData.AccountId}");
            _cache.Remove($"Grandchild_{pData.Gcid}");

            Log.Debug($"Updated Grandchild setting {pData.Gcid} in Child {pData.Cid}.");
            return 204;
        }

    }
}
