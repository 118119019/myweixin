using CommonService.Serilizer;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.News;

namespace WebApplication1
{
    public partial class NewsEdit : BaseAuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var id = Request["id"] ?? "0";
                if (id == "0")
                {
                    return;
                }
                this.ViewState.Add("id", id);
                var path = Server.MapPath(string.Format("~/News/News{0}.html", id));
                string pageContent = File.ReadAllText(path);
                HtmlDocument pageDoc = new HtmlDocument();
                pageDoc.LoadHtml(pageContent);

                var node = pageDoc.DocumentNode.SelectSingleNode("//div[@id=\"content\"]");

                if (node != null)
                {
                    hidContent.Value = node.InnerHtml.Replace("<", "&lt;")
                        .Replace(">", "&gt;")
                        .Replace("#", "%23");
                }

                path = Server.MapPath("~/News/newsconfig.xml");
                List<NewsCfg> cfgList =
                SerilizeService<List<NewsCfg>>.CreateSerilizer(Serilize_Type.Xml).Deserilize(
               File.ReadAllText(path));
                var cfg = cfgList.Find(p => p.Id == int.Parse(id));
                if (cfg != null)
                {
                    txtTitle.Text = cfg.Name;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var id = this.ViewState["id"].ToString();
            if (!string.IsNullOrEmpty(id))
            {
                var path = Server.MapPath(string.Format("~/News/News{0}.html", id));
                string pageContent = File.ReadAllText(path);
                HtmlDocument pageDoc = new HtmlDocument();
                pageDoc.LoadHtml(pageContent);
                var node = pageDoc.DocumentNode.SelectSingleNode("//div[@id=\"content\"]");
                string str = hidContent.Value.Replace("&lt;", "<").Replace("&gt;", ">").Replace("%23", "#");
                node.InnerHtml = str;
                pageDoc.Save(path);

                path = Server.MapPath("~/News/newsconfig.xml");
                List<NewsCfg> cfgList = SerilizeService<List<NewsCfg>>.CreateSerilizer(Serilize_Type.Xml).Deserilize(
                File.ReadAllText(path));
                var cfg = cfgList.Find(p => p.Id == int.Parse(id));
                if (cfg != null)
                {
                    cfg.Name = txtTitle.Text.Trim();
                }
                string cfgContent = SerilizeService<List<NewsCfg>>.CreateSerilizer(Serilize_Type.Xml).Serilize
                    (cfgList);
                File.WriteAllText(path, cfgContent);
                RedirectUrl("NewsList.aspx");
            }
        }
    }
}