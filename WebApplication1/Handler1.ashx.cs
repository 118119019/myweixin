using CommonService.Serilizer;
using HtmlAgilityPack;
using LongYanService;
using LongYanService.Model;
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
            var service = new DataAccessSerive();
            switch (function)
            {
                case "GetNewList":
                    var industry = context.Request["industry"];
                    var degree = context.Request["degree"];
                    var work = context.Request["work"];
                    var regDate = context.Request["reg"];
                    var effectDate = context.Request["effect"];
                    var place = context.Request["place"];
                    var page = int.Parse(context.Request["page"]);
                    string queryPage = string.Format(
                        "'{0}','{1}','{2}','{3}','{4}','{5}'",
                        industry, degree, work, regDate, effectDate, place
                        );
                    List<WhereParam> whereList = new List<WhereParam>();
                    if (industry != "00")
                    {
                        whereList.Add(new WhereParam()
                        {
                            Where = " and c.ZPA010=:{0}",
                            ParameterName = "Industry",
                            Value = industry
                        });
                    }
                    if (degree != "00")
                    {
                        whereList.Add(new WhereParam()
                        {
                            Where = " and w.ZPB005=:{0}",
                            ParameterName = "Degree",
                            Value = degree
                        });
                    }
                    if (work != "0000000")
                    {
                        whereList.Add(new WhereParam()
                        {
                            Where = " and w.ZPB002=:{0}",
                            ParameterName = "Work",
                            Value = work
                        });
                    }
                    if (regDate != "")
                    {
                        whereList.Add(new WhereParam()
                        {
                            Where = " and w.ZPC004>=to_date(:{0}, 'yyyy-mm-dd')",
                            ParameterName = "regDate",
                            Value = regDate
                        });
                    }
                    if (effectDate != "")
                    {
                        whereList.Add(new WhereParam()
                        {
                            Where = " and w.ZPC004<=to_date(:{0}, 'yyyy-mm-dd')",
                            ParameterName = "effectDate",
                            Value = effectDate
                        });
                    }
                    if (place != "0")
                    {
                        if (place.Contains(','))
                        {
                            string[] strs = place.Split(',');
                            var str = "";
                            for (int i = 0; i < strs.Length; i++)
                            {
                                if (i == 0)
                                {
                                    str = " and c.ZPA018 in (:{0}";
                                }
                                else if (i == strs.Length - 1)
                                {
                                    str = ",:{0})";
                                }
                                else
                                {
                                    str = ",:{0}";
                                }
                                whereList.Add(new WhereParam()
                                {
                                    Where = str,
                                    ParameterName = "place" + i.ToString(),
                                    Value = strs[i]
                                });
                            }
                        }
                        else
                        {
                            whereList.Add(new WhereParam()
                            {
                                Where = " and c.ZPA018=:{0}",
                                ParameterName = "place",
                                Value = place
                            });
                        }
                    }

                    context.Response.Write(GetJson(service.LoadEntities(queryPage, page, whereList)));
                    break;
                case "GetNewDetail":
                    var id = context.Request["id"];
                    context.Response.Write(service.LoadDetail(id));
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

   

}