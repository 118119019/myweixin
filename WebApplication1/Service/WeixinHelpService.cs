using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication1.Service
{
    public class WeixinHelpService
    {
        public  string GetTemp(HttpServerUtility Server)
        {
            string url = Server.MapPath("~/Temp.html");

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(File.ReadAllText(url));
            HtmlNode trNode = doc.DocumentNode.SelectSingleNode("//div[@id='content']");
            string str = trNode.InnerHtml ;
            return trNode.InnerHtml ;
        }
    }
}