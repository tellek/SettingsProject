using Microsoft.Extensions.Configuration;
using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsResources.DatabaseRepositories
{
    class ChildRepository : IRepository<Child>
    {
        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Child> GetAll(long id)
        {
            throw new NotImplementedException();
        }

        public Child GetById(long id)
        {
            throw new NotImplementedException();
        }

        public void Upsert(Child item)
        {
            throw new NotImplementedException();
        }
    }
}
