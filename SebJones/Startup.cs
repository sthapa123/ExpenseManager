using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SebJones.Startup))]
namespace SebJones
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
