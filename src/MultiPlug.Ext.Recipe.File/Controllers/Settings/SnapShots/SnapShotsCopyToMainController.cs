using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.Recipe.File.Controllers.Settings.SnapShots
{
    [Route("snapshots/copytomain/*")]
    public class SnapShotsCopyToMainController : SettingsApp
    {
        public SnapShotsCopyToMainController()
        {
        }

        public Response Post(string FileName)
        {
            if(Core.Instance.CopyToMainRecipe(FileName))
            {
                return new Response
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            else
            {
                return new Response
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }
        }
    }
}
