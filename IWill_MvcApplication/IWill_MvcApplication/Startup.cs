using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IWill_MvcApplication.Startup))]
namespace IWill_MvcApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
