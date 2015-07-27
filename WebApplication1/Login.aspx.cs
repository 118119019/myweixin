using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string cookie = HttpContext.Current.Request.Cookies["panda_sessionid"].Value;
                if ("" != cookie)
                {
                    if (cookie == WebConfigurationManager.AppSettings["WeixinAgentWeiweihiKey"])
                    {
                        //重新定向到首页
                        Response.Write("<script>window.top.location='Main.aspx';</script>");
                    }
                }
            }
            catch
            {
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!CheckValid())
            {
                return;
            }
            if (txtUsername.Text.Trim() == "longyan" && txtPassword.Text.Trim() == "longyan")
            {

                HttpCookie cookie = new HttpCookie("panda_sessionid",
                    WebConfigurationManager.AppSettings["WeixinAgentWeiweihiKey"]);
                cookie.Expires = DateTime.Now.AddMinutes(30);
                Response.Cookies.Add(cookie);
                Response.Write("<script>window.top.location='Main.aspx';</script>");
            }
        }

        private bool CheckValid()
        {
            if (txtUsername.Text.Trim() == "")
            {
                lbError.Text = "请输入用户名！";
                return false;
            }
            if (txtPassword.Text.Trim() == "")
            {
                lbError.Text = "请输入密码！";
                return false;
            }
            return true;
        }
    }
}