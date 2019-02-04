using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsResources.DatabaseRepositories
{
    public class GrandparentRepository<T> : IRepository<Grandparent> //where T : class
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;
        internal IDbConnection DbConnection => new NpgsqlConnection(connectionString);

        public GrandparentRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:PostgresDb"];
        }

        public async Task<int> CreateAsync(ProcessData pData, SettingsOnly settings)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO dbo.grandparent");
            sql.AppendLine("(name,values,aid) VALUES (");
            sql.AppendLine($"'{settings.Name}',");
            sql.AppendLine($"'{settings.Values}',");
            sql.AppendLine($"{pData.AccountId}");
            sql.AppendLine(") RETURNING gpid;");

            using (DbConnection)
            {
                DbConnection.Open();
                var result = await DbConnection.QueryAsync<int>(sql.ToString());
                return result.FirstOrDefault();
            }
        }

        public async Task<int> UpdateAsync(ProcessData pData, SettingsOnly settings)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE dbo.grandparent SET");

            StringBuilder p = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(settings.Name)) p.Append($",name = '{settings.Name}'");
            if (!string.IsNullOrWhiteSpace(settings.Values)) p.Append($",values = '{settings.Values}'");
            sql.AppendLine(p.ToString().TrimStart(','));
            if (p.Length <= 5) return 0;

            sql.AppendLine($"WHERE aid = {pData.AccountId} AND gpid = {pData.Gpid} RETURNING gpid;");

            using (DbConnection)
            {
                DbConnection.Open();
                var result = await DbConnection.QueryAsync<int>(sql.ToString());
                return result.FirstOrDefault();
            }
        }

        public async Task<int> DeleteAsync(ProcessData pData)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                var sql = $"DELETE FROM dbo.grandparent WHERE aid = {pData.AccountId} AND gpid = {pData.Gpid} RETURNING gpid;";
                var result = await DbConnection.QueryAsync<int>(sql);
                return result.FirstOrDefault();
            }
        }

        public async Task<Grandparent> GetSingleAsync(ProcessData pData)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                var sql = $"SELECT * FROM dbo.grandparent WHERE aid = {pData.AccountId} AND gpid = {pData.Gpid};";
                var result = await DbConnection.QueryAsync<Grandparent>(sql);
                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Grandparent>> GetManyAsync(ProcessData pData)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                var sql = $"SELECT * FROM dbo.grandparent WHERE aid = {pData.AccountId};";
                var result = await DbConnection.QueryAsync<Grandparent>(sql);
                return result;
            }
        }
    }
}
