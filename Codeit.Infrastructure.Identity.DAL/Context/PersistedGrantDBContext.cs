
namespace Codeit.Infrastructure.Identity.DAL.Context
{
    using IdentityServer4.EntityFramework.DbContexts;
    using IdentityServer4.EntityFramework.Options;
    using Microsoft.EntityFrameworkCore;

    public class PersistedGrantDBContext : PersistedGrantDbContext<PersistedGrantDBContext>
    {
        private const string SCHEMA_NAME = "GRANTS";

        public PersistedGrantDBContext(DbContextOptions<PersistedGrantDBContext> options, OperationalStoreOptions storeOptions)
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
