using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SettingsResources.DatabaseRepositories
{
    public interface IDbRepository<T>
    {
        // Generic
        Task<long> CreateAsync(ProcessData pData, SettingsOnly settings, Resource resource, bool byName = false);
        Task<int> UpdateAsync(ProcessData pData, SettingsOnly settings, Resource resource, bool byName = false);
        Task<int> DeleteAsync(ProcessData pData, Resource resource, bool byName = false);
        Task<T> GetSingleAsync(ProcessData pData, Resource resource, bool byName = false);
        Task<IEnumerable<T>> GetManyAsync(ProcessData pData, Resource resource, bool byName = false);

        // Non-generic
        Task<Permissions> ChallengeCredentialsAsync(string username, string password);
    }
}
