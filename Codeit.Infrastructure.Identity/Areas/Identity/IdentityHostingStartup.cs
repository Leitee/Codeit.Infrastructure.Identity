using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Codeit.Infrastructure.Identity.Areas.Identity.IdentityHostingStartup))]
namespace Codeit.Infrastructure.Identity.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                
            });
        }
    }
}