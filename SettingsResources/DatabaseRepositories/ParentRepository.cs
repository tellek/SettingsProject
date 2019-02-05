﻿using SettingsContracts.DatabaseModels;
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
using SettingsUtilities;

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
            // Remember to make the order of the properties match the values.
            sql.AppendLine("(name,values,gpid,aid) VALUES (");

            sql.AppendLine($"'{settings.Name}',");
            sql.AppendLine($"'{MutateString.ConvertToJsonbString(settings.Values)}',");
            sql.AppendLine($"{pData.Gpid},");
            sql.AppendLine($"{pData.AccountId}");

            // Return the created ID for reference.
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
            // Set properties to be updated only if they contain a value.
            if (!string.IsNullOrWhiteSpace(settings.Name)) p.Append($",name = '{settings.Name}'");
            if (settings.Values != null) p.Append($",values = '{MutateString.ConvertToJsonbString(settings.Values)}'");

            sql.AppendLine(p.ToString().TrimStart(','));
            // Skip the whole thing if no values were updated. 
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
                var sql = $"DELETE FROM dbo.parent WHERE aid = {pData.AccountId} AND pid = {pData.Pid};";
                var result = await DbConnection.ExecuteAsync(sql);
                return result;
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
