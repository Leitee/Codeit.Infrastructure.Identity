
using Codeit.Infrastructure.Identity.Config;
using Codeit.Infrastructure.Identity.DAL;
using Codeit.Infrastructure.Identity.DAL.Context;
using Codeit.Infrastructure.Identity.Interfaces;
using Codeit.Infrastructure.Identity.Services;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;

namespace Codeit.Infrastructure.Identity
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly AppSettings _settings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _settings = AppSettings.GetSettings(Configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<AppSettings>(Configuration);


            //var settings = new IdentitySettings();
            //this.Configuration.GetSection(WorkflowSettings.ConfigurationKey).Bind(_settings);

            //services.AddSingleton<AppSettings>((sp) => _settings);


            var efPersistenceBuilder = EFPersistenceBuilder.Build(_settings.DalSection);

            services
                .AddPersistenceTier("DalSection")
                .AddControllersWithViews();

            services.AddHealthChecks()
                .AddCheck("Self", _ => HealthCheckResult.Healthy());

            services.AddCors(options =>
            {
                options.AddPolicy("AllOrigins", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services
                .AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                }); /*https://docs.microsoft.com/en-us/aspnet/core/security/authorization/razor-pages-authorization?view=aspnetcore-3.1 */


            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.EmitStaticAudienceClaim = true; // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                    options.UserInteraction.LoginUrl = "/Account/Login";
                    options.UserInteraction.LogoutUrl = "/Account/Logout";
                    options.UserInteraction.ErrorUrl = "/Home/Error";
                    options.Authentication = new AuthenticationOptions()
                    {
                        CookieLifetime = TimeSpan.FromHours(10), // ID server cookie timeout set to 10 hours
                        CookieSlidingExpiration = true
                    };
                })
                .AddConfigurationStore<ConfigurationDBContext>(efPersistenceBuilder.ConfigureGrantsStore)
                .AddOperationalStore<PersistedGrantDBContext>(efPersistenceBuilder.ConfigureOperationalStore)
                .AddAspNetIdentity<Model.Entities.IdentityAppUser>();

            // not recommended for production - you need to store your key material somewhere secure
            services
                .AddIf(_settings.IsDevelopment, _ => builder.AddDeveloperSigningCredential().Services)
                .AddIf(_settings.IsDevelopment, sv => sv.AddDatabaseDeveloperPageExceptionFilter());

            services.AddAuthentication()
                .AddCookie(options =>
                {
                    // add an instance of the patched manager to the options:
                    options.CookieManager = new ChunkingCookieManager();

                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Unspecified;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                })
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = "<insert here>";
                    options.ClientSecret = "<insert here>";
                })
                .AddOpenIdConnect("oidc", "Demo IdentityServer", options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                    options.SaveTokens = true;

                    options.Authority = "https://demo.identityserver.io/";
                    options.ClientId = "interactive.confidential";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                });

            services.AddLocalApiAuthentication();
            services.AddTransient<IEmailSender, EmailSenderClient>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IContextProvider, ContextProvider>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseIf(_settings.IsDevelopment, _ => IdentityModelEventSource.ShowPII = true);
            app.UseIf(_settings.IsDevelopment, app => app.UseDeveloperExceptionPage());
            app.UseIf(_settings.IsDevelopment, app => app.UseMigrationsEndPoint());

            app.UseIf(_settings.IsDevelopment, app =>
            {

                app.TryMigrateAndSeed();
                app.SeedClients();
                app.SeedPermissions();
                app.SeedResources();
                app.SeedScopes();
                app.SeedUsers();

                return app;
            });

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();

            app.UseCors("AllOrigins");
            app.UseIdentityServer();

            // This will make the HTTP requests log as rich logs instead of plain text.
            app.UseSerilogRequestLogging();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc");
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
