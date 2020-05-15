using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Ext.Recipe.File.Models;
using System;

namespace MultiPlug.Ext.Recipe.File.Controllers.Settings.Editor
{
    [Route("editor/*")]
    class EditorController : Controller
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
