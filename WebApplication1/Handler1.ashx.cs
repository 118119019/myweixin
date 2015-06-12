using CommonService.Serilizer;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace WebApplication1
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var function = context.Request["funct"];
            WebParse webparse = new WebParse();
            switch (function)
            {
                case "GetNewList":
                    context.Response.Write(webparse.LoadNewlist(context));
                    TableJson tableJson = new TableJson();
                    var fbsj = "00";
                    var gzgw = context.Request["work"];
                    var ssldbm = context.Request["place"];
                    var page = context.Request["page"];

                    //select * from LYJYGD.ZP03 where ZPC006=1 and ZPC010=0 order by  ZPC004  desc



                    break;
                case "GetNewDetail":
                    context.Response.Write(webparse.LoadNewDetail(context));
                    break;
                default:
                    context.Response.Write("Hello World");
                    break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private string GetJson(TableJson tableJson)
        {
            return SerilizeService<TableJson>.CreateSerilizer(Serilize_Type.Json).Serilize(tableJson);
        }

    }

    public class TableJson
    {
        public List<MyLink> myLinkList { get; set; }
        public MyPage mypage { get; set; }
    }
    public class MyLink
    {
        public string Company { get; set; }
        public string Url { get; set; }
        public string Work { get; set; }
        public string Num { get; set; }
        public string Time { get; set; }
    }

    public class MyPage
    {
        public string Previous { get; set; }
        public string Next { get; set; }

        public string Desc { get; set; }
        public string SelPage { get; set; }
    }

}