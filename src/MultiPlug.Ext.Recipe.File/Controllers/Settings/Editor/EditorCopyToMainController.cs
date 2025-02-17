using System.Linq;
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
            var Recipe = Core.Instance.Replace(Core.c_MainFile, theModel.Json);

            if (Recipe != null && Recipe.Extensions != null && Recipe.Extensions.Any())
            {
                foreach (var Extension in Recipe.Extensions)
                {
                    Core.Instance.SetOverwrite(Extension.Assembly, false);
                }

                Core.Instance.RebootUserPrompt = true;
            }

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
