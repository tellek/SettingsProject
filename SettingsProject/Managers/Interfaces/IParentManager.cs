using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SettingsProject.Managers.Interfaces
{
    public interface IParentManager
    {
        Task<(int, Parent)> GetParentAsync(ProcessData pData);
        Task<(int, IEnumerable<Parent>)> GetParentsAsync(ProcessData pData);
        Task<(int, long)> CreateParentAsync(ProcessData pData, SettingsOnly payload);
        Task<int> UpdateParentAsync(ProcessData pData, SettingsOnly payload);
        Task<int> DeleteParentAsync(ProcessData pData);
    }
}
