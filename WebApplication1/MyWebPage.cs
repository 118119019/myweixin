using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WebApplication1
{
    public class MyWebPage : System.Web.UI.Page
    {
        protected void RedirectUrl(string urlParam)
        {

            string uri = string.Format("http://{0}{1}", Request.Url.Host,
                   Request.Url.Port == 80 ? "" : ":" + Request.Url.Port.ToString());

            string url = WebConfigurationManager.AppSettings["domain"] + "/" + urlParam;
            Response.Redirect(url, true);
        }
    }
}