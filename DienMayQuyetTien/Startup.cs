using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DienMayQuyetTien.Startup))]
namespace DienMayQuyetTien
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
