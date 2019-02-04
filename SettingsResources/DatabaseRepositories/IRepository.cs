using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SettingsResources.DatabaseRepositories
{
    public interface IRepository<T> //where T : BaseEntity
    {
        Task<int> CreateAsync(ProcessData pData, SettingsOnly settings);
        Task<int> UpdateAsync(ProcessData pData, SettingsOnly settings);
        Task<int> DeleteAsync(ProcessData pData);
        Task<T> GetSingleAsync(ProcessData pData);
        Task<IEnumerable<T>> GetManyAsync(ProcessData pData);
    }
}
