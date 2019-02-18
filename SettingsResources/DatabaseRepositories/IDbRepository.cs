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
        Task<long> CreateAsync(ProcessData pData, SettingsOnly settings, bool byName = false);
        Task<int> UpdateAsync(ProcessData pData, SettingsOnly settings, bool byName = false);
        Task<int> DeleteAsync(ProcessData pData, bool byName = false);
        Task<T> GetSingleAsync(ProcessData pData, bool byName = false);
        Task<IEnumerable<T>> GetManyAsync(ProcessData pData, bool byName = false);

        // Non-generic
        Task<Permissions> ChallengeCredentialsAsync(string username, string password);
        Task<Hierarchy> GetRequestHierarchyAsync(ProcessData pData, bool byName = false);
    }
}
