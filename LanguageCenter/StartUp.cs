using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LanguageCenter.App_Start.StartUp))]
namespace LanguageCenter.App_Start
{
    public partial class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }


    }
}