/// <summary>
/// 
/// </summary>
namespace Codeit.Infrastructure.Identity.Model.Entities
{
    using Codeit.Infrastructure.Identity.Model.Dtos;
    using Codeit.NetStdLibrary.Base.Identity;
    using System;

    public class IdentityAppUser : ApplicationUser
    {
        public string ProfileId { get; private set; }
        public int? SettingsId { get; private set; }
        public virtual Settings Settings { get; private set; }
        

        #region Override Base Properties
        public new string Id { get => base.Id; private set => base.Id = value; }
        public new string UserName { get => base.UserName; private set => base.UserName = value; }
        public new string NormalizedUserName { get => base.NormalizedUserName; private set => base.NormalizedUserName = value; }
        public new string Email { get => base.Email; private set => base.Email = value; }
        public new string NormalizedEmail { get => base.NormalizedEmail; private set => base.NormalizedEmail = value; }
        public new bool EmailConfirmed { get => base.EmailConfirmed; private set => base.EmailConfirmed = value; }
        public new string PasswordHash { get => base.PasswordHash; private set => base.PasswordHash = value; }
        public new string SecurityStamp { get => base.SecurityStamp; private set => base.SecurityStamp = value; }
        public new string ConcurrencyStamp { get => base.ConcurrencyStamp; private set => base.ConcurrencyStamp = value; }
        public new string PhoneNumber { get => base.PhoneNumber; private set => base.PhoneNumber = value; }
        public new bool PhoneNumberConfirmed { get => base.PhoneNumberConfirmed; private set => base.PhoneNumberConfirmed = value; }
        public new bool TwoFactorEnabled { get => base.TwoFactorEnabled; private set => base.TwoFactorEnabled = value; }
        public new DateTimeOffset? LockoutEnd { get => base.LockoutEnd; private set => base.LockoutEnd = value; }
        public new bool LockoutEnabled { get => base.LockoutEnabled; private set => base.LockoutEnabled = value; }
        public new int AccessFailedCount { get => base.AccessFailedCount; private set => base.AccessFailedCount = value; }

        public new string FirstName { get => base.FirstName; private set => base.FirstName = value; }
        public new string LastName { get => base.LastName; private set => base.LastName = value; }
        public new DateTime JoinDate { get => base.JoinDate; private set => base.JoinDate = value; }
        public new bool Deleted { get => base.Deleted; private set => base.Deleted = value; }
        #endregion

        public IdentityAppUser()
            : base()
        {

        }

        public IdentityAppUser(string pUsername, string pEmail, string pFirstName = default, string pLastName = default, string profileId = default)
            : base(pUsername, pEmail, pFirstName, pLastName)
        {
            ProfileId = profileId;
        }

        public IdentityAppUser(UserDTO dto)
            : this(pUsername: GenerateUsername(dto.Email), dto.Email, dto.FirstName, dto.LastName, dto.ProfileId)
        {
            
        }

        public void Update(string pFirstName, string pLastName, string pProfileId, int pSettingsId)
        {
            this.FirstName = pFirstName;
            this.LastName = pLastName;
            this.ProfileId = pProfileId;
            this.SettingsId = pSettingsId;
        }

        public void Delete()
        {
            this.Deleted = true;
        }

        public UserDTO ConvertToDto()
        {
            return new UserDTO
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                FullName = this.FullName,
                Email = this.Email,
                ProfileId = this.ProfileId,
                Settings = this.Settings?.ConvertToDto()
            };
        }

        public static string GenerateUsername(string email)
        {
            var rnd = new Random();
            var split = email.Split('@');
            return $"{split[0]}{rnd.Next(0,10)}";
        }
    }
}
