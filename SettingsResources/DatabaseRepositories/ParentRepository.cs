using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace SettingsResources.DatabaseRepositories
{
    class ParentRepository : IRepository<Parent>
    {
        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parent> GetAll(long id)
        {
            throw new NotImplementedException();
        }

        public Parent GetById(long id)
        {
            throw new NotImplementedException();
        }

        public void Upsert(Parent item)
        {
            throw new NotImplementedException();
        }
    }
}
