using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pandora.NetStdLibrary.Base.Abstractions.DataAccess;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Pandora.Infrastructure.Identity.DAL.Context
{

    public class DesignTimeIdentityDBContextFactory : IDesignTimeDbContextFactory<IdentityDBContext>
    {
        public IdentityDBContext CreateDbContext(string[] args)
        {
            Console.WriteLine($"Arguments passed: {args.Join(",")}");
            var configuration = SharedHostConfiguration.GetBasicConfiguration();

            Console.WriteLine($"DesignTimeDbContextFactoryBase.CreatePersistenceBuilder.");
            var efPersistenceBuilder = CreatePersistenceBuilder(configuration);
            var optionsBuilder = new DbContextOptionsBuilder<IdentityDBContext>();
            efPersistenceBuilder.ConfigurePersistence(optionsBuilder);

            Console.WriteLine($"DesignTimeDbContextFactoryBase.CreateNewInstance.");
            return CreateNewInstance(optionsBuilder.Options);
        }
        protected override IdentityDBContext CreateNewInstance(DbContextOptions<IdentityDBContext> options)
        {
            return new IdentityDBContext(options);
        }

        protected override IPersistenceBuider CreatePersistenceBuilder(IConfiguration configuration)
        {
            return IdentityEFPersistenceBuilder.Build(configuration);
        }
    }
}
