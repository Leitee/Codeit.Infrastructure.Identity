using Pandora.Infrastructure.Identity.Interfaces;
using Pandora.Infrastructure.Identity.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandora.Infrastructure.Identity.Services
{
    public class ContextProvider : IContextProvider
    {
        public ContextSession GetCurrentContext()
        {
            //var currentUserId = HttpContext.Current.User.Identity.GetUserId<int>();
            return new ContextSession { UserId = Guid.NewGuid().ToString() };
        }
    }
}
