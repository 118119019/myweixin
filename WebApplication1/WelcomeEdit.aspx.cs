using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class WelcomeEdit : BaseAuthPage
    {
        protected string WelcomePath = HttpContext.Current.Server.MapPath("~/Welcome.txt");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    txtWelcome.Text = File.ReadAllText(WelcomePath);
                }
                catch (Exception ex)
                {

                    File.WriteAllText(WelcomePath, "您好，欢迎关注龙岩就业微信公众平台！");
                    txtWelcome.Text = "您好，欢迎关注龙岩就业微信公众平台！";
                }
            }
        }
        protected void btnSaveWelcome_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(WelcomePath, txtWelcome.Text.Trim());
                txtResult.Text = "修改配置成功 " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                txtResult.Text = "修改配置 " + ex.Message + " " + DateTime.Now.ToString();
            }

        }
    }
}