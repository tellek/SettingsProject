using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SettingsProject.Managers.Interfaces
{
    public interface IChildManager
    {
        Task<(int, Child)> GetChildAsync(ProcessData pData);
        Task<(int, IEnumerable<Child>)> GetChildsAsync(ProcessData pData);
        Task<(int, long)> CreateChildAsync(ProcessData pData, SettingsOnly payload);
        Task<int> UpdateChildAsync(ProcessData pData, SettingsOnly payload);
        Task<int> DeleteChildAsync(ProcessData pData);
    }
}
