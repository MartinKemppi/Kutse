using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kutsung.Startup))]
namespace Kutsung
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
