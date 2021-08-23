
namespace Codeit.Infrastructure.Identity.DAL.Context
{
    using IdentityServer4.EntityFramework.DbContexts;
    using IdentityServer4.EntityFramework.Options;
    using Microsoft.EntityFrameworkCore;


    public class ConfigurationDBContext : ConfigurationDbContext<ConfigurationDBContext>
    {
        private const string SCHEMA_NAME = "ENGINE";

        public ConfigurationDBContext(DbContextOptions<ConfigurationDBContext> options, ConfigurationStoreOptions storeOptions)
            : base(options, storeOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(SCHEMA_NAME);
            base.OnModelCreating(builder);
        }
    }
}
