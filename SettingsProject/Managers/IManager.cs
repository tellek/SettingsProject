using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SettingsProject.Managers
{
    public interface IManager<T>
    {
        Task<(int, T)> GetSettingAsync(ProcessData pData);
        Task<(int, IEnumerable<T>)> GetSettingsAsync(ProcessData pData);
        Task<(int, long)> CreateSettingAsync(ProcessData pData, SettingsOnly payload);
        Task<int> UpdateSettingAsync(ProcessData pData, SettingsOnly payload);
        Task<int> DeleteSettingAsync(ProcessData pData);
    }
}
