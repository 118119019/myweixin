using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Senparc.Weixin.MP.Entities;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

            //openId.data.openid

        }

        protected void sendmsg_Click(object sender, EventArgs e)
        {
            //var accessToken = AccessTokenContainer.TryGetToken(WebConfigurationManager.AppSettings["WeixinAppId"],
            //   WebConfigurationManager.AppSettings["WeixinAppSecret"]);
            //var openId = Senparc.Weixin.MP.AdvancedAPIs.User.Get(accessToken, "");
            //foreach (var item in openId.data.openid)
            //{
            //    List<Article> articles = new List<Article>();
            //    Article article = new Article();
            //    article.Title = "官网";
            //    article.Description = "官网链接";
            //    article.PicUrl = "http://fjlylm.com/imagesnews/tb1.gif";
            //    article.Url = "http://fjlylm.com";
            //    articles.Add(article);

            //    article = new Article();
            //    article.Title = "手工书套";
            //    article.Description = "手工书套";
            //    article.PicUrl = "http://imglf0.ph.126.net/6ZvzrNuHfY84M-RykhBaug==/3320560300355692608.jpg";
            //    article.Url = "http://echokid.lofter.com/post/cab81_a86556?act=qbweekmagazine_20130609_02";
            //    articles.Add(article);
            //    var result = Custom.SendNews(accessToken, item, articles);
            //}
        }
    }
}