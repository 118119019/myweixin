using CommonService.Serilizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace WebApplication1.News
{
    public class NewsCfg
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
    }
    public class NewsCfgOp
    {
        public List<NewsCfg> GetNewsCfgList()
        {
            string path = HttpContext.Current.Server.MapPath("~/News/newsconfig.xml");
            List<NewsCfg> cfgList =
            SerilizeService<List<NewsCfg>>.CreateSerilizer(Serilize_Type.Xml).Deserilize(
           File.ReadAllText(path));
            return cfgList;
        }

        public string GetHrefStr(int type)
        {
            var list = GetNewsCfgList().FindAll(p => p.Type == type);
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.AppendFormat("<a href=\"{0}/News/News{1}.html\">{2}</a>\r\n",
                WebConfigurationManager.AppSettings["domain"], item.Id, item.Name);
            }
            return sb.ToString();
        }

    }
}