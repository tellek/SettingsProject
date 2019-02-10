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
    public class DbRepository : IDbRepository
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;
        internal IDbConnection DbConnection => new NpgsqlConnection(connectionString);

        public DbRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:PostgresDb"];
        }

        public async Task<long> CreateAsync(ProcessData pData, SettingsOnly settings, Resource resource)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                string sql = "";
                string value = MutateString.ConvertToJsonbString(settings.Values);
                switch (resource)
                {
                    case Resource.Account:
                        sql = $"SELECT dbo.account_insert({settings.Name});";
                        break;
                    case Resource.Grandparent:
                        sql = $"SELECT dbo.setting_gp_insert({pData.AccountId},{settings.Name},{value};";
                        break;
                    case Resource.Parent:
                        sql = $"SELECT dbo.setting_p_insert({pData.Gpid},{settings.Name},{value});";
                        break;
                    case Resource.Child:
                        sql = $"SELECT dbo.setting_c_insert({pData.Pid},{settings.Name},{value});";
                        break;
                    case Resource.Grandchild:
                        sql = $"SELECT dbo.setting_gc_insert({pData.Cid},{settings.Name},{value});";
                        break;
                    case Resource.User:
                        throw new NotImplementedException();
                }
                var result = await DbConnection.QueryAsync<long>(sql);
                return result.FirstOrDefault();
            }
        }

        public async Task<int> UpdateAsync(ProcessData pData, SettingsOnly settings, Resource resource)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                string sql = "";
                string value = MutateString.ConvertToJsonbString(settings.Values);
                switch (resource)
                {
                    case Resource.Account:
                        sql = $"SELECT dbo.account_update({pData.AccountId},{settings.Name});";
                        break;
                    case Resource.Grandparent:
                        sql = $"SELECT dbo.setting_gp_update({pData.Gpid},{settings.Name},{value};";
                        break;
                    case Resource.Parent:
                        sql = $"SELECT dbo.setting_p_update({pData.Pid},{settings.Name},{value});";
                        break;
                    case Resource.Child:
                        sql = $"SELECT dbo.setting_c_update({pData.Cid},{settings.Name},{value});";
                        break;
                    case Resource.Grandchild:
                        sql = $"SELECT dbo.setting_gc_update({pData.Gcid},{settings.Name},{value});";
                        break;
                    case Resource.User:
                        throw new NotImplementedException();
                }
                var result = await DbConnection.QueryAsync<int>(sql);
                return result.FirstOrDefault();
            }
        }

        public async Task<int> DeleteAsync(ProcessData pData, Resource resource)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                string sql = "";
                switch (resource)
                {
                    case Resource.Account:
                        sql = $"SELECT dbo.account_delete({pData.AccountId});";
                        break;
                    case Resource.Grandparent:
                        sql = $"SELECT dbo.setting_gp_delete({pData.Gpid});";
                        break;
                    case Resource.Parent:
                        sql = $"SELECT dbo.setting_p_delete({pData.Pid});";
                        break;
                    case Resource.Child:
                        sql = $"SELECT dbo.setting_c_delete({pData.Cid});";
                        break;
                    case Resource.Grandchild:
                        sql = $"SELECT dbo.setting_gc_delete({pData.Gcid});";
                        break;
                    case Resource.User:
                        throw new NotImplementedException();
                }
                var result = await DbConnection.QueryAsync<int>(sql);
                return result.FirstOrDefault();
            }
        }

        public async Task<string> GetSingleAsync(ProcessData pData, Resource resource)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                string sql = "";
                switch (resource)
                {
                    case Resource.Account:
                        sql = $"SELECT dbo.account_select({pData.AccountId});";
                        break;
                    case Resource.Grandparent:
                        sql = $"SELECT dbo.setting_gp_select({pData.Gpid});";
                        break;
                    case Resource.Parent:
                        sql = $"SELECT dbo.setting_p_select({pData.Pid});";
                        break;
                    case Resource.Child:
                        sql = $"SELECT dbo.setting_c_select({pData.Cid});";
                        break;
                    case Resource.Grandchild:
                        sql = $"SELECT dbo.setting_gc_select({pData.Gcid});";
                        break;
                    case Resource.User:
                        sql = $"SELECT dbo.users_select({pData.UserId});";
                        break;
                }
                var result = await DbConnection.QueryAsync<string>(sql);
                return result.FirstOrDefault();
            }
            
        }

        public async Task<IEnumerable<string>> GetManyAsync(ProcessData pData, Resource resource)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                string sql = "";
                switch (resource)
                {
                    case Resource.Account:
                        throw new NotImplementedException();
                    case Resource.Grandparent:
                        sql = $"SELECT dbo.collection_gp_select({pData.AccountId});";
                        break;
                    case Resource.Parent:
                        sql = $"SELECT dbo.collection_p_select({pData.Gpid});";
                        break;
                    case Resource.Child:
                        sql = $"SELECT dbo.collection_c_select({pData.Pid});";
                        break;
                    case Resource.Grandchild:
                        sql = $"SELECT dbo.collection_gc_select({pData.Cid});";
                        break;
                    case Resource.User:
                        sql = $"SELECT dbo.collection_users_select({pData.AccountId});";
                        break;
                }
                var result = await DbConnection.QueryAsync<string>(sql);
                return result;
            }
        }

    }
}
