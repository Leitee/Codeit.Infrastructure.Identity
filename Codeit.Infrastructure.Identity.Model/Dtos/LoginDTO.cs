﻿/*
* Copyright (c) Akveo 2019. All Rights Reserved.
* Licensed under the Single Application / Multi Application License.
* See LICENSE_SINGLE_APP / LICENSE_MULTI_APP in the ‘docs’ folder for license information on type of purchased license.
*/

using Codeit.Enterprise.Base.Abstractions.BusinessLogic;

namespace Codeit.Infrastructure.Identity.Model.Dtos
{
    public class LoginDTO : IDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}