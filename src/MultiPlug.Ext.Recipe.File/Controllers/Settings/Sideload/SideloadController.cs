using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Ext.Recipe.File.Controllers.Settings.SharedRazor;
using MultiPlug.Ext.Recipe.File.Models;

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
                Model = new SideloadModel
                {
                    RebootUserPrompt = Core.Instance.RebootUserPrompt,
                    SnapShots = Core.Instance.GetSnapShots()
                },
                Template = Templates.Sideload
            };
        }

        public Response Post(UploadFilePaths theFiles, string into)
        {
            if (theFiles.Files.Length > 0)
            {
                var json = System.IO.File.ReadAllText(theFiles.Files[0].Path);

                Core.Instance.Replace(string.IsNullOrEmpty(into) ? Core.c_MainFile : into, json);

                if((!string.IsNullOrEmpty(into)) && into == Core.c_MainFile)
                {
                    Core.Instance.SetOverwriteAll(false);
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
