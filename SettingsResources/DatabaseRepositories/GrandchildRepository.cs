using Microsoft.Extensions.Configuration;
using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsResources.DatabaseRepositories
{
    class GrandchildRepository : IRepository<Grandchild>
    {
        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Grandchild> GetAll(long id)
        {
            throw new NotImplementedException();
        }

        public Grandchild GetById(long id)
        {
            throw new NotImplementedException();
        }

        public void Upsert(Grandchild item)
        {
            throw new NotImplementedException();
        }
    }
}
