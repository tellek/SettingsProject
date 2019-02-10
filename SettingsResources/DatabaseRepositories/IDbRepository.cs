using SettingsContracts;
using SettingsContracts.ApiTransaction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SettingsResources.DatabaseRepositories
{
    public interface IDbRepository
    {
        Task<long> CreateAsync(ProcessData pData, SettingsOnly settings, Resource resource);
        Task<int> UpdateAsync(ProcessData pData, SettingsOnly settings, Resource resource);
        Task<int> DeleteAsync(ProcessData pData, Resource resource);
        Task<string> GetSingleAsync(ProcessData pData, Resource resource);
        Task<IEnumerable<string>> GetManyAsync(ProcessData pData, Resource resource);
    }
}
