using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AspNetWebSha.Startup))]
namespace AspNetWebSha
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
