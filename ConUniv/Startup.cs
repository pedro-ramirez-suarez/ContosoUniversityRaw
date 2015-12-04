using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ConUniv.Startup))]
namespace ConUniv
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
