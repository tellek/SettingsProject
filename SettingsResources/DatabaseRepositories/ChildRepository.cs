using Microsoft.Extensions.Configuration;
using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SettingsResources.DatabaseRepositories
{
    public class ChildRepository<Child> : IRepository<Child>
    {
        public Task<int> CreateAsync(int accountId, Child item)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int accountId, long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Child>> GetManyAsync(int accountId, long id)
        {
            throw new NotImplementedException();
        }

        public Task<Child> GetSingleAsync(int accountId, long id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(int accountId, long id, Child item)
        {
            throw new NotImplementedException();
        }
    }
}
