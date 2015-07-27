using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    /// <summary>
    /// NewsApi 的摘要说明
    /// </summary>
    public class NewsApi : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var request = context.Request;           
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}