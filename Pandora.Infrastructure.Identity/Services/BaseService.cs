/*
* Copyright (c) Akveo 2019. All Rights Reserved.
* Licensed under the Single Application / Multi Application License.
* See LICENSE_SINGLE_APP / LICENSE_MULTI_APP in the ‘docs’ folder for license information on type of purchased license.
*/

using Pandora.Infrastructure.Identity.DAL.Context;
using Pandora.Infrastructure.Identity.Interfaces;
using Pandora.Infrastructure.Identity.Model;
using Microsoft.Extensions.Logging;
using PandoraServiceBase = Pandora.NetStdLibrary.Base.BusinessLogic.BaseService;

namespace Pandora.Infrastructure.Identity.Services
{
    public abstract class BaseService : PandoraServiceBase
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
