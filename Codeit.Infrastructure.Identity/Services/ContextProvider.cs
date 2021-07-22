using Codeit.Infrastructure.Identity.Interfaces;
using Codeit.Infrastructure.Identity.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Codeit.Infrastructure.Identity.Services
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
