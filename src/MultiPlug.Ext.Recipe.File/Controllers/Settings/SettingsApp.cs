using System.Collections.Generic;

using MultiPlug.Base.Http;
using MultiPlug.Extension.Core.Http;
using MultiPlug.Extension.Core.Attribute;

using MultiPlug.Ext.Recipe.File.Models;
using MultiPlug.Ext.Recipe.File.Controllers.Settings.Home;
using MultiPlug.Ext.Recipe.File.Controllers.Settings.Editor;

namespace MultiPlug.Ext.Recipe.File.Controllers.Settings
{
    [Name("Simple File Configurator")]
    [HttpEndpointType(HttpEndpointType.Settings)]
    [ViewAs(ViewAs.Partial)]
    public class SettingsApp : HttpEndpoint
    {
        readonly Controller[] m_Controllers;

        public SettingsApp()
        {
            m_Controllers = new Controller[]
            {
                new HomeController(),
                new EditorController()
            };
        }

        public override IEnumerable<Controller> Controllers
        {
            get
            {
                return m_Controllers;
            }
        }
    }
}
