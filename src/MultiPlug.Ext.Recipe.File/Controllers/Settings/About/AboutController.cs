using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Ext.Recipe.File.Controllers.Settings.SharedRazor;
using System.Reflection;

namespace MultiPlug.Ext.Recipe.File.Controllers.Settings.About
{
    [Route("about")]
    public class AboutController : SettingsApp
    {
        public AboutController()
        {

        }

        public Response Get()
        {
            Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();

            return new Response
            {
                Template = Templates.About,
                Model = new Models.AboutModel
                {
                    RebootUserPrompt = Core.Instance.RebootUserPrompt,
                    Title = ExecutingAssembly.GetCustomAttribute<AssemblyTitleAttribute>().Title,
                    Description = ExecutingAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description,
                    Company = ExecutingAssembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company,
                    Product = ExecutingAssembly.GetCustomAttribute<AssemblyProductAttribute>().Product,
                    Copyright = ExecutingAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright,
                    Trademark = ExecutingAssembly.GetCustomAttribute<AssemblyTrademarkAttribute>().Trademark,
                    Version = ExecutingAssembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version,
                    RecipeFileLocation = Core.Instance.FilePath
                }
            };
        }
    }
}
