using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lab4Auth.Startup))]
namespace Lab4Auth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
