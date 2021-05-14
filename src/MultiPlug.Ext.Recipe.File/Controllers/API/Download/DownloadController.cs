﻿using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using System.Text;

namespace MultiPlug.Ext.Recipe.File.Controllers.API.Download
{
    [Route("download/*")]
    public class DownloadController : APIEndpoint
    {
        public Response Get(string id)
        {
  
            int LastIndexOfDot = id.LastIndexOf('.');
            string lhs = LastIndexOfDot < 0 ? id : id.Substring(0, LastIndexOfDot), rhs = LastIndexOfDot < 0 ? "" : id.Substring(LastIndexOfDot + 1);



            string Json = Core.Instance.Load(lhs);

            return new Response
            {
                MediaType = "text/plain",
                RawBytes = Encoding.ASCII.GetBytes(Json)
            };
        }


    }
}
