﻿
namespace Codeit.Infrastructure.Identity.DAL.Context
{
    using Codeit.Enterprise.Base.Abstractions.DataAccess;
    using Codeit.Enterprise.Base.DataAccess;
    using IdentityServer4.EntityFramework.Options;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public class EFPersistenceBuilder : IPersistenceBuilder
    {
        private static EFPersistenceBuilder instance;

        private readonly DALSettings _setting;
        private EFPersistenceBuilder(DALSettings settings)
        {
            _setting = settings ?? throw new DataAccessLayerException(nameof(settings));
        }

        public void BuildConfiguration(DbContextOptionsBuilder options)
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            options.EnableDetailedErrors(_setting.EnableDetailedDebug is true);
            options.EnableSensitiveDataLogging(_setting.EnableDetailedDebug is true);
            options.UseSqlServer(_setting.DatabaseConnection, sqlOpt =>
            {
                sqlOpt.MigrationsHistoryTable("Migrations", "CONFIG");
                sqlOpt.MigrationsAssembly(typeof(DependencyInjection).GetTypeInfo().Assembly.GetName().Name);
            });
        }

        public void ConfigureOperationalStore(OperationalStoreOptions storeOptions)
        {
            storeOptions.ConfigureDbContext = BuildConfiguration;
            storeOptions.EnableTokenCleanup = true;
        }

        public void ConfigureGrantsStore(ConfigurationStoreOptions storeOptions)
        {
            storeOptions.ConfigureDbContext = BuildConfiguration;
        }

        public static EFPersistenceBuilder Build(DALSettings settings)
        {
            if (instance == null)
                instance = new EFPersistenceBuilder(settings);

            return instance;
        }
    }
}
