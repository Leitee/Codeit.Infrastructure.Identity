// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Pandora.NetStdLibrary.Base.Common;
using Serilog;
using System;

namespace Pandora.Infrastructure.Identity
{
    public class Program
    {
        public static readonly string _appName = typeof(Program).Namespace;
        private static string _environmentName = Environments.Development;

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("ApplicationName", _appName)
                .Enrich.WithProperty("Environment", _environmentName)
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(new ConfigurationBuilder().GetBasicConfiguration())
                .CreateLogger();

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", _appName);
                var host = Host
                    .CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(wb => wb.UseStartup<Startup>())
                    .UseSerilog()
                    .Build();

                Log.Information("Starting web host ({ApplicationContext})...", _appName);
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed at start up");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}