using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pandora.NetStdLibrary.Base.Abstractions.DataAccess;
using Pandora.NetStdLibrary.Base.DataAccess;
using System.Reflection;

namespace Pandora.Infrastructure.Identity.DAL.Context
{
    public class IdentityEFPersistenceBuilder : IPersistenceBuider
    {
        private static IdentityEFPersistenceBuilder instance;

        private readonly DALSettings _setting;

        private IdentityEFPersistenceBuilder(IConfiguration configuration)
        {
            _setting = DALSettings.GetSection(configuration ?? throw new DataAccessTierException(nameof(configuration)));
        }

        public void ConfigurePersistence(DbContextOptionsBuilder options)
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
            storeOptions.ConfigureDbContext = ConfigurePersistence;
            storeOptions.EnableTokenCleanup = true;
        }

        public void ConfigureGrantsStore(ConfigurationStoreOptions storeOptions)
        {
            storeOptions.ConfigureDbContext = ConfigurePersistence;
        }

        public static IdentityEFPersistenceBuilder Build(IConfiguration configuration)
        {
            if (instance == null)
                instance = new IdentityEFPersistenceBuilder(configuration);

            return instance;
        }
    }
}
