using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Ext.Recipe.File.Controllers.Settings.SharedRazor;
using MultiPlug.Ext.Recipe.File.Models;

namespace MultiPlug.Ext.Recipe.File.Controllers.Settings.Home
{
    [Route("")]
    public class HomeController : SettingsApp
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
                    RebootUserPrompt = Core.Instance.RebootUserPrompt,
                    Extensions = Core.Instance.ExtensionItems,
                    FilePath = Core.Instance.FilePath,
                    LastRead = Core.Instance.LastRead,
                    LastWrite = Core.Instance.LastWrite
                },
                Template = Templates.Home
            };
        }
    }
}
