using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsResources.DatabaseRepositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Upsert(T item);
        void Delete(Int64 id);
        T GetById(Int64 id);
        IEnumerable<T> GetAll(Int64 id);
    }
}
