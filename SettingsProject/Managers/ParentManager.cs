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

namespace SettingsProject.Managers
{
    public class ParentManager<T> : IManager<Parent>
    {
        private readonly IMemoryCache _cache;
        private readonly IDbRepository<Parent> _db;
        private readonly int _cacheTime; // Minutes

        public ParentManager(IMemoryCache memoryCache, IDbRepository<Parent> dbRepository)
        {
            _db = dbRepository;
            _cache = memoryCache;
            _cacheTime = 10; //TODO: Get this from config.
        }

        public async Task<(int, long)> CreateSettingAsync(ProcessData pData, SettingsOnly payload)
        {
            long createdRecordId = await _db.CreateAsync(pData, payload, Resource.Parent);

            // Assume this retuened zero because a data issue prevented it from being successful.
            if (createdRecordId <= 0) return (400, 0);

            // Remove cached items this change will affect.
            _cache.Remove($"ParentList_{pData.AccountId}");

            Log.Debug($"Parent setting {createdRecordId} created for Grandparent {pData.Gpid}.");
            return (201, createdRecordId);
        }

        public async Task<int> DeleteSettingAsync(ProcessData pData)
        {
            int deletedAmount = await _db.DeleteAsync(pData, Resource.Parent);

            // Assume this returned zero because no rows were affected.
            if (deletedAmount <= 0) return 404;

            // Remove cached items this change will affect.
            _cache.Remove($"ParentList_{pData.AccountId}");
            _cache.Remove($"Parent_{pData.Pid}");

            Log.Debug($"Deleted Parent setting {pData.Pid} from Grandparent {pData.Gpid}.");
            return 200;
        }

        public async Task<(int, Parent)> GetSettingAsync(ProcessData pData)
        {
            Parent cachedValue;
            string accountkey = $"ParentList_{pData.AccountId}";
            string recordKey = $"Parent_{pData.Pid}";

            cachedValue = await _cache.GetOrCreateAsync<Parent>(recordKey, entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_cacheTime);

                IEnumerable<Parent> cachedList;
                    // Check if cached Parent settings list for this account already exists and take it from there instead.
                    if (_cache.TryGetValue(accountkey, out cachedList))
                {
                    var temp = cachedList.FirstOrDefault(c => c.Id == pData.Pid);
                    return Task.FromResult(temp);
                }

                // May need to work on async since await may be needed here. Not sure how though.
                else return _db.GetSingleAsync(pData, Resource.Parent);
            });

            // Assume null means update failed due to bad data.
            if (cachedValue == null) return (404, null);

            Log.Debug($"Retrieved Parent setting {pData.Pid} from Grandparent {pData.Gpid}.");
            return (200, cachedValue);
        }

        public async Task<(int, IEnumerable<Parent>)> GetSettingsAsync(ProcessData pData)
        {
            string key = $"ParentList_{pData.AccountId}";

            IEnumerable<Parent> cachedValue = await _cache.GetOrCreateAsync(key, entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_cacheTime);
                var result = _db.GetManyAsync(pData, Resource.Parent);
                return result;
            });

            Log.Debug($"Retrieved Parent list from Grandparent {pData.Gpid}.");
            // No need to return 404 as an empty Ienumerable will give the '[]' that we want.
            return (200, cachedValue);
        }

        public async Task<int> UpdateSettingAsync(ProcessData pData, SettingsOnly payload)
        {
            long updatedRecordId = await _db.UpdateAsync(pData, payload, Resource.Parent);
            if (updatedRecordId <= 0) return 404;

            // Remove cached items this change will affect.
            _cache.Remove($"ParentList_{pData.AccountId}");
            _cache.Remove($"Parent_{pData.Pid}");

            Log.Debug($"Updated Parent setting {pData.Pid} in Grandparent {pData.Gpid}.");
            return 204;
        }

    }
}
