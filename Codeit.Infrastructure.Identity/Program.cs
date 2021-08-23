// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Codeit.NetStdLibrary.Base.Common;
using Serilog;
using System;
using System.IO;

namespace Codeit.Infrastructure.Identity
{
    public class Program
    {
        public static readonly string _appName = typeof(Program).Namespace;

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("ApplicationName", _appName)
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(CodeitUtils.BuildDefaultSettings(new ConfigurationBuilder()))
                .CreateLogger();

            try
            {
                Log.Information("Configuring web host ({ApplicationName})...", _appName);
                var host = Host
                    .CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(wb => wb.UseStartup<Startup>())
                    .UseSerilog()
                    .Build();

                //Log.Warning()

                Log.Information("Starting web host ({ApplicationName})...", _appName);
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