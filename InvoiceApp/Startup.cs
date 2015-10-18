using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InvoiceApp.Startup))]
namespace InvoiceApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
