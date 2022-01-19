// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Codeit.Infrastructure.Identity.Config;
using Codeit.NetStdLibrary.Base.Common;
using Codeit.NetStdLibrary.Base.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Codeit.Infrastructure.Identity
{
    public class Program
    {
        public static string AppName { get; } = typeof(Program).Namespace;
        private static readonly IConfiguration configuration = CodeitUtils.BuildDefaultSettings(new ConfigurationBuilder());
        private static readonly AppSettings appSettings = configuration.Get<AppSettings>();

        public static void Main(string[] args)
        {
            Log.Logger = CreateSerilogLogger();
            
            try
            {
                Log.Information("Configuring web host ({ApplicationName})...", AppName);
                var host = Host
                    .CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(wb => wb.UseStartup<Startup>())
                    .UseSerilog(Log.Logger)
                    .Build();

                Log.Information("Starting web host ({ApplicationName})...", AppName);
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

        public static ILogger CreateSerilogLogger()
        {
            var logConf = new LoggerConfiguration()
             .ReadFrom.Configuration(configuration)
             .Enrich.WithProperty("ApplicationName", AppName)
             .Enrich.WithEnvironmentName()
             .Enrich.WithMachineName()
             .Enrich.FromLogContext();

            if (appSettings.UseLoggerServer is true)
                logConf.WriteTo.Elasticsearch(ConfigureElasticSink());

            return logConf.CreateLogger();
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink()
        {
            return new ElasticsearchSinkOptions(new Uri(appSettings.LoggerServerUrl))
            {
                BufferCleanPayload = (failingEvent, statuscode, exception) =>
                {
                    dynamic e = JObject.Parse(failingEvent);
                    return JsonConvert.SerializeObject(new Dictionary<string, object>()
                    {
                        { "@timestamp",e["@timestamp"]},
                        { "level","Error"},
                        { "message","Error: "+e.message},
                        { "messageTemplate",e.messageTemplate},
                        { "failingStatusCode", statuscode},
                        { "failingException", exception}
                    });
                },
                MinimumLogEventLevel = LogEventLevel.Debug,
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                IndexFormat = $"codeit-infrastructure-{DateTime.UtcNow:yyyy-MM}",
                EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                       EmitEventFailureHandling.WriteToFailureSink |
                                       EmitEventFailureHandling.RaiseCallback
            };
        }
    }
}