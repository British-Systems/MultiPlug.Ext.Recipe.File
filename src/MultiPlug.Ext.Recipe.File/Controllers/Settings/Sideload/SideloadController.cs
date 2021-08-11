using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Ext.Recipe.File.Controllers.Settings.SharedRazor;

namespace MultiPlug.Ext.Recipe.File.Controllers.Settings.Sideload
{
    [Route("sideload")]
    public class SideloadController : SettingsApp
    {
        public SideloadController()
        {
        }

        public Response Get()
        {

            return new Response
            {
                Model = null,
                Template = Templates.Sideload
            };
        }

        public Response Post(UploadFilePaths theFiles)
        {
            if (theFiles.Files.Length > 0)
            {
                var json = System.IO.File.ReadAllText(theFiles.Files[0].Path);

                Core.Instance.Replace(json);
            }

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Moved,
                Location = Context.Referrer
            };
        }
    }
}
