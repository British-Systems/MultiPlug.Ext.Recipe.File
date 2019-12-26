using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Ext.Recipe.File.Models;

namespace MultiPlug.Ext.Recipe.File.Views
{
    [Route("")]
    class HomeController : Controller
    {
        readonly SettingsModel m_Model;

        public HomeController(SettingsModel theModel)
        {
            m_Model = theModel;
        }

        public Response Get()
        {
            return new Response
            {
                Model = m_Model,
                Template = "SimpleFileConfiguratorSettingsView"
            };
        }
    }
}
