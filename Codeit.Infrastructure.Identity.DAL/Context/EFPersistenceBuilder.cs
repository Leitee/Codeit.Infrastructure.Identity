using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Codeit.NetStdLibrary.Base.Abstractions.DataAccess;
using Codeit.NetStdLibrary.Base.DataAccess;
using System.Reflection;

namespace Codeit.Infrastructure.Identity.DAL.Context
{
    public class EFPersistenceBuilder : IPersistenceBuilder
    {
        private static EFPersistenceBuilder instance;

        private readonly DALSettings _setting;

        private EFPersistenceBuilder(IConfiguration configuration)
        {
            _setting = DALSettings.GetSection(configuration ?? throw new DataAccessTierException(nameof(configuration)));
        }

        public void BuildConfiguration(DbContextOptionsBuilder options)
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            options.EnableDetailedErrors(_setting.IsDevelopment);
            options.EnableSensitiveDataLogging(_setting.IsDevelopment);
            options.UseSqlServer(_setting.DatabaseUrl, sqlOpt =>
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

        public static EFPersistenceBuilder Build(IConfiguration configuration)
        {
            if (instance == null)
                instance = new EFPersistenceBuilder(configuration);

            return instance;
        }
    }
}
