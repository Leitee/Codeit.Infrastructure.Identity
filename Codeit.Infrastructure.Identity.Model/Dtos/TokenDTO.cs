/*
* Copyright (c) Akveo 2019. All Rights Reserved.
* Licensed under the Single Application / Multi Application License.
* See LICENSE_SINGLE_APP / LICENSE_MULTI_APP in the ‘docs’ folder for license information on type of purchased license.
*/

using Codeit.NetStdLibrary.Base.Abstractions.DomainModel;

namespace Codeit.Infrastructure.Identity.Model.Dtos
{
    public class TokenDTO : IDto
    {
        public double Expires_in;

        public string Access_token;

        public string Refresh_token;
    }
}
