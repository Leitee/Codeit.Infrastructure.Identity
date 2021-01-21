/*
* Copyright (c) Akveo 2019. All Rights Reserved.
* Licensed under the Single Application / Multi Application License.
* See LICENSE_SINGLE_APP / LICENSE_MULTI_APP in the ‘docs’ folder for license information on type of purchased license.
*/

using Pandora.NetStdLibrary.Base.Abstractions.DomainModel;

namespace Pandora.Infrastructure.Identity.Model.Dtos
{
    public class UserDTO : IDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }

        public string ProfileId { get; set; }
        public SettingsDTO Settings { get; set; }
    }
}
