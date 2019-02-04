using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsResources.DatabaseRepositories
{
    public class ChildRepository<T> : IRepository<Child>
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;
        internal IDbConnection DbConnection => new NpgsqlConnection(connectionString);

        public ChildRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:PostgresDb"];
        }

        public async Task<int> CreateAsync(ProcessData pData, SettingsOnly settings)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO dbo.child");
            sql.AppendLine("(name,values,gpid,pid,aid) VALUES (");
            sql.AppendLine($"'{settings.Name}',");
            sql.AppendLine($"'{settings.Values}',");
            sql.AppendLine($"{pData.Gpid},");
            sql.AppendLine($"{pData.Pid},");
            sql.AppendLine($"{pData.AccountId}");
            sql.AppendLine(") RETURNING cid;");

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
            sql.AppendLine("UPDATE dbo.child SET");

            StringBuilder p = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(settings.Name)) p.Append($",name = '{settings.Name}'");
            if (!string.IsNullOrWhiteSpace(settings.Values)) p.Append($",values = '{settings.Values}'");
            sql.AppendLine(p.ToString().TrimStart(','));
            if (p.Length <= 5) return 0;

            sql.AppendLine($"WHERE aid = {pData.AccountId} AND cid = {pData.Cid} RETURNING cid;");

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
                var sql = $"DELETE FROM dbo.child WHERE aid = {pData.AccountId} AND cid = {pData.Cid} RETURNING cid;";
                var result = await DbConnection.QueryAsync<int>(sql);
                return result.FirstOrDefault();
            }
        }

        public async Task<Child> GetSingleAsync(ProcessData pData)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                var sql = $"SELECT * FROM dbo.child WHERE aid = {pData.AccountId} AND cid = {pData.Cid};";
                var result = await DbConnection.QueryAsync<Child>(sql);
                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Child>> GetManyAsync(ProcessData pData)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                var sql = $"SELECT * FROM dbo.child WHERE aid = {pData.AccountId} AND gpid = {pData.Gpid} AND pid = {pData.Pid};";
                var result = await DbConnection.QueryAsync<Child>(sql);
                return result;
            }
        }
    }
}
