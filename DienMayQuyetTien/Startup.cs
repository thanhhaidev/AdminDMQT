using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(dienmayquyettien.Startup))]
namespace dienmayquyettien
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
