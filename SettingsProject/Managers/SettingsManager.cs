using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Serilog;
using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.ApiTransaction.ResponseModels;
using SettingsContracts.DatabaseModels;
using SettingsResources.DatabaseRepositories;

namespace SettingsProject.Managers
{
    //TODO: See about making this whole class a generic.
    public class SettingsManager<T> : ISettingsManager<T>
    {
        private readonly IMemoryCache _cache;
        private readonly IDbRepository<T> _db;
        private readonly int _cacheTime; // Minutes

        public SettingsManager(IMemoryCache memoryCache, IDbRepository<T> dbRepository)
        {
            _db = dbRepository;
            _cache = memoryCache;
            _cacheTime = 10; //TODO: Get this from config.
        }

        public async Task<(int, long)> CreateSettingAsync(ProcessData pData, SettingsOnly payload, Resource resource)
        {
            long createdRecordId = await _db.CreateAsync(pData, payload, resource);

            // Assume this retuened zero because a data issue prevented it from being successful.
            if (createdRecordId <= 0) return (400, 0);

            // Remove cached items this change will affect.
            _cache.Remove($"{resource.ToString()}List_{pData.AccountId}");

            Log.Debug($"{resource.ToString()} setting {createdRecordId} created for account {pData.AccountId}.");
            return (201, createdRecordId);
        }

        public async Task<int> DeleteSettingAsync(ProcessData pData, Resource resource)
        {
            int deletedAmount = await _db.DeleteAsync(pData, resource);

            // Assume this returned zero because no rows were affected.
            if (deletedAmount <= 0) return 404;

            // Remove cached items this change will affect.
            _cache.Remove($"{resource.ToString()}List_{pData.AccountId}");
            _cache.Remove($"{resource.ToString()}_{pData.Gpid}");

            Log.Debug($"Deleted {resource.ToString()} setting {pData.Gpid} from account {pData.AccountId}.");
            return 200;
        }

        public async Task<(int, object)> GetSettingAsync(ProcessData pData, Resource resource)
        {
            var response = new SingleSuccessResponse<T> { Cached = true };
            string accountkey = $"{resource.ToString()}List_{pData.AccountId}";
            string key = $"{resource.ToString()}_{pData.Gpid}";

            var cachedValue = await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_cacheTime);
                response.Cached = false;
                // Could check if cached resource settings list for this account already exists and take it from there instead.
                // May need to work on async since await may be needed here. Not sure how though.
                return await _db.GetSingleAsync(pData, resource);
            });

            // Assume null means update failed due to bad data.
            response.Result = cachedValue;
            if (response.Result == null) return (404, null);

            Log.Debug($"Retrieved {resource.ToString()} setting {pData.Gpid} from account {pData.AccountId}.");
            return (200, response);
        }

        public async Task<(int, object)> GetSettingsAsync(ProcessData pData, Resource resource)
        {
            var response = new PagedSuccessResponse<T> { Cached = true };
            string key = $"{resource.ToString()}List_{pData.AccountId}";

            var cachedValue = await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_cacheTime);
                response.Cached = false;
                return await _db.GetManyAsync(pData, resource);
            });

            // No need to return 404 as an empty Ienumerable will give the '[]' that we want.

            Log.Debug($"Retrieved {resource.ToString()} list from account {pData.AccountId}.");
            response.Results = cachedValue;
            return (200, response);
        }

        public async Task<int> UpdateSettingAsync(ProcessData pData, SettingsOnly payload, Resource resource)
        {
            long updatedRecordId = await _db.UpdateAsync(pData, payload, Resource.Grandparent);
            if (updatedRecordId <= 0) return 404;

            // Remove cached items this change will affect.
            _cache.Remove($"{resource.ToString()}List_{pData.AccountId}");
            _cache.Remove($"{resource.ToString()}_{pData.Gpid}");

            Log.Debug($"Updated {resource.ToString()} setting {pData.Gpid} in account {pData.AccountId}.");
            return 204;
        }
    }
}
