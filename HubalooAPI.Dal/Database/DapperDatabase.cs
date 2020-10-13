using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace HubalooAPI.Dal.Database
{
    public class DapperDatabase : IDatabase
    {
        private readonly IConfiguration _config;

        public DapperDatabase(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class
        {
            using (var connection = OpenConnection())
            {
                return await connection.QueryAsync<T>(sql, parameters, transaction, commandTimeout, commandType);
            }
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = OpenConnection())
            {
                return await connection.ExecuteAsync(sql, parameters, transaction, commandTimeout, commandType);
            }
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class
        {
            using (var connection = OpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters, transaction, commandTimeout, commandType);
            }
        }

        private IDbConnection OpenConnection()
        {
            // //MS SQL
            // var connection = new SqlConnection(_config["ConnectionString"]);
            // connection.Open();
            // return connection;

            //PSQL
            var connection = new NpgsqlConnection(_config["ConnectionString"]);
            return connection;
        }
    }
}