using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using SettingsUtilities;
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
            // Remember to make the order of the properties match the values.
            sql.Append("(gp_name,aid");
            if (settings.Values != null && settings.Values.Length > 0)
                sql.Append(",gp_values");
            sql.AppendLine(") VALUES (");

            sql.AppendLine($"'{settings.Name}'");
            if (settings.Values != null && settings.Values.Length > 0)
                sql.AppendLine($",{MutateString.ConvertToJsonbString(settings.Values)}");
            sql.AppendLine($",{pData.AccountId}");

            // Return the created ID for reference.
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
            // Set properties to be updated only if they contain a value.
            if (!string.IsNullOrWhiteSpace(settings.Name)) p.Append($",gp_name = '{settings.Name}'");
            if (settings.Values != null) p.Append($",gp_values = '{MutateString.ConvertToJsonbString(settings.Values)}'");

            sql.AppendLine(p.ToString().TrimStart(','));
            // Skip the whole thing if no values were updated. 
            if (p.Length <= 5) return 0;

            sql.AppendLine($"WHERE aid = {pData.AccountId} AND gpid = {pData.Gpid};");

            using (DbConnection)
            {
                DbConnection.Open();
                var result = await DbConnection.ExecuteAsync(sql.ToString());
                return result;
            }
        }

        public async Task<int> DeleteAsync(ProcessData pData)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                var sql = $"DELETE FROM dbo.grandparent WHERE aid = {pData.AccountId} AND gpid = {pData.Gpid};";
                var result = await DbConnection.ExecuteAsync(sql);
                return result;
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
