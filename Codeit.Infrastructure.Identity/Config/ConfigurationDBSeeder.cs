using Codeit.Infrastructure.Identity.DAL.Context;
using Codeit.Infrastructure.Identity.Model.Entities;
using Codeit.NetStdLibrary.Base.Abstractions.Identity;
using Codeit.NetStdLibrary.Base.Common;
using Codeit.NetStdLibrary.Base.Identity;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Codeit.Infrastructure.Identity.Config
{
    public static class ConfigurationDBSeeder
    {
        public static void RunMigration(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDBContext>().Database.Migrate();
            serviceScope.ServiceProvider.GetRequiredService<ConfigurationDBContext>().Database.Migrate();
            serviceScope.ServiceProvider.GetRequiredService<IdentityDBContext>().Database.Migrate();
        }

        public static void SeedClient(this IApplicationBuilder app)
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

        public static void SeedResources(this IApplicationBuilder app)
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

        public static void SeedResource(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDBContext>();

            if (!context.ApiResources.Any())
            {
                var catalogRes = new ApiResource("catalog", "Catalog API");
                context.ApiResources.Add(catalogRes.ToEntity());

                var accountsRes = new ApiResource("accounts", "Account API");
                context.ApiResources.Add(accountsRes.ToEntity());

                var localRes = new ApiResource(IdentityServerConstants.LocalApi.ScopeName, "IS4 Local API");
                context.ApiResources.Add(localRes.ToEntity());

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                var apiResource = new ApiScope("shcoolmngr", "Shcoolmngr Services");
                context.ApiScopes.Add(apiResource.ToEntity());
                context.SaveChanges();
            }
        }

        public static void SeedUser(this IApplicationBuilder app)
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