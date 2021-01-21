/// <summary>
/// 
/// </summary>
namespace Pandora.Infrastructure.Identity.DAL
{
    using Pandora.Infrastructure.Identity.DAL.Context;
    using Pandora.Infrastructure.Identity.Model.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Pandora.NetStdLibrary.Base.Identity;
    using System;

    public static partial class DependencyInjection
    {
        public static IServiceCollection AddPersistenceTier(this IServiceCollection services, IConfiguration configuration)
        {
            var efPersistenceBuilder = IdentityEFPersistenceBuilder.Build(configuration);

            services
                .AddDbContext<IdentityDBContext>(efPersistenceBuilder.ConfigurePersistence)
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
