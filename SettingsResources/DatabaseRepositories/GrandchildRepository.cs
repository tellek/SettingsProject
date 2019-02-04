using Microsoft.Extensions.Configuration;
using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SettingsResources.DatabaseRepositories
{
    public class GrandchildRepository<Grandchild> : IRepository<Grandchild>
    {
        public Task<int> CreateAsync(int accountId, Grandchild item)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int accountId, long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Grandchild>> GetManyAsync(int accountId, long id)
        {
            throw new NotImplementedException();
        }

        public Task<Grandchild> GetSingleAsync(int accountId, long id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(int accountId, long id, Grandchild item)
        {
            throw new NotImplementedException();
        }
    }
}
