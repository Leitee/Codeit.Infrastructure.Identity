
using Codeit.Infrastructure.Identity.Model.Dtos;
using Codeit.NetStdLibrary.Base.Abstractions.DomainModel;

namespace Codeit.Infrastructure.Identity.Model.Entities
{
    public class Settings : IEntity<int>
    {
        public int Id { get; set; }
        public string ThemeName { get; set; }

        public Settings()
        {

        }

        public Settings(SettingsDTO dto)
        {
            this.Update(themeName: dto.ThemeName);
        }

        public void Update(string themeName)
        {
            ThemeName = themeName;
        }

        public SettingsDTO ConvertToDto()
        {
            return new SettingsDTO
            {
                Id = this.Id,
                ThemeName = this.ThemeName
            };
        }
    }
}
