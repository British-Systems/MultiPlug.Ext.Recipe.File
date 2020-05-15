using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Ext.Recipe.File.Models;

namespace MultiPlug.Ext.Recipe.File.Controllers.Settings.Home
{
    [Route("")]
    class HomeController : Controller
    {
        public HomeController()
        {
        }

        public Response Get()
        {
            return new Response
            {
                Model = new SettingsModel
                {
                    ExtensionNames = Core.Instance.ExtensionNames,
                    FilePath = Core.Instance.FilePath,
                    LastRead = Core.Instance.LastRead,
                    LastWrite = Core.Instance.LastWrite
                },
                Template = "SimpleFileConfiguratorSettingsView"
            };
        }
    }
}
