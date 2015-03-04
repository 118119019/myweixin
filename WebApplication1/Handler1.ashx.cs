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

            switch (function)
            {
                case "GetList":
                    context.Response.Write(LoadEntities(context));
                    break;
                case "GetDetail":
                    context.Response.Write(LoadDetail(context));
                    break;
                case "GetNewList":
                    context.Response.Write(LoadNewlist(context));
                    break;
                case "GetNewDetail":
                    context.Response.Write(LoadNewDetail(context));
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
        private string LoadNewDetail(HttpContext context)
        {
            var id = context.Request["id"];
            if (id == "")
            {
                return "";
            }
            string url = string.Format(@"http://fjlylm.com/zwxq.asp?id={0}", id);
            string content = CommonUtility.HttpUtility.Get(url, System.Text.Encoding.Default);
            //企业信息
            string tableStr = GetSpiltContent(content,
                "<table border=\"1\" cellpadding=\"3\" cellspacing=\"0\" width=\"762\" class=\"pix9\" bordercolorlight=\"#FFFFFF\" bordercolordark=\"#808000\" height=\"244\">"
                , "</table>");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(tableStr);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<dl><dt>企业信息</dt><dd>");
            GetDeatailTd(doc, sb);
            sb.AppendFormat("</dd></dl><dl><dt>招聘信息</dt><dd>");
            tableStr = GetSpiltContent(content,
               "<table border=\"1\" cellpadding=\"3\" cellspacing=\"0\" width=\"98%\" class=\"pix9\" bordercolorlight=\"#FFFFFF\" bordercolordark=\"#808000\">"
               , "</table>");
            doc = new HtmlDocument();
            doc.LoadHtml(tableStr);
            GetDeatailTd(doc, sb);
            sb.AppendFormat("</dd>");
            return sb.ToString();
        }
        private string LoadNewlist(HttpContext context)
        {
            var fbsj = "00";
            var gzgw = context.Request["work"];
            var ssldbm = context.Request["place"];
            var page = context.Request["page"];
            string url = string.Format(@"http://fjlylm.com/vizpxx.asp?ssldbm={0}&gzgw={1}&fbsj={2}&page={3}", ssldbm, gzgw, fbsj, page);
            string content = CommonUtility.HttpUtility.Get(url, System.Text.Encoding.Default);
            int iTableStart = content.IndexOf("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"95%\" class=\"pix9\">", 0);
            if (iTableStart < 1)
            {
                return "";
            }
            int iTableEnd = content.IndexOf("</table>", iTableStart);
            if (iTableEnd < 1)
            {
                return "";
            }
            string strWeb = content.Substring(iTableStart, iTableEnd - iTableStart);
            #region Tr列表

            //生成HtmlDocument
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(strWeb);
            List<MyLink> myLinkList = new List<MyLink>();
            foreach (HtmlNode trNode in doc.DocumentNode.SelectNodes("//tr"))
            {
                if (!trNode.InnerHtml.Contains("font"))
                {
                    var mylink = new MyLink();
                    foreach (HtmlNode tdNode in trNode.ChildNodes)
                    {
                        HtmlAttribute width = tdNode.Attributes["width"];
                        if (width == null)
                        {
                            continue;
                        }
                        HtmlNode node;
                        switch (width.Value)
                        {
                            case "40%":
                                node = tdNode.ChildNodes.FindFirst("a");
                                mylink.Url = node.OuterHtml.Replace("zwxq.asp", "detail.html");
                                break;
                            case "35%":
                                node = tdNode.ChildNodes.FindFirst("p");
                                mylink.Work = node.InnerHtml.Trim();
                                break;
                            case "10%":
                                node = tdNode.ChildNodes.FindFirst("p");
                                mylink.Num = node.InnerHtml.Trim();
                                break;
                            case "12%":
                                node = tdNode.ChildNodes.FindFirst("p");
                                mylink.Time = node.InnerHtml.Trim();
                                break;
                            default:
                                break;
                        }
                    }
                    myLinkList.Add(mylink);
                }
            }
            #endregion

            if (myLinkList.Count > 0)
            {
                var tableJson = new TableJson()
                {
                    myLinkList = myLinkList
                };
                //分页
                int iPageStart = content.IndexOf("<p align='center' vAlign='bottom'>", 0);
                if (iPageStart < 1)
                {
                    return GetJson(tableJson);
                }
                int iPageEnd = content.IndexOf("转到：", iPageStart);
                if (iPageEnd < 1)
                {
                    return GetJson(tableJson);
                }
                string pageContent = content.Substring(iPageStart, iPageEnd + 3 - iPageStart);
                HtmlDocument pageDoc = new HtmlDocument();
                pageDoc.LoadHtml(pageContent);
                MyPage mypage = new MyPage();
                var ANodeList = pageDoc.DocumentNode.SelectNodes("//a");
                if (ANodeList == null)
                {
                    return GetJson(tableJson);
                }
                foreach (HtmlNode aNode in ANodeList)
                {
                    if (aNode.ChildNodes.FindFirst("font").InnerHtml == "上一页")
                    {
                        mypage.Previous = GetPageParam(aNode.Attributes["href"].Value);
                    }
                    else
                    {
                        mypage.Next = GetPageParam(aNode.Attributes["href"].Value);
                    }
                }
                iPageStart = pageContent.IndexOf("]&nbsp;&nbsp;", "]&nbsp;&nbsp;".Length);
                iPageEnd = pageContent.IndexOf("转到：", iPageStart);
                mypage.Desc = pageContent.Substring(iPageStart, iPageEnd - iPageStart).Replace("]","");
                tableJson.mypage = mypage;
                return GetJson(tableJson);
            }
            return "";

        }

        private string GetJson(TableJson tableJson)
        {
            return SerilizeService<TableJson>.CreateSerilizer(Serilize_Type.Json).Serilize(tableJson);
        }
        private string LoadDetail(HttpContext context)
        {
            var id = context.Request["id"];
            if (id == "")
            {
                return "";
            }
            string url = string.Format(@"http://fjlylm.com/zwxq.asp?id={0}", id);
            string content = CommonUtility.HttpUtility.Get(url, System.Text.Encoding.Default);
            //企业信息
            string tableStr = GetSpiltContent(content,
                "<table border=\"1\" cellpadding=\"3\" cellspacing=\"0\" width=\"762\" class=\"pix9\" bordercolorlight=\"#FFFFFF\" bordercolordark=\"#808000\" height=\"244\">"
                , "</table>");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(tableStr);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<h2>企业信息</h2><p>");
            GetDeatailTd(doc, sb);
            sb.AppendFormat("</p><h2>招聘信息</h2><p>");
            tableStr = GetSpiltContent(content,
               "<table border=\"1\" cellpadding=\"3\" cellspacing=\"0\" width=\"98%\" class=\"pix9\" bordercolorlight=\"#FFFFFF\" bordercolordark=\"#808000\">"
               , "</table>");
            doc = new HtmlDocument();
            doc.LoadHtml(tableStr);
            GetDeatailTd(doc, sb);
            sb.AppendFormat("</p>");
            return sb.ToString();
        }

        private static void GetDeatailTd(HtmlDocument doc, StringBuilder sb)
        {
            foreach (HtmlNode tdNode in doc.DocumentNode.SelectNodes("//td"))
            {
                if (tdNode.InnerHtml.Contains("<b>"))
                {
                    sb.AppendFormat("<p>"+ tdNode.InnerHtml.Replace("b", "label"));
                }
                else
                {
                    sb.Append(tdNode.InnerHtml + "</p>");
                }
            }
        }
        private string LoadEntities(HttpContext context)
        {
            var fbsj = "00";//context.Request["fbsj"];
            var gzgw = context.Request["work"];
            var ssldbm = context.Request["place"];
            var page = context.Request["page"];
            string url = string.Format(@"http://fjlylm.com/vizpxx.asp?ssldbm={0}&gzgw={1}&fbsj={2}&page={3}", ssldbm, gzgw, fbsj, page);
            string content = CommonUtility.HttpUtility.Get(url, System.Text.Encoding.Default);
            int iTableStart = content.IndexOf("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"95%\" class=\"pix9\">", 0);
            if (iTableStart < 1)
            {
                return "";
            }
            int iTableEnd = content.IndexOf("</table>", iTableStart);
            if (iTableEnd < 1)
            {
                return "";
            }
            string strWeb = content.Substring(iTableStart, iTableEnd - iTableStart);
            #region Tr列表

            //生成HtmlDocument
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(strWeb);
            List<MyLink> myLinkList = new List<MyLink>();
            foreach (HtmlNode trNode in doc.DocumentNode.SelectNodes("//tr"))
            {
                if (!trNode.InnerHtml.Contains("font"))
                {
                    var mylink = new MyLink();
                    foreach (HtmlNode tdNode in trNode.ChildNodes)
                    {
                        HtmlAttribute width = tdNode.Attributes["width"];
                        if (width == null)
                        {
                            continue;
                        }
                        HtmlNode node;
                        switch (width.Value)
                        {
                            case "40%":
                                node = tdNode.ChildNodes.FindFirst("a");
                                mylink.Url = node.OuterHtml.Replace("zwxq.asp", "WorkDetail.html");
                                break;
                            case "35%":
                                node = tdNode.ChildNodes.FindFirst("p");
                                mylink.Work = node.InnerHtml.Trim();
                                break;
                            case "10%":
                                node = tdNode.ChildNodes.FindFirst("p");
                                mylink.Num = node.InnerHtml.Trim();
                                break;
                            case "12%":
                                node = tdNode.ChildNodes.FindFirst("p");
                                mylink.Time = node.InnerHtml.Trim();
                                break;
                            default:
                                break;
                        }
                    }
                    myLinkList.Add(mylink);
                }
            }
            #endregion

            if (myLinkList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<table class=\"table table-bordered\"><thead><tr><th>企业名称</th><th>招聘职位</th><th>招聘人数</th><th>登记时间</th></tr></thead><tbody>");

                foreach (var item in myLinkList)
                {
                    sb.AppendFormat(
                        "<tr><th scope=\"row\">{0}</th><td>{1}</td><td>{2}</td><td>{3}</td></tr>",
                        item.Url, item.Work, item.Num, item.Time
                        );
                }
                sb.Append("</tbody></table>");
                //分页

                int iPageStart = content.IndexOf("<p align='center' vAlign='bottom'>", 0);
                if (iPageStart < 1)
                {
                    return sb.ToString();
                }
                int iPageEnd = content.IndexOf("转到：", iPageStart);
                if (iPageEnd < 1)
                {
                    return sb.ToString();
                }
                string pageContent = content.Substring(iPageStart, iPageEnd + 3 - iPageStart);
                HtmlDocument pageDoc = new HtmlDocument();
                pageDoc.LoadHtml(pageContent);
                MyPage mypage = new MyPage();
                var ANodeList = pageDoc.DocumentNode.SelectNodes("//a");
                if (ANodeList == null)
                {
                    return sb.ToString();
                }
                foreach (HtmlNode aNode in ANodeList)
                {
                    if (aNode.ChildNodes.FindFirst("font").InnerHtml == "上一页")
                    {
                        mypage.Previous = aNode.Attributes["href"].Value;
                    }
                    else
                    {
                        mypage.Next = aNode.Attributes["href"].Value;
                    }
                }
                iPageStart = pageContent.IndexOf("]&nbsp;&nbsp;", "]&nbsp;&nbsp;".Length);
                iPageEnd = pageContent.IndexOf("转到：", iPageStart);
                mypage.Desc = pageContent.Substring(iPageStart, iPageEnd - iPageStart);




                sb.AppendFormat(
                    "<nav><ul class=\"pager\"><li><a href='#'onclick=\"queryPage({0})\">上一页</a></li><li><a href='#'onclick=\"queryPage({1})\">下一页</a></li><li>{2}</li><li style=\"display:none;\">转到{3}</li></ul></nav>"
                    , GetPageParam(mypage.Previous), GetPageParam(mypage.Next), mypage.Desc, mypage.SelPage

                    );

                return sb.ToString();
            }

            return "";

        }
        public string GetPageParam(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "'','',''";
            }
            string[] strs = url.Split('&');
            string prams = string.Empty;
            foreach (var item in strs)
            {
                if (item.Contains("ssldbm=") || item.Contains("gzgw") || item.Contains("page"))
                {
                    prams += "'" + item.Split('=')[1].Replace("&amp;", "") + "',";
                }
            }
            return prams.Remove(prams.Length - 1, 1);

        }


        public string GetSpiltContent(string content, string star, string end)
        {

            int iTableStart = content.IndexOf(star, 0);
            if (iTableStart < 1)
            {
                return "";
            }
            int iTableEnd = content.IndexOf(end, iTableStart);
            if (iTableEnd < 1)
            {
                return "";
            }
            return content.Substring(iTableStart, iTableEnd - iTableStart);
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