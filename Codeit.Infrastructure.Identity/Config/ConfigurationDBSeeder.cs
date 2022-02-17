using Codeit.Infrastructure.Identity.DAL.Context;
using Codeit.Infrastructure.Identity.Model.Entities;
using Codeit.Enterprise.Base.Abstractions.Identity;
using Codeit.Enterprise.Base.Common;
using Codeit.Enterprise.Base.Identity;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Codeit.Infrastructure.Identity.Config
{
    public static class ConfigurationDBSeeder
    {
        private static ConfigurationDBContext configContext;

        private static TContext GetContext<TContext>(IApplicationBuilder builder) where TContext : DbContext
        {
            using var serviceScope = builder.ApplicationServices
                .GetService<IServiceScopeFactory>()
                .CreateScope();

            if (typeof(TContext) is ConfigurationDBContext)
            {
                return (configContext ?? serviceScope
                        .ServiceProvider
                        .GetRequiredService<ConfigurationDBContext>()) as TContext;
            }

            return serviceScope
                .ServiceProvider
                .GetRequiredService<TContext>();
        }

        public async static Task TryMigrateAsync(this IApplicationBuilder app, CancellationToken cancellationToken = default)
        {
            //using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            await GetContext<ConfigurationDBContext>(app).Database.MigrateAsync(cancellationToken);
            await GetContext<PersistedGrantDBContext>(app).Database.MigrateAsync(cancellationToken);
            await GetContext<IdentityDBContext>(app).Database.MigrateAsync(cancellationToken);
        }

        public static IApplicationBuilder TryMigrateAndSeed(this IApplicationBuilder app)
        {
            TryMigrateAsync(app, CancellationToken.None).GetAwaiter();
            return app;
        }

        public static void SeedClients(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDBContext>();

            #region Web Client
            if (!context.Clients.Any(_ => _.ClientId == "shcoolmngr-spa"))
            {
                var client = new Client
                {
                    ClientId = "shcoolmngr-spa",
                    ClientName = "Client for SchoolMngr web app.",
                    ClientSecrets = { new Secret("shcoolmngr-secret".ToSha256()) },
                    AllowedGrantTypes = new string[] { GrantType.ClientCredentials, GrantType.ResourceOwnerPassword },

                    AllowedScopes =
                    {
                        OidcConstants.StandardScopes.OpenId,
                        OidcConstants.StandardScopes.Profile,
                        "admin",
                        IdentityServerConstants.LocalApi.ScopeName
                    },

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:4200/signin-oidc" },
                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:4200/signout-callback-oidc" },

                    AllowAccessTokensViaBrowser = true,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                };

                context.Clients.Add(client.ToEntity());
                context.SaveChanges();
            }
            #endregion

            #region MVC Client
            if (!context.Clients.Any(_ => _.ClientId == "backoffice-mvc"))
            {
                var client = new Client
                {
                    ClientId = "backoffice-mvc",
                    ClientSecrets = { new Secret("backoffice-secret".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = false,

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:7180/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:7180/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        OidcConstants.StandardScopes.OpenId,
                        OidcConstants.StandardScopes.Profile,
                        OidcConstants.StandardScopes.Email,
                        "backoffice",
                        IdentityServerConstants.LocalApi.ScopeName
                    },

                    AllowOfflineAccess = true
                };

                context.Clients.Add(client.ToEntity());
                context.SaveChanges();
            }
            #endregion
        }

        public static void SeedPermissions(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDBContext>();

            if (!context.IdentityResources.Any())
            {
                var resources = new List<IdentityResource>
                {
                    new IdentityResource("openid", "Your user identifier", new[] { "sub" }),
                    new IdentityResource("profile", "Your profile data", new[] { "name", "email" }),
                    new IdentityResource("email", "Your email identifier", new[] { "email" }),
                };

                foreach (var resource in resources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }

        public static void SeedResources(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDBContext>();

            if (!context.ApiResources.Any())
            {
                context.ApiResources
                    .Add(new ApiResource("backofficeapi", "Backoffice Api")
                    .ToEntity());

                context.ApiResources
                    .Add(new ApiResource("academeapi", "Academe Api")
                    .ToEntity());

                context.ApiResources
                    .Add(new ApiResource(IdentityServerConstants.LocalApi.ScopeName, "IS4 Local API")
                    .ToEntity());

                context.SaveChanges();
            }
        }
        
        public static void SeedScopes(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDBContext>();

            if (!context.ApiScopes.Any())
            {
                context.ApiScopes
                    .Add(new ApiScope("shcoolmngr:read", "Shcoolmngr Read Only")
                    .ToEntity());

                context.ApiScopes
                    .Add(new ApiScope("shcoolmngr:write", "Shcoolmngr Write Only")
                    .ToEntity());

                context.ApiScopes
                    .Add(new ApiScope("shcoolmngr:full", "Shcoolmngr Read and Write")
                    .ToEntity());

                context.SaveChanges();
            }
        }
        
        public static void SeedUsers(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            var context = serviceScope.ServiceProvider.GetRequiredService<IdentityDBContext>();

            if (!context.Users.Any(_ => _.Email == "dev@admin.com"))
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityAppUser>>();

                var user = new IdentityAppUser("devadmin", "dev@admin.com", "Milton", "Friedman");
                var result = userManager.CreateAsync(user, "Dev321").Result;

                if (result.Succeeded)
                {
                    var roleName = RoleEnum.DEBUG.GetDescription();
                    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                    _ = roleManager.CreateAsync(new ApplicationRole(roleName, "For development porpouse")).Result;

                    user = userManager.FindByNameAsync("devadmin").Result;

                    var roleResul = userManager.AddToRoleAsync(user, roleName).Result;

                }
            }
        }
    }
}