using System.Collections.Generic;

using MultiPlug.Base.Http;
using MultiPlug.Extension.Core.Http;
using MultiPlug.Extension.Core.Attribute;

using MultiPlug.Ext.Recipe.File.Models;
using MultiPlug.Ext.Recipe.File.Views;

namespace MultiPlug.Ext.Recipe.File
{
    [Name("Simple File Configurator")]
    [HttpEndpointType(HttpEndpointType.Settings)]
    [ViewAs(ViewAs.Partial)]
    public class CSettingsView : HttpEndpoint
    {
        readonly SettingsModel m_SettingsModel;
        readonly Controller[] m_Controllers;

        public CSettingsView( SettingsModel model )
        {
            m_SettingsModel = model;
            m_Controllers = new Controller[]
            {
                new HomeController(model)
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
