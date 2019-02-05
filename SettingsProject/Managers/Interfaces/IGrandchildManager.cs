using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SettingsProject.Managers.Interfaces
{
    public interface IGrandchildManager
    {
        Task<(int, Grandchild)> GetGrandchildAsync(ProcessData pData);
        Task<(int, IEnumerable<Grandchild>)> GetGrandchildsAsync(ProcessData pData);
        Task<(int, long)> CreateGrandchildAsync(ProcessData pData, SettingsOnly payload);
        Task<int> UpdateGrandchildAsync(ProcessData pData, SettingsOnly payload);
        Task<int> DeleteGrandchildAsync(ProcessData pData);
    }
}
