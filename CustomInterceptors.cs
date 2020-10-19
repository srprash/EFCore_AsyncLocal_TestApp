using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp
{
    public class CustomInterceptor_1 : DbCommandInterceptor
    {
        public override Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            Debug.WriteLine("Inside ReaderExecutingAsync of CustomInterceptor_1");

            // Set a value to AsyncLocal here
            var val = "Value_1";
            Debug.Print("Setting AsyncLocal value as '{0}'", val);
            AsyncLocalContext._asyncLocalString.Value = val;

            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override Task<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
        {
            Debug.WriteLine("Inside ReaderExecutedAsync of CustomInterceptor_1");

            // read that value from AsyncLocal here
            var val = AsyncLocalContext._asyncLocalString.Value;
            Debug.Print("Getting AsyncLocalContext Value: '{0}'", val);

            return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
        }
    }

    public class CustomInterceptor_2 : DbCommandInterceptor
    {
        public override Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            Debug.WriteLine("Inside ReaderExecutingAsync of CustomInterceptor_2");
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override Task<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
        {
            Debug.WriteLine("Inside ReaderExecutedAsync of CustomInterceptor_2");
            return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
        }
    }
}
