using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Pandora.Infrastructure.Identity.Areas.Identity.IdentityHostingStartup))]
namespace Pandora.Infrastructure.Identity.Areas.Identity
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