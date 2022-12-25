using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SMIS.Startup))]
namespace SMIS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }


    }
}
