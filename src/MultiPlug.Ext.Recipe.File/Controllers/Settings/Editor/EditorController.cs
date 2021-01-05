using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Ext.Recipe.File.Models;

namespace MultiPlug.Ext.Recipe.File.Controllers.Settings.Editor
{
    [Route("editor/*")]
    public class EditorController : SettingsApp
    {
        public EditorController()
        {
        }

        public Response Get( string theAssembly )
        {
            return new Response
            {
                Model = new EditorModel { Assembly = theAssembly, Json = Core.Instance.Load(theAssembly) },
                Template = "RecipeFileEditor"
            };
        }

        public Response Post(EditorModel theModel)
        {
            Core.Instance.Push(theModel.Assembly, theModel.Json);

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Moved,
                Location = Context.Referrer
            };
        }
    }
}
