using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SettingsContracts;
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

        public async Task<int> CreateAsync(int accountId, Grandparent item)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO dbo.grandparent");
            sql.AppendLine("(gp_name,aid) VALUES (");
            sql.AppendLine($"'{item.Name}',");
            sql.AppendLine($"{accountId}");
            sql.AppendLine(") RETURNING gpid;");

            using (DbConnection)
            {
                DbConnection.Open();
                var result = await DbConnection.QueryAsync<int>(sql.ToString());
                return result.FirstOrDefault();
            }
        }

        public async Task<int> UpdateAsync(int accountId, long id, Grandparent item)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE dbo.grandparent SET");

            StringBuilder p = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(item.Name)) p.Append($",gp_name = '{item.Name}'");
            sql.AppendLine(p.ToString().TrimStart(','));
            if (p.Length <= 5) return 0;

            sql.AppendLine($"WHERE aid = {accountId} AND gpid = {id} RETURNING gpid;");

            using (DbConnection)
            {
                DbConnection.Open();
                var result = await DbConnection.QueryAsync<int>(sql.ToString());
                return result.FirstOrDefault();
            }
        }

        public async Task<int> DeleteAsync(int accountId, long id)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                var sql = $"DELETE FROM dbo.grandparent WHERE aid = {accountId} AND gpid = {id} RETURNING gpid;";
                var result = await DbConnection.QueryAsync<int>(sql);
                return result.FirstOrDefault();
            }
        }

        public async Task<Grandparent> GetSingleAsync(int accountId, long id)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                var sql = $"SELECT * FROM dbo.grandparent WHERE aid = {accountId} AND gpid = {id};";
                var result = await DbConnection.QueryAsync<Grandparent>(sql);
                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Grandparent>> GetManyAsync(int accountId, long id = 0)
        {
            using (DbConnection)
            {
                DbConnection.Open();
                var sql = $"SELECT * FROM dbo.grandparent WHERE aid = {accountId};";
                var result = await DbConnection.QueryAsync<Grandparent>(sql);
                return result;
            }
        }
    }
}
