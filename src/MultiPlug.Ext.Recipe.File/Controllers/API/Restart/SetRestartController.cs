using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.Recipe.File.Controllers.API.Restart
{
    [Route("setrestart")]
    public class SetRestartController : APIEndpoint
    {
        public Response Post()
        {
            Core.Instance.RebootUserPrompt = true;

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Location = Context.Referrer
            };
        }
    }
}
