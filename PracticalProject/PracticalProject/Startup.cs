using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PracticalProject.Startup))]
namespace PracticalProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
