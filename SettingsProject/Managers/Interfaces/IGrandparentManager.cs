using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SettingsProject.Managers.Interfaces
{
    public interface IGrandparentManager
    {
        Task<(int, Grandparent)> GetGrandparentAsync(ProcessData pData);
        Task<(int, IEnumerable<Grandparent>)> GetGrandparentsAsync(ProcessData pData);
        Task<(int, long)> CreateGrandparentAsync(ProcessData pData, SettingsOnly payload);
        Task<int> UpdateGrandparentAsync(ProcessData pData, SettingsOnly payload);
        Task<int> DeleteGrandparentAsync(ProcessData pData);
    }
}
