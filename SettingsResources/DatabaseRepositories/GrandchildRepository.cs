using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using SettingsUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsResources.DatabaseRepositories
{
    public class GrandchildRepository<T> : IRepository<Grandchild>
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;
        internal IDbConnection DbConnection => new NpgsqlConnection(connectionString);

        public GrandchildRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:PostgresDb"];
        }

        public async Task<int> CreateAsync(ProcessData pData, SettingsOnly settings)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO dbo.grandchild");
            // Remember to make the order of the properties match the values.
            sql.Append("(gc_name,gpid,pid,cid,aid");
            if (settings.Values != null && settings.Values.Length > 0)
                sql.Append(",gc_values");
            sql.AppendLine(") VALUES (");

            sql.AppendLine($"'{settings.Name}'");
            sql.AppendLine($",{pData.Gpid}");
            sql.AppendLine($",{pData.Pid}");
            sql.AppendLine($",{pData.Cid}");
            sql.AppendLine($",{pData.AccountId}");
            if (settings.Values != null && settings.Values.Length > 0)
                sql.AppendLine($",'{MutateString.ConvertToJsonbString(settings.Values)}'");

            // Return the created ID for reference.
            sql.AppendLine(") RETURNING gcid;");

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
            sql.AppendLine("UPDATE dbo.grandchild SET");

            StringBuilder p = new StringBuilder();
            // Set properties to be updated only if they contain a value.
            if (!string.IsNullOrWhiteSpace(settings.Name)) p.Append($",gc_name = '{settings.Name}'");
            if (settings.Values != null) p.Append($",gc_values = '{MutateString.ConvertToJsonbString(settings.Values)}'");

            sql.AppendLine(p.ToString().TrimStart(','));
            // Skip the whole thing if no values were updated. 
            if (p.Length <= 5) return 0;

            sql.AppendLine($"WHERE aid = {pData.AccountId} AND gcid = {pData.Gcid} RETURNING gcid;");

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
                var sql = $"DELETE FROM dbo.grandchild WHERE aid = {pData.AccountId} AND gcid = {pData.Gcid};";
                var result = await DbConnection.ExecuteAsync(sql);
                return result;
            }
        }

        public async Task<Grandchild> GetSingleAsync(ProcessData pData)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                var sql = $"SELECT * FROM dbo.grandchild WHERE aid = {pData.AccountId} AND gcid = {pData.Gcid};";
                var result = await DbConnection.QueryAsync<Grandchild>(sql);
                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Grandchild>> GetManyAsync(ProcessData pData)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                var sql = $"SELECT * FROM dbo.grandchild WHERE aid = {pData.AccountId} AND gpid = {pData.Gpid} AND pid = {pData.Pid} AND cid = {pData.Cid};";
                var result = await DbConnection.QueryAsync<Grandchild>(sql);
                return result;
            }
        }
    }
}
