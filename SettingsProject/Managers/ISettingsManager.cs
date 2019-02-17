using SettingsContracts;
using SettingsContracts.ApiTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SettingsProject.Managers
{
    public interface ISettingsManager<T>
    {
        Task<(int, object)> GetSettingAsync(ProcessData pData, Resource resource);
        Task<(int, object)> GetSettingsAsync(ProcessData pData, Resource resource);
        Task<(int, long)> CreateSettingAsync(ProcessData pData, SettingsOnly payload, Resource resource);
        Task<int> UpdateSettingAsync(ProcessData pData, SettingsOnly payload, Resource resource);
        Task<int> DeleteSettingAsync(ProcessData pData, Resource resource);
    }
}
