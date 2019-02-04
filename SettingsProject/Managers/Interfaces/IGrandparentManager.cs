using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SettingsProject.Managers.Interfaces
{
    public interface IGrandparentManager
    {
        Task<(int, Grandparent)> GetGrandparentAsync(int accountId, long gpid);
        Task<(int, IEnumerable<Grandparent>)> GetGrandparentsAsync(int accountId);
        Task<(int, long)> CreateGrandparentAsync(int accountId, Grandparent payload);
        Task<int> UpdateGrandparentAsync(int accountId, long gpid, Grandparent payload);
        Task<int> DeleteGrandparentAsync(int accountId, long gpid);
    }
}
