using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.Recipe.File.Controllers.Settings.SnapShots
{
    [Route("snapshots/delete/*")]
    public class SnapShotsDeleteController : SettingsApp
    {
        public SnapShotsDeleteController()
        {
        }

        public Response Post(string FileName)
        {
            if(Core.Instance.DeleteSnapShot(FileName))
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
