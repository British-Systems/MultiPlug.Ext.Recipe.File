using System.Text;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.Recipe.File.Controllers.API.Push
{
    [Route("push/*")]
    public class PushController : APIEndpoint
    {
        public Response Get(string id)
        {
            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Moved,
                Location = Context.Referrer
            };
        }


        public Response Post(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new Response
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            if (id.Equals("recipe", System.StringComparison.OrdinalIgnoreCase))
            {
                Core.Instance.SetOverwriteAll(true);
                Core.Instance.RebootUserPrompt = false;
                Core.Instance.PushRecipe();

                return new Response
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Location = Context.Referrer
                };
            }
            else
            {
                Core.Instance.LoadSingle(id);

                return new Response
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Location = Context.Referrer
                };
            }
        }
    }
}
