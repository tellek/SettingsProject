using SettingsContracts.ApiTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SettingsProject.Managers
{
    public interface IAuthManager
    {
        Task<(int, object)> DoAuthenticationAsync(AuthProperties payload);
    }
}
