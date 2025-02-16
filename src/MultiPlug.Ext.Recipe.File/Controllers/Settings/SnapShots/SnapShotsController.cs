using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Ext.Recipe.File.Controllers.Settings.SharedRazor;
using MultiPlug.Ext.Recipe.File.Models;

namespace MultiPlug.Ext.Recipe.File.Controllers.Settings.SnapShot
{
    [Route("snapshots")]
    public class SnapShotsController : SettingsApp
    {
        public SnapShotsController()
        {
        }

        public Response Get()
        {

            return new Response
            {
                Model = new SnapShotsModel
                {
                    RebootUserPrompt = Core.Instance.RebootUserPrompt,
                    SnapShots = Core.Instance.GetSnapShots()
                },
                Template = Templates.SnapShots
            };
        }

        public Response Post(string NewSnapshotFileName)
        {
            Core.Instance.CreateSnapShot(NewSnapshotFileName);

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Moved,
                Location = Context.Referrer
            };
        }
    }
}
