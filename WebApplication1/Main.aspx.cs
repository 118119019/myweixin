using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Main : BaseAuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnQuit_Click(object sender, EventArgs e)
        {
            try
            {
                string cookie = HttpContext.Current.Request.Cookies["panda_sessionid"].Value;
                HttpCookie cookies = new HttpCookie("panda_sessionid", "");
                HttpContext.Current.Response.Cookies.Add(cookies);
            }
            catch
            {

            }
            Response.Redirect("Login.aspx");
        }
    }
}