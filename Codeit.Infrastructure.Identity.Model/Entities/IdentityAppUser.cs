/// <summary>
/// 
/// </summary>
namespace Codeit.Infrastructure.Identity.Model.Entities
{
    using Codeit.Infrastructure.Identity.Model.Dtos;
    using Codeit.Enterprise.Base.Identity;
    using System;

    public class IdentityAppUser : ApplicationUser
    {
        public string ProfileId { get; private set; }
        public int? SettingsId { get; private set; }
        public virtual Settings Settings { get; private set; }
        
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
