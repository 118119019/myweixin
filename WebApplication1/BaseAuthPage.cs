using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;

namespace WebApplication1
{
    public class BaseAuthPage : Page
    {
        protected bool IsAuth = true;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //必须添加domain，否则会报错
            Auth();

        }
        protected override void OnLoad(EventArgs e)
        {
            if (IsAuth)
            {
                base.OnLoad(e);
            }
        }
        protected void Auth()
        {
            try
            {
                string cookie = HttpContext.Current.Request.Cookies["panda_sessionid"].Value;
                if (!string.IsNullOrEmpty(cookie))
                {

                    TimeSpan span = new TimeSpan(0, 30, 0);
                    if (cookie != WebConfigurationManager.AppSettings["WeixinAgentWeiweihiKey"])
                    {
                        IsAuth = false;
                        Response.Write("<script>window.top.location='Login.aspx';</script>");
                        //重新定向到没有授权的页面
                        return;
                    }
                    else
                    {
                        IsAuth = true;
                    }
                    HttpCookie cookies = new HttpCookie("panda_sessionid", cookie);
                    cookies.Expires = DateTime.Now.Add(span);
                    Response.Cookies.Add(cookies);
                }
                else
                {
                    IsAuth = false;
                    Response.Write("<script>window.top.location='Login.aspx';</script>");
                }

            }
            catch
            {
                IsAuth = false;
                Response.Write("<script>window.top.location='Login.aspx';</script>");
            }
        }

        protected void RedirectUrl(string urlParam)
        {

            string uri = string.Format("http://{0}{1}", Request.Url.Host,
                   Request.Url.Port == 80 ? "" : ":" + Request.Url.Port.ToString());

            string url = WebConfigurationManager.AppSettings["domain"] + "/" + urlParam;
            Response.Redirect(url, true);
        }
    }
}