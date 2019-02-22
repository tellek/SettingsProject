using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.ApiTransaction.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SettingsProject.Managers
{
    public interface ISettingsManager<T>
    {
        Task<(int, object)> GetSettingAsync(ProcessData pData);
        Task<(int, object)> GetSettingsAsync(ProcessData pData);
        Task<(int, object)> CreateSettingAsync(ProcessData pData, SettingsOnly payload, ControllerBase controller, HttpContext context);
        Task<int> UpdateSettingAsync(ProcessData pData, SettingsOnly payload);
        Task<int> DeleteSettingAsync(ProcessData pData);
    }
}
