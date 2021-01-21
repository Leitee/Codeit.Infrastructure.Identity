/*
* Copyright (c) Akveo 2019. All Rights Reserved.
* Licensed under the Single Application / Multi Application License.
* See LICENSE_SINGLE_APP / LICENSE_MULTI_APP in the ‘docs’ folder for license information on type of purchased license.
*/

using Pandora.Infrastructure.Identity.Model.Dtos;
using Pandora.NetStdLibrary.Base.Abstractions.DomainModel;
using Pandora.NetStdLibrary.Base.DomainModel;

namespace Pandora.Infrastructure.Identity.Model.Entities
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
