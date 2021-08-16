using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(PAD.Areas.Identity.IdentityHostingStartup))]
namespace PAD.Areas.Identity
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