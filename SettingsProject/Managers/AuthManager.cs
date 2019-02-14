using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.ApiTransaction.ResponseModels;
using SettingsContracts.DatabaseModels;
using SettingsResources.DatabaseRepositories;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SettingsProject.Managers
{
    public class AuthManager : IAuthManager
    {
        private readonly IMemoryCache _cache;
        private readonly IDbRepository<UserAuth> _db;

        public AuthManager(IMemoryCache cache, IDbRepository<UserAuth> dbRepository)
        {
            _cache = cache;
            _db = dbRepository;
        }

        public async Task<(int, object)> DoAuthenticationAsync(AuthProperties payload)
        {
            return await _cache.GetOrCreateAsync<(int, object)>($"{payload.Username}{payload.Password}", async entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(50);

                // Validate username / password
                Permissions userData = await _db.ChallengeCredentialsAsync(payload.Username, payload.Password);
                if (userData == null)
                {
                    var error = new { Error = "Username or password authentication failure." };
                    return (401, error);
                }

                // Generate a new token
                string token = Convert.ToBase64String(Encoding.ASCII.GetBytes(payload.Username + Guid.NewGuid().ToString()));

                // Add new memoryCache entry -> Key: token, Value: access arrays
                SetMemoryCacheEntry(token, userData);

                return (200, new { Token = token });
            });
        }

        private void SetMemoryCacheEntry(string token, Permissions access)
        {
            if (_cache.TryGetValue(token, out Permissions cachedItem))
            {
                _cache.Remove(token);
            }
            var options = new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.High,
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(60)
            };
            _cache.Set(token, access, options);
        }
    }
}
