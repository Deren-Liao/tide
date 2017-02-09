using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GoogleCloudAspNet.Startup))]
namespace GoogleCloudAspNet
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }       // StackdriverLogging
    }
}

/*
 * 
 * 
 * StackdriverLogging
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 *         public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }       // StackdriverLogging        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }       // StackdriverLogging        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }       // StackdriverLogging        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }       // StackdriverLogging        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }       // StackdriverLogging        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }       // StackdriverLogging        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }       // StackdriverLogging
 * 
 * 
 * 
 * 
 * 
 */
