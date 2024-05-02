using System.Data;

namespace UnusualSuspect.DataLayer.Contracts;

public interface IDapperRepository
{
  Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null,
    CancellationToken cancellationToken = default, IDbTransaction? dbTransaction = null,
    int? commandTimeout = null, CommandType commandType = CommandType.Text);
  Task<T?> QuerySingleAsync<T>(string sql, object? param = null,
    CancellationToken cancellationToken = default, IDbTransaction? dbTransaction = null,
    int? commandTimeout = null, CommandType commandType = CommandType.Text);
  Task<int> ExecuteAsync(string sql, object? param = null,
    CancellationToken cancellationToken = default, IDbTransaction? dbTransaction = null,
    int? commandTimeout = null, CommandType commandType = CommandType.Text);
}