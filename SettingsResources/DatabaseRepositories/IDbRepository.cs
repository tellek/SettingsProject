using SettingsContracts;
using SettingsContracts.ApiTransaction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SettingsResources.DatabaseRepositories
{
    public interface IDbRepository<T>
    {
        Task<long> CreateAsync(ProcessData pData, SettingsOnly settings, Resource resource);
        Task<int> UpdateAsync(ProcessData pData, SettingsOnly settings, Resource resource);
        Task<int> DeleteAsync(ProcessData pData, Resource resource);
        Task<T> GetSingleAsync(ProcessData pData, Resource resource);
        Task<IEnumerable<T>> GetManyAsync(ProcessData pData, Resource resource);
    }
}
