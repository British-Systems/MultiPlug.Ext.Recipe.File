using System.Text;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.Recipe.File.Controllers.Settings.SnapShots
{
    [Route("snapshots/download/*")]
    public class SnapShotsDownloadController : SettingsApp
    {
        public Response Get(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return new Response
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            file = file.Replace(".json", string.Empty);

            return new Response
            {
                MediaType = "text/plain",
                RawBytes = Encoding.ASCII.GetBytes(Core.Instance.ReadFile(file))
            };
        }
    }
}
