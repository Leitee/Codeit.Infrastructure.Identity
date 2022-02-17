/// <summary>
/// 
/// </summary>
namespace Codeit.Infrastructure.Identity.DAL
{
    using Codeit.Infrastructure.Identity.DAL.Context;
    using Codeit.Infrastructure.Identity.Model.Entities;
    using Codeit.Enterprise.Base.DataAccess;
    using Codeit.Enterprise.Base.Identity;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static partial class DependencyInjection
    {
        public static IServiceCollection AddPersistenceTier(this IServiceCollection services, string sectionKey)
        {
            DALSettings dalSettings;
            using (var servProv = services.BuildServiceProvider())
            {
                var config = servProv.GetService<IConfiguration>();
                dalSettings = config.GetSection(sectionKey).Get<DALSettings>();
            }

            if (dalSettings is null)
                throw new ArgumentNullException(nameof(dalSettings));

            services.Configure<DALSettings>(sp => sp = dalSettings);
            var efPersistenceBuilder = EFPersistenceBuilder.Build(dalSettings);

            services
                .AddDbContext<IdentityDBContext>(efPersistenceBuilder.BuildConfiguration)
                .AddIdentity<IdentityAppUser, ApplicationRole>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 0;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = true;
                    options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                    options.SignIn.RequireConfirmedEmail = false;
                })
                .AddEntityFrameworkStores<IdentityDBContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
