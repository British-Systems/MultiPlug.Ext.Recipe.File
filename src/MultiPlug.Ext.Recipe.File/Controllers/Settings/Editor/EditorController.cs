using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Ext.Recipe.File.Models;
using MultiPlug.Ext.Recipe.File.Controllers.Settings.SharedRazor;
using MultiPlug.Extension.Core.Exchange;

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
            if( string.IsNullOrEmpty(theAssembly) )
            {
                RecipeItem Item = new RecipeItem();
                Item.Assembly = string.Empty;
                Item.Properties = new Newtonsoft.Json.Linq.JObject();

                return new Response
                {
                    Model = new EditorModel
                    {
                        Selected = string.Empty,
                        Json = Item.ToJson(),
                        Extensions = Core.Instance.ExtensionItems
                    },
                    Template = Templates.Editor
                };

            }
            else
            {
                return new Response
                {
                    Model = new EditorModel
                    {
                        Selected = theAssembly,
                        Json = Core.Instance.Load(theAssembly),
                        Extensions = Core.Instance.ExtensionItems
                    },
                    Template = Templates.Editor
                };
            }
        }

        public Response Post(EditorModel theModel)
        {
            Core.Instance.Replace(theModel.Json);

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Moved,
                Location = Context.Referrer
            };
        }
    }
}
