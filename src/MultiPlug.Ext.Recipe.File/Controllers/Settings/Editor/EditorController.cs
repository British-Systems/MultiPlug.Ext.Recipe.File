using System.Linq;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Ext.Recipe.File.Models;
using MultiPlug.Ext.Recipe.File.Controllers.Settings.SharedRazor;
using MultiPlug.Extension.Core.Exchange;

namespace MultiPlug.Ext.Recipe.File.Controllers.Settings.Editor
{
    [Route("editor/")]
    public class EditorController : SettingsApp
    {
        public EditorController()
        {
        }

        public Response Get( string file, string extension )
        {
            var Json = Core.Instance.ReadFile(string.IsNullOrEmpty(file) ? Core.c_MainFile : file);

            RecipeCollection RecipeCollection = Extension.Core.Exchange.Recipe.ToObject(Json);

            string[] Extensions = new string[0];

            if(RecipeCollection.Extensions != null)
            {
                Extensions = RecipeCollection.Extensions.Select(e => e.Assembly).ToArray();
            }


            if ( string.IsNullOrEmpty(extension) )
            {
                RecipeItem Item = new RecipeItem();
                Item.Assembly = string.Empty;
                Item.Properties = new Newtonsoft.Json.Linq.JObject();

                return new Response
                {
                    Model = new EditorModel
                    {
                        RebootUserPrompt = Core.Instance.RebootUserPrompt,
                        SelectedExtension = string.Empty,
                        Json = Item.ToJson(),
                        Extensions = Extensions,
                        SnapShots = Core.Instance.GetSnapShots(),
                        SelectedFile = string.IsNullOrEmpty(file) ? Core.c_MainFile : file
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
                        RebootUserPrompt = Core.Instance.RebootUserPrompt,
                        SelectedExtension = extension,
                        Json = Core.Instance.GetJson(RecipeCollection, extension),
                        Extensions = Extensions,
                        SnapShots = Core.Instance.GetSnapShots(),
                        SelectedFile = string.IsNullOrEmpty(file) ? Core.c_MainFile : file
                    },
                    Template = Templates.Editor
                };
            }
        }

        public Response Post(EditorModel theModel)
        {
            var Recipe = Core.Instance.Replace(theModel.SelectedFile, theModel.Json);

            if(theModel.SelectedFile == Core.c_MainFile)
            {
                if(Recipe != null && Recipe.Extensions != null && Recipe.Extensions.Any())
                {
                    foreach(var Extension in Recipe.Extensions)
                    {
                        Core.Instance.SetOverwrite(Extension.Assembly, false);
                    }

                    Core.Instance.RebootUserPrompt = true;
                }
            }

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Moved,
                Location = Context.Referrer
            };
        }
    }
}
