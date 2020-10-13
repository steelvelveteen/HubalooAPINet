using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HubalooAPI.Dal.Database
{
    public interface IDatabase
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class;

        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class;

        Task<int> ExecuteAsync(string sql, object parameters = null, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null);
    }
}