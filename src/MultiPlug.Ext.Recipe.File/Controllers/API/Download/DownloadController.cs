using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using System.Text;

namespace MultiPlug.Ext.Recipe.File.Controllers.API.Download
{
    [Route("download/*")]
    public class DownloadController : APIEndpoint
    {
        public Response Get(string id)
        {
            if( string.IsNullOrEmpty(id) )
            {
                return new Response
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            if( id.Equals("recipe.json", System.StringComparison.OrdinalIgnoreCase))
            {
                return new Response
                {
                    MediaType = "text/plain",
                    RawBytes = Encoding.ASCII.GetBytes(Core.Instance.ReadFile())
                };
            }
            else
            {
                int LastIndexOfDot = id.LastIndexOf('.');
                string lhs = LastIndexOfDot < 0 ? id : id.Substring(0, LastIndexOfDot), rhs = LastIndexOfDot < 0 ? "" : id.Substring(LastIndexOfDot + 1);
                return new Response
                {
                    MediaType = "text/plain",
                    RawBytes = Encoding.ASCII.GetBytes(Core.Instance.Load(lhs))
                };
            }
        }
    }
}
