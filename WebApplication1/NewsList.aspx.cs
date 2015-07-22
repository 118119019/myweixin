using CommonService.Serilizer;
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
    public partial class NewsList : MyWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //NewsCfg cfg = new NewsCfg() { Id = 1, Name = "2014年龙岩市直城乡劳动力职业技能培训定点机构公示", Type = 1 };
            //List<NewsCfg> cfgList = new List<NewsCfg>();
            //cfgList.Add(cfg);
            //cfg = new NewsCfg() { Id = 2, Name = "2014 年龙岩市直SIYB创业培训定点机构公示", Type = 1 };
            //cfgList.Add(cfg);
            // 
            //  File.WriteAllText(path, SerilizeService<List<NewsCfg>>.CreateSerilizer(Serilize_Type.Xml).Serilize(cfgList));
            string path = Server.MapPath("~/News/newsconfig.xml");
            List<NewsCfg> cfgList =
            SerilizeService<List<NewsCfg>>.CreateSerilizer(Serilize_Type.Xml).Deserilize(
           File.ReadAllText(path));
            rptList1.DataSource = cfgList;
            rptList1.DataBind();

        }
        protected void rptList1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Docommand(e.CommandName, Convert.ToString(e.CommandArgument));
        }
        protected void Docommand(string command, string id)
        {
            if (command == "Edit")
            {
                RedirectUrl("NewsEdit.aspx?id=" + id);
            }
        }
        protected string showTypeName(string type)
        {
            return type == "1" ? "培训信息" : "资讯信息";
        }
    }
}