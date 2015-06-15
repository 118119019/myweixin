using CommonService.Serilizer;
using HtmlAgilityPack;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
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
                    var industry = context.Request["industry"];
                    var degree = context.Request["degree"];
                    var work = context.Request["work"];
                    var regDate = context.Request["regdate"];
                    var effectDate = context.Request["effectdate"];
                    var place = context.Request["place"];
                    var page = context.Request["page"];
                    
                //select w.ZPA001,w.ZPA002,w.ZPB003,w.ZPC002,w.ZPC004 from LYJYGD.ZP03 w  inner join
//  LYJYGD.ZP01 c on w.ZPA001=C.ZPA001
//where w.ZPC006=1 and w.ZPC010=0 order by w.ZPC004 desc,w.ZPA002


                    //select * from LYJYGD.AA11 where AAA100='ZPA010'   ---行业分类
                    // select COUNT(*) from LYJYGD.AA11 where AAA100='ZPB002'
                    //select ZPA001,ZPA002,ZPB003,ZPC002,ZPC004 from LYJYGD.ZP03 where ZPC006=1 and ZPC010=0 order by ZPC004 desc,ZPA002

                    // 页次：1/2页  共26条信息3164个岗位 22条信息/页  

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