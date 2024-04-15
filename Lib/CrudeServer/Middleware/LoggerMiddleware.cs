﻿using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

using CrudeServer.Middleware.Registration.Contracts;
using CrudeServer.Models.Contracts;
using CrudeServer.Providers.Contracts;

namespace CrudeServer.Middleware
{
    public class LoggerMiddleware : IMiddleware
    {
        private static int _counter = 0;
        private readonly ILoggerProvider _loggerProvider;

        public LoggerMiddleware(ILoggerProvider loggerProvider)
        {
            this._loggerProvider = loggerProvider;
        }

        public async Task Process(IRequestContext context, Func<Task> next)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Request #{++_counter}");
            sb.AppendLine(context.Url.ToString());
            sb.AppendLine(context.HttpMethod.ToString());
            sb.AppendLine(context.Host);
            sb.AppendLine(context.UserAgent);
            sb.AppendLine();

            this._loggerProvider.Log(sb.ToString());

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            await next();

            stopwatch.Stop();

            this._loggerProvider.Log($"Request #{0} completed in {1}ms", _counter, stopwatch.ElapsedMilliseconds);
        }
    }
}
