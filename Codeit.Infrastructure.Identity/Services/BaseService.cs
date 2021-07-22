/*
* Copyright (c) Akveo 2019. All Rights Reserved.
* Licensed under the Single Application / Multi Application License.
* See LICENSE_SINGLE_APP / LICENSE_MULTI_APP in the ‘docs’ folder for license information on type of purchased license.
*/

using Codeit.Infrastructure.Identity.DAL.Context;
using Codeit.Infrastructure.Identity.Interfaces;
using Codeit.Infrastructure.Identity.Model;
using Microsoft.Extensions.Logging;
using CodeitServiceBase = Codeit.NetStdLibrary.Base.BusinessLogic.BaseService;

namespace Codeit.Infrastructure.Identity.Services
{
    public abstract class BaseService : CodeitServiceBase
    {
        protected readonly ContextSession _session;
        protected readonly IdentityDBContext _context;

        protected BaseService(IContextProvider contextProvider, ILoggerFactory loggerFactory, IdentityDBContext context)
            : base(loggerFactory)
        {
            _session = contextProvider.GetCurrentContext();
            _context = context;
        }
    }
}
