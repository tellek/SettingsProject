using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using SettingsContracts;
using SettingsContracts.ApiTransaction;
using System.Data;
using Npgsql;
using Dapper;
using System.Linq;

namespace SettingsResources.DatabaseRepositories
{
    public class ParentRepository<T> : IRepository<Parent>
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;
        internal IDbConnection DbConnection => new NpgsqlConnection(connectionString);

        public ParentRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:PostgresDb"];
        }

        public async Task<int> CreateAsync(ProcessData pData, SettingsOnly settings)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO dbo.parent");
            sql.AppendLine("(name,values,gpid,aid) VALUES (");
            sql.AppendLine($"'{settings.Name}',");
            sql.AppendLine($"'{settings.Values}',");
            sql.AppendLine($"{pData.Gpid},");
            sql.AppendLine($"{pData.AccountId}");
            sql.AppendLine(") RETURNING pid;");

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
            sql.AppendLine("UPDATE dbo.parent SET");

            StringBuilder p = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(settings.Name)) p.Append($",name = '{settings.Name}'");
            if (!string.IsNullOrWhiteSpace(settings.Values)) p.Append($",values = '{settings.Values}'");
            sql.AppendLine(p.ToString().TrimStart(','));
            if (p.Length <= 5) return 0;

            sql.AppendLine($"WHERE aid = {pData.AccountId} AND pid = {pData.Pid} RETURNING pid;");

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
                var sql = $"DELETE FROM dbo.parent WHERE aid = {pData.AccountId} AND pid = {pData.Pid} RETURNING pid;";
                var result = await DbConnection.QueryAsync<int>(sql);
                return result.FirstOrDefault();
            }
        }

        public async Task<Parent> GetSingleAsync(ProcessData pData)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                var sql = $"SELECT * FROM dbo.parent WHERE aid = {pData.AccountId} AND pid = {pData.Pid};";
                var result = await DbConnection.QueryAsync<Parent>(sql);
                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Parent>> GetManyAsync(ProcessData pData)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                var sql = $"SELECT * FROM dbo.parent WHERE aid = {pData.AccountId} AND gpid = {pData.Gpid};";
                var result = await DbConnection.QueryAsync<Parent>(sql);
                return result;
            }
        }
    }
}
