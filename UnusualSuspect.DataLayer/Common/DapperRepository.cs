using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using UnusualSuspect.DataLayer.Contracts;
using Microsoft.Extensions.Options;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.DataLayer.Common
{
  public class DapperRepository(IOptionsSnapshot<ProjectSetting> setting) : IDapperRepository
  {
    private readonly string connectionString = setting.Value.ConnectionStrings.ApplicationConnectionString;

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, CommandType commandType = CommandType.Text)
    {
      using IDbConnection db = new SqlConnection(connectionString);
      return await db.QueryAsync<T>(sql, param, commandType: commandType);
    }
    public async Task<T?> QuerySingleAsync<T>(string sql, object? param = null, CommandType commandType = CommandType.Text)
    {
      using IDbConnection db = new SqlConnection(connectionString);
      return await db.QuerySingleOrDefaultAsync<T>(sql, param, commandType: commandType);
    }
    public async Task<int> ExecuteAsync(string sql, object? param = null, CommandType commandType = CommandType.Text)
    {
      using IDbConnection db = new SqlConnection(connectionString);
      return await db.ExecuteAsync(sql, param, commandType: commandType);
    }
  }
}
