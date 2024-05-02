using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using UnusualSuspect.DataLayer.Contracts;
using Microsoft.Extensions.Options;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.DataLayer.Common;

public class DapperRepository(IOptionsSnapshot<ProjectSetting> setting) : IDapperRepository
{
  private readonly string connectionString = setting.Value.ConnectionStrings.ApplicationConnectionString;

  public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null,
    CancellationToken cancellationToken = default, IDbTransaction? dbTransaction = null,
    int? commandTimeout = null, CommandType commandType = CommandType.Text)
  {
    using IDbConnection db = new SqlConnection(connectionString);
    var command = new CommandDefinition(sql, param, commandType: commandType,
      cancellationToken: cancellationToken, transaction: dbTransaction, commandTimeout: commandTimeout);
    return await db.QueryAsync<T>(command);
  }
  public async Task<T?> QuerySingleAsync<T>(string sql, object? param = null,
    CancellationToken cancellationToken = default, IDbTransaction? dbTransaction = null,
    int? commandTimeout = null, CommandType commandType = CommandType.Text)
  {
    using IDbConnection db = new SqlConnection(connectionString);
    var command = new CommandDefinition(sql, param, commandType: commandType,
      cancellationToken: cancellationToken, transaction: dbTransaction, commandTimeout: commandTimeout);
    return await db.QuerySingleOrDefaultAsync<T>(command);
  }
  public async Task<int> ExecuteAsync(string sql, object? param = null,
    CancellationToken cancellationToken = default, IDbTransaction? dbTransaction = null,
    int? commandTimeout = null ,CommandType commandType = CommandType.Text)
  {
    using IDbConnection db = new SqlConnection(connectionString);
    var command = new CommandDefinition(sql, param, commandType: commandType,
      cancellationToken: cancellationToken, transaction: dbTransaction, commandTimeout: commandTimeout);
    return await db.ExecuteAsync(command);
  }
}