using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.ApiTransaction.ResponseModels;
using SettingsResources.DatabaseRepositories;

namespace SettingsProject.Managers
{
    //TODO: See about making this whole class a generic.
    public class SettingsManager<T> : ISettingsManager<T>
    {
        private readonly IMemoryCache _cache;
        private readonly IDbRepository<T> _db;
        private readonly IAuthManager _authorization;
        private readonly int _cacheTime; // Minutes

        public SettingsManager(IMemoryCache memoryCache, IDbRepository<T> dbRepository, IAuthManager authManager)
        {
            _db = dbRepository;
            _cache = memoryCache;
            _authorization = authManager;
            _cacheTime = 10; //TODO: Get this from config.
        }

        public async Task<(int, object)> CreateSettingAsync(ProcessData pData, SettingsOnly payload)
        {
            long createdRecordId = await _db.CreateAsync(pData, payload);

            // Assume this retuened zero because a data issue prevented it from being successful.
            if (createdRecordId <= 0) return (400, new FailureResponse("na", "Failed to create resource!"));

            // Remove cached items this change will affect.
            _cache.Remove($"{pData.Resource.ToString()}List_{pData.AccountId}");

            Log.Debug($"{pData.Resource.ToString()} setting {createdRecordId} created for account {pData.AccountId}.");
            return (201, new CreatedResponse(createdRecordId, ""));
        }

        public async Task<int> DeleteSettingAsync(ProcessData pData)
        {
            int deletedAmount = await _db.DeleteAsync(pData);

            // Assume this returned zero because no rows were affected.
            if (deletedAmount <= 0) return 404;

            // Remove cached items this change will affect.
            _cache.Remove($"{pData.Resource.ToString()}List_{pData.AccountId}");
            _cache.Remove($"{pData.Resource.ToString()}_{pData.Gpid}");

            Log.Debug($"Deleted {pData.Resource.ToString()} setting {pData.Gpid} from account {pData.AccountId}.");
            return 204;
        }

        public async Task<(int, object)> GetSettingAsync(ProcessData pData)
        {
            var response = new SingleSuccessResponse<T> { Cached = true };
            string accountkey = $"{pData.Resource.ToString()}List_{pData.AccountId}";
            string key = $"{pData.Resource.ToString()}_{pData.Gpid}";

            var cachedValue = await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_cacheTime);
                response.Cached = false;
                // Could check if cached resource settings list for this account already exists and take it from there instead.
                // May need to work on async since await may be needed here. Not sure how though.
                return await _db.GetSingleAsync(pData);
            });

            // Assume null means update failed due to bad data.
            response.Result = cachedValue;
            if (response.Result == null) return (404, null);

            Log.Debug($"Retrieved {pData.Resource.ToString()} setting {pData.Gpid} from account {pData.AccountId}.");
            return (200, response);
        }

        public async Task<(int, object)> GetSettingsAsync(ProcessData pData)
        {
            var response = new PagedSuccessResponse<T> { Cached = true };
            string key = $"{pData.Resource.ToString()}List_{pData.AccountId}";

            var cachedValue = await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_cacheTime);
                response.Cached = false;
                return await _db.GetManyAsync(pData);
            });

            Log.Debug($"Retrieved {pData.Resource.ToString()} list from account {pData.AccountId}.");
            response.Results = cachedValue;
            return (200, response);
        }

        public async Task<int> UpdateSettingAsync(ProcessData pData, SettingsOnly payload)
        {
            long updatedRecordId = await _db.UpdateAsync(pData, payload);
            if (updatedRecordId <= 0) return 404;

            // Remove cached items this change will affect.
            _cache.Remove($"{pData.Resource.ToString()}List_{pData.AccountId}");
            _cache.Remove($"{pData.Resource.ToString()}_{pData.Gpid}");

            Log.Debug($"Updated {pData.Resource.ToString()} setting {pData.Gpid} in account {pData.AccountId}.");
            return 204;
        }
    }
}
