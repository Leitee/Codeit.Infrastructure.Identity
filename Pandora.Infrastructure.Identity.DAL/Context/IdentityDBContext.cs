using Pandora.Infrastructure.Identity.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pandora.NetStdLibrary.Base.Identity;

namespace Pandora.Infrastructure.Identity.DAL.Context
{

    public class IdentityDBContext : IdentityDbContext<IdentityAppUser, ApplicationRole, string>
    {
        private const string SCHEMA_NAME = "IDENTITY";
        public DbSet<Settings> Settings { get; set; }

        public IdentityDBContext(DbContextOptions<IdentityDBContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //Rename Identity tables
            builder.Entity<IdentityAppUser>().ToTable("Users", SCHEMA_NAME);
            builder.Entity<ApplicationRole>().ToTable("Roles", SCHEMA_NAME);
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", SCHEMA_NAME);
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", SCHEMA_NAME);
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", SCHEMA_NAME).HasKey(key => new { key.UserId, key.RoleId });
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", SCHEMA_NAME).HasKey(key => new { key.ProviderKey, key.LoginProvider });
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", SCHEMA_NAME).HasKey(key => new { key.UserId, key.LoginProvider, key.Name });

            //Data Structure configuration
            builder.Entity<Settings>()
                .ToTable("UserSettings", SCHEMA_NAME)
                .HasKey(s => s.Id)
                .HasName("SettingsID");

            /*
            #region Identity fields seeding

            builder.Entity<ApplicationRole>().HasData(new List<ApplicationRole>
            {
                new ApplicationRole(RolesEnum.DEBUG.GetDescription(), "Full functionality over app and debugin") { Id = RolesEnum.DEBUG.GetId().ToString() },
                new ApplicationRole(RolesEnum.ADMINISTRADOR.GetDescription(), "Full permissions and features") { Id = RolesEnum.ADMINISTRADOR.GetId().ToString() },
                new ApplicationRole(RolesEnum.SUPERVISOR.GetDescription(), "Limited functionality just administrative permissions") { Id = RolesEnum.SUPERVISOR.GetId().ToString() },
                new ApplicationRole(RolesEnum.TEACHER.GetDescription(), "Limited functionality just teaching-relative permissions") { Id = RolesEnum.TEACHER.GetId().ToString() },
                new ApplicationRole(RolesEnum.STUDENT.GetDescription(), "Limited functionality just student-relative permissions") { Id = RolesEnum.STUDENT.GetId().ToString() }
            });

            builder.Entity<IdentityServerUser>().HasData(new IdentityServerUser("devadmin", "info@pandorasistemas.com", "Jhon", "Doe")
            {
                Id = "-1",
                EmailConfirmed = true,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PasswordHash = new PasswordHasher<IdentityServerUser>().HashPassword(null, "Dev321"),
                SecurityStamp = string.Empty,
            });

            builder.Entity<IdentityServerUser>().HasData(new IdentityServerUser("risanchez", "risanchez@admin.com", "Rick", "Sanchez")
            {
                Id = "1",
                EmailConfirmed = true,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PasswordHash = new PasswordHasher<IdentityServerUser>().HashPassword(null, "Rick321"),
                SecurityStamp = string.Empty,
            });

            builder.Entity<IdentityServerUser>().HasData(new IdentityServerUser("dabrown", "dabrown@teacher.com", "Dan", "Brown")
            {
                Id = "11",
                EmailConfirmed = true,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PasswordHash = new PasswordHasher<IdentityServerUser>().HashPassword(null, "Dan321"),
                SecurityStamp = string.Empty,
            });

            builder.Entity<IdentityServerUser>().HasData(new IdentityServerUser("brwayne", "bruce.wayne@student.com", "Bruce", "Wayne")
            {
                Id = "101",
                EmailConfirmed = true,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PasswordHash = new PasswordHasher<IdentityServerUser>().HashPassword(null, "Bru321"),
                SecurityStamp = string.Empty,
            });

            builder.Entity<IdentityServerUser>().HasData(new IdentityServerUser("anrand", "ayn.rand@student.com", "Ayn", "Rand")
            {
                Id = "102",
                EmailConfirmed = true,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PasswordHash = new PasswordHasher<IdentityServerUser>().HashPassword(null, "Ayn321"),
                SecurityStamp = string.Empty,
            });

            builder.Entity<IdentityServerUser>().HasData(new IdentityServerUser("mifriedman", "milton.friedman@student.com", "Milton", "Friedman")
            {
                Id = "103",
                EmailConfirmed = true,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PasswordHash = new PasswordHasher<IdentityServerUser>().HashPassword(null, "Mil321"),
                SecurityStamp = string.Empty,
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { UserId = "-1", RoleId = RolesEnum.DEBUG.GetId().ToString() });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { UserId = "1", RoleId = RolesEnum.ADMINISTRADOR.GetId().ToString() });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { UserId = "11", RoleId = RolesEnum.TEACHER.GetId().ToString() });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { UserId = "101", RoleId = RolesEnum.STUDENT.GetId().ToString() });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { UserId = "102", RoleId = RolesEnum.STUDENT.GetId().ToString() });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { UserId = "103", RoleId = RolesEnum.STUDENT.GetId().ToString() });

            #endregion

            */
        }
    }
}
