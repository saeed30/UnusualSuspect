[assembly: HostingStartup(typeof(UnusualSuspect.Admin.Areas.Identity.IdentityHostingStartup))]
namespace UnusualSuspect.Admin.Areas.Identity;
public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) => {
        });
    }
}
