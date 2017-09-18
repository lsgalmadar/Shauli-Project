using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShauliBlogProject.Startup))]
namespace ShauliBlogProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
