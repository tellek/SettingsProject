using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SettingsResources.DatabaseRepositories
{
    public interface IRepository<T> //where T : BaseEntity
    {
        Task<int> CreateAsync(int accountId, T item);
        Task<int> UpdateAsync(int accountId, long id, T item);
        Task<int> DeleteAsync(int accountId, long id);
        Task<T> GetSingleAsync(int accountId, long id);
        Task<IEnumerable<T>> GetManyAsync(int accountId, long id);
    }
}
