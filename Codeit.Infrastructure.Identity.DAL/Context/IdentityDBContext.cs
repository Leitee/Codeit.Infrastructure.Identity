
namespace Codeit.Infrastructure.Identity.DAL.Context
{
    using Codeit.Infrastructure.Identity.Model.Entities;
    using Codeit.NetStdLibrary.Base.Abstractions.Identity;
    using Codeit.NetStdLibrary.Base.Common;
    using Codeit.NetStdLibrary.Base.Identity;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;


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

            #region Identity fields seeding

            builder.Entity<ApplicationRole>().HasData(new List<ApplicationRole>
            {
                new ApplicationRole(RoleEnum.DEBUG.GetDescription(), "Full functionality over app and debugin") { Id = RoleEnum.DEBUG.GetId().ToString() },
                new ApplicationRole(RoleEnum.ADMIN.GetDescription(), "Full permissions and features") { Id = RoleEnum.ADMIN.GetId().ToString() },
                new ApplicationRole(RoleEnum.USER.GetDescription(), "Sensitives features are limited ") { Id = RoleEnum.USER.GetId().ToString() },
                new ApplicationRole(RoleEnum.GUEST.GetDescription(), "Public data accebility only") { Id = RoleEnum.GUEST.GetId().ToString() }
            });

            var user1  = new IdentityAppUser("devadmin", "info@codeitcorp.com", "Jhon", "Doe")
            {
                Id = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PasswordHash = new PasswordHasher<IdentityAppUser>().HashPassword(null, "Dev321"),
                SecurityStamp = string.Empty,
            };
            builder.Entity<IdentityAppUser>().HasData(user1);
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { UserId = user1.Id, RoleId = RoleEnum.DEBUG.GetId().ToString() });

            var user2 = new IdentityAppUser("risanchez", "risanchez@admin.com", "Rick", "Sanchez")
            {
                Id = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PasswordHash = new PasswordHasher<IdentityAppUser>().HashPassword(null, "Rick321"),
                SecurityStamp = string.Empty,
            };
            builder.Entity<IdentityAppUser>().HasData(user2);
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { UserId = user2.Id, RoleId = RoleEnum.ADMIN.GetId().ToString() });

            var user3 = new IdentityAppUser("dabrown", "dabrown@teacher.com", "Dan", "Brown")
            {
                Id = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PasswordHash = new PasswordHasher<IdentityAppUser>().HashPassword(null, "Dan321"),
                SecurityStamp = string.Empty,
            };
            builder.Entity<IdentityAppUser>().HasData(user3);
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { UserId = user3.Id, RoleId = RoleEnum.USER.GetId().ToString() });

            var user4 = new IdentityAppUser("brwayne", "bruce.wayne@student.com", "Bruce", "Wayne")
            {
                Id = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PasswordHash = new PasswordHasher<IdentityAppUser>().HashPassword(null, "Bru321"),
                SecurityStamp = string.Empty,
            };
            builder.Entity<IdentityAppUser>().HasData(user4);
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { UserId = user4.Id, RoleId = RoleEnum.USER.GetId().ToString() });

            var user5 = new IdentityAppUser("anrand", "ayn.rand@student.com", "Ayn", "Rand")
            {
                Id = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PasswordHash = new PasswordHasher<IdentityAppUser>().HashPassword(null, "Ayn321"),
                SecurityStamp = string.Empty,
            };
            builder.Entity<IdentityAppUser>().HasData(user5);
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { UserId = user5.Id, RoleId = RoleEnum.USER.GetId().ToString() });

            var user6 = new IdentityAppUser("mifriedman", "milton.friedman@student.com", "Milton", "Friedman")
            {
                Id = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PasswordHash = new PasswordHasher<IdentityAppUser>().HashPassword(null, "Mil321"),
                SecurityStamp = string.Empty,
            };
            builder.Entity<IdentityAppUser>().HasData(user6);
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { UserId = user6.Id, RoleId = RoleEnum.GUEST.GetId().ToString() });

            #endregion

        }
    }
}
