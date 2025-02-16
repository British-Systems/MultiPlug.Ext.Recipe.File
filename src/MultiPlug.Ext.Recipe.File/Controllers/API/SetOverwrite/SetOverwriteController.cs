using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.Recipe.File.Controllers.API.SetOverwrite
{
    [Route("setoverwrite")]
    public class SetOverwriteController : APIEndpoint
    {
        public Response Post(string extension, bool overwrite)
        {
            Core.Instance.SetOverwrite(extension, overwrite);

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Location = Context.Referrer
            };
        }
    }
}
