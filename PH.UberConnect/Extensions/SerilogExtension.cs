﻿using Serilog;

namespace PH.UberConnect.Extensions
{
    public static class SerilogExtension
    {
        public static void AddSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("Logs/log_.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
