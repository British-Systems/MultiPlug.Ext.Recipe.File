using MultiPlug.Base.Http;
using MultiPlug.Extension.Core.Attribute;

namespace MultiPlug.Ext.Recipe.File.Controllers.Settings
{
    [Name("Recipe via File")]
    [HttpEndpointType(HttpEndpointType.Settings)]
    [ViewAs(ViewAs.Partial)]
    public class SettingsApp : Controller
    {
    }
}
