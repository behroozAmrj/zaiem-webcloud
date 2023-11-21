using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cloud.IU.WEB.Startup))]
namespace Cloud.IU.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

    }
}
