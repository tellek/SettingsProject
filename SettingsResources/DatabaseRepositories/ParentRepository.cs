using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace SettingsResources.DatabaseRepositories
{
    public class ParentRepository<Parent> : IRepository<Parent>
    {
        public Task<int> CreateAsync(int accountId, Parent item)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int accountId, long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Parent>> GetManyAsync(int accountId, long id)
        {
            throw new NotImplementedException();
        }

        public Task<Parent> GetSingleAsync(int accountId, long id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(int accountId, long id, Parent item)
        {
            throw new NotImplementedException();
        }
    }
}
