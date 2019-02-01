using Dapper;
using Microsoft.Extensions.Configuration;
using SettingsContracts;
using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SettingsResources.DatabaseRepositories
{
    class GrandparentRepository : IRepository<Grandparent>
    {
        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Grandparent> GetAll(long id)
        {
            throw new NotImplementedException();
        }

        public Grandparent GetById(long id)
        {
            using (IDbConnection dbConnection = new SqlConnection(StaticProps.DbConnectionString))
            {
                dbConnection.Open();
                var sql = $"SELECT * FROM dbo.grandparent WHERE gpid = {id}";
                var result = dbConnection.Query<Grandparent>(sql).FirstOrDefault();
                return result;
            }
        }

        public void Upsert(Grandparent item)
        {
            throw new NotImplementedException();
        }
    }
}
