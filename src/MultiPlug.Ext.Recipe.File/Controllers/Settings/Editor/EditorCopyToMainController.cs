using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Ext.Recipe.File.Models;

namespace MultiPlug.Ext.Recipe.File.Controllers.Settings.Editor
{
    [Route("editor/copytomain")]
    public class EditorCopyToMainController : SettingsApp
    {
        public EditorCopyToMainController()
        {
        }

        public Response Post(EditorModel theModel)
        {
            Core.Instance.Replace(Core.c_MainFile, theModel.Json);

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
