using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Codeit.NetStdLibrary.Base.Abstractions.DataAccess;
using Codeit.NetStdLibrary.Base.DataAccess;
using System.Reflection;

namespace Codeit.Infrastructure.Identity.DAL.Context
{
    public class IdentityEFPersistenceBuilder : IPersistenceBuider
    {
        private static IdentityEFPersistenceBuilder instance;

        private readonly DALSettings _setting;

        private IdentityEFPersistenceBuilder(DALSettings settings)
        {
            _setting = settings ?? throw new DataAccessTierException(nameof(settings));
        }

        public void ConfigurePersistence(DbContextOptionsBuilder options)
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            options.EnableDetailedErrors(_setting.IsDevelopment);
            options.EnableSensitiveDataLogging(_setting.IsDevelopment);
            options.UseSqlServer(_setting.DatabaseConnection, sqlOpt =>
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

        public static IdentityEFPersistenceBuilder Build(DALSettings settings)
        {
            if (instance == null)
                instance = new IdentityEFPersistenceBuilder(settings);

            return instance;
        }
    }
}
