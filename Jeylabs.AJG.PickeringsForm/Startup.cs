using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Jeylabs.AJG.PickeringsForm.Startup))]
namespace Jeylabs.AJG.PickeringsForm
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
