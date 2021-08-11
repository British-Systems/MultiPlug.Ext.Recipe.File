using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.Recipe.File.Controllers.API.SetSave
{
    [Route("setsave")]
    public class SetSaveController : APIEndpoint
    {
        public Response Post(string extension, bool save)
        {
            Core.Instance.SetSave(extension, save);

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Location = Context.Referrer
            };
        }
    }
}
