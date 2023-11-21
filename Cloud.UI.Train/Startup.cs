using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cloud.UI.Train.Startup))]
namespace Cloud.UI.Train
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
