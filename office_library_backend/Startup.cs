using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(office_library_backend.Startup))]
namespace office_library_backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
