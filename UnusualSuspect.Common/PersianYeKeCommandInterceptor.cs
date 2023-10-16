using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace UnusualSuspect.Common;

public class PersianYeKeCommandInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        ApplyCorrectYeKe(command);
        return result;
    }


    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
    {
        ApplyCorrectYeKe(command);
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyCorrectYeKe(command);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        ApplyCorrectYeKe(command);
        return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override InterceptionResult<object> ScalarExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<object> result)
    {
        ApplyCorrectYeKe(command);
        return result;
    }


    public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
    {
        ApplyCorrectYeKe(command);
        return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
    }

    private static void ApplyCorrectYeKe(DbCommand command)
    {
        command.CommandText = command.CommandText.ApplyCorrectYeKe();

        foreach (DbParameter parameter in command.Parameters)
        {
            switch (parameter.DbType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.Xml:
                    if (!(parameter.Value is DBNull) && parameter.Value is string)
                    {
                        parameter.Value =
                            Convert.ToString(parameter.Value, CultureInfo.InvariantCulture).ApplyCorrectYeKe();
                    }
                    break;
            }
        }
    }
}
