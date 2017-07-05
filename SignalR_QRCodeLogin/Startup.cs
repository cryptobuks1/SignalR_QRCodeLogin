using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SignalR_QRCodeLogin.Startup))]
namespace SignalR_QRCodeLogin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
