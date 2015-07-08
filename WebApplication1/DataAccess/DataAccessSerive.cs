using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication1.DataAccess
{
    public class WhereParam
    {

        public string Where { get; set; }
        public string ParameterName { get; set; }
        public string Value { get; set; }


    }
    public class DataAccessSerive
    {
        public TableJson LoadEntities(string queryPage, int currentPageIndex, List<WhereParam> whereList)
        {
            string cmdWhere = "";
            List<OracleParameter> parameters = new List<OracleParameter>();
            whereList.ForEach(p =>
            {
                cmdWhere += string.Format(p.Where, p.ParameterName);
                parameters.Add(new OracleParameter()
            {
                ParameterName = p.ParameterName,
                Value = p.Value
            });
            });
            string sqlSelect = @"select count(*) from LYJYGD.ZP03 w   inner join LYJYGD.ZP01 c on w.ZPA001=C.ZPA001 
            where w.ZPC006=1 and w.ZPC010=0 {0} ";
            string commandText = string.Format(sqlSelect, cmdWhere);
            int count = 0;
            try
            {
                count = int.Parse(
            OracleHelper.ExecuteScalar(OracleHelper.ConnectionString, CommandType.Text, commandText, parameters).ToString()
        );
            }
            catch (Exception ex)
            {
                 
            }
            int pageSize = 10;
            sqlSelect = @"select * from (select rownum rn,w.ZPC001,w.ZPA001,w.ZPA002,w.ZPB003,w.ZPC002,w.ZPC004 from LYJYGD.ZP03 w  
            inner join LYJYGD.ZP01 c on w.ZPA001=C.ZPA001 
            where w.ZPC006=1 and w.ZPC010=0 {0}  order by w.ZPC004 desc,w.ZPA002)
            where rn>{1} and rn<{2}";
            commandText = string.Format(sqlSelect, cmdWhere, (currentPageIndex - 1) * pageSize, currentPageIndex * pageSize + 1);
            var reader = OracleHelper.ExecuteReader(OracleHelper.ConnectionString, CommandType.Text, commandText, parameters);
            StringBuilder sb = new StringBuilder();
            List<MyLink> myLinkList = new List<MyLink>();
            while (reader.Read())
            {
                try
                {
                    MyLink mylink = new MyLink();
                    mylink.Url = string.Format("<a href=\"detail.html?id={0}\">{1}</a>", reader["ZPC001"].ToString(), reader["ZPA002"]);
                    mylink.Work = reader["ZPB003"].ToString();
                    mylink.Company = reader["ZPA002"].ToString();
                    mylink.Num = reader["ZPC002"].ToString();
                    mylink.Time = ((DateTime)reader["ZPC004"]).ToString("yyyy-MM-dd");
                    myLinkList.Add(mylink);
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            MyPage page = new MyPage();
            if (currentPageIndex - 1 > 0)
            {
                page.Previous = string.Format("{0},'{1}'", queryPage, currentPageIndex - 1);
            }
            if (currentPageIndex + 1 < ((int)(count / pageSize)))
            {
                page.Next = string.Format("{0},'{1}'", queryPage, currentPageIndex + 1);
            }
            page.SelPage = ((int)(count / pageSize)).ToString();
            page.Desc = string.Format("&nbsp;&nbsp;页次：<strong><font color=\"red\">{0}</font>/{1}</strong>页" +
                        " &nbsp;共<b>{2}</b>条信息<b>{3}</b>条信息/页  &nbsp;</div>",
                        currentPageIndex, page.SelPage, count, pageSize
                        );
            TableJson tableJson = new TableJson();
            tableJson.myLinkList = myLinkList;
            tableJson.mypage = page;
            return tableJson;
        }


        public string LoadDetail(string id)
        {
            //select ZPA034 from LYJYGD.ZP01A2 经济类型
            // 表ZP01A2  ZPA032   联系地址 ZPA030   联系人  ZPA031   联系电话
            // ZP01   ZPA017   企业简介
            StringBuilder sb = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter()
           {
               ParameterName = "id",
               Value = id
           });
            string str = "";
            string sqlSelect = @" select w.ZPA001, w.ZPC001,w.ZPA002,w.ZPC008,c.ZPA016,c.ZPA017,w.ZPB003,w.ZPB011,w.ZPC003,w.ZPC002,
            w.ZPB006,w.ZPB005,w.ZPC004,w.ZPC005,w.ZPB007,w.ZPC002            
            from LYJYGD.ZP03 w inner join LYJYGD.ZP01 c on w.ZPA001=C.ZPA001 where w.ZPC001=:id";
            try
            {
                var reader = OracleHelper.ExecuteReader(OracleHelper.ConnectionString, CommandType.Text, sqlSelect, parameters);
                while (reader.Read())
                {
                    sb.Append("<dl><dt>企业信息</dt><dd>");
                    sb.AppendFormat("<p><label>企业名称:</label><font color='#FF0000'>{0}</font></p>",
                        ShowVal(reader["ZPA002"]));
                    sb.AppendFormat("<p><label>所属劳动部门:</label>{0}</p>", ShowVal(reader["ZPC008"]));
                    sb.AppendFormat("<p><label>单位类型:</label>{0}</p>", GetAA11Type(reader["ZPA016"].ToString(), "ZPA016"));
                    sb.Append(GetContack(reader["ZPA001"].ToString()));
                    sb.AppendFormat("<p><label>企业介绍:</label>{0}</p>", ShowVal(reader["ZPA017"]));
                    sb.Append("</dd></dl>");
                    sb.Append("<dl><dt>招聘信息</dt><dd>");
                    sb.AppendFormat("<p><label>招聘职位:</label><font color='#FF0000'>{0}</font></p>", ShowVal(reader["ZPB003"]));
                    sb.AppendFormat("<p><label>工作方式:</label>{0}</p>", reader["ZPB011"] == null ? "&nbsp;&nbsp;" : "全职");
                    sb.AppendFormat("<p><label>最低月薪:</label>{0}</p>", ShowVal(reader["ZPC003"]));
                    sb.Append(GetWorkOther(reader["ZPC002"].ToString(), id));
                    sb.AppendFormat("<p><label>年龄:</label>{0}</p>", ShowVal(reader["ZPB006"]));
                    sb.AppendFormat("<p><label>文化要求:</label>{0}</p>", GetAA11Type(reader["ZPB005"].ToString(), "ZPB005"));
                    sb.AppendFormat("<p><label>登记日期:</label>{0}</p>", ((DateTime)reader["ZPC004"]).ToString("yyyy-MM-dd"));
                    sb.AppendFormat("<p><label>有效日期:</label>{0}</p>", ((DateTime)reader["ZPC005"]).ToString("yyyy-MM-dd"));
                    sb.AppendFormat("<p><label>其他要求:</label>{0}</p>", ShowVal(reader["ZPB007"]));
                    sb.Append("</dd></dl>");
                }
            }
            catch (Exception ex)
            {


            }


            return sb.ToString();
        }

        private string ShowVal(object val)
        {
            if (val == null)
            {
                return "&nbsp;&nbsp;";
            }
            if (string.IsNullOrEmpty(val.ToString().Trim()))
            {
                return "&nbsp;&nbsp;";
            }
            return val.ToString();
        }

        /// <summary>
        /// 取得对应编码名称
        /// </summary>
        /// <returns></returns>
        private string GetAA11Type(string val, string name)
        {
            string sql = string.Format("select AAA103 from  LYJYGD.AA11 where AAA102='{0}' and AAA100='{1}'", val, name);
            var reader = OracleHelper.ExecuteReader(sql);
            while (reader.Read())
            {
                return reader[0].ToString();
            }
            return "";
        }

        private string GetContack(string cid)
        {
            string sql = string.Format("select ZPA032,ZPA030,ZPA031 from  LYJYGD.ZP01A2  where ZPA001='{0}'", cid);
            var reader = OracleHelper.ExecuteReader(sql);
            StringBuilder sb = new StringBuilder();
            while (reader.Read())
            {
                sb.AppendFormat("<p><label>详细地址:</label>{0}</p>", ShowVal(reader["ZPA032"]));
                sb.AppendFormat("<p><label>联系人:</label>{0}</p>", ShowVal(reader["ZPA030"]));
                sb.AppendFormat("<p><label>联系电话:</label>{0}</p>", ShowVal(reader["ZPA031"]));
                return sb.ToString();
            }
            return "";
        }

        /// <summary>
        /// ZP03A1单位招聘信息副表
        /// </summary>
        private string GetWorkOther(string num, string wid)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p><label>招聘人数:</label>{0}", num);
            string sql = string.Format(" select * from LYJYGD.ZP03A1  where ZPC001='{0}'", wid);
            ///招聘人数:	1人，其中：男0人，女1人，不限0人　	招聘对象:	　
            //年龄：	不限 　	身高:ZPC032   	 	视力:ZPC033   	 	户口要求:ZPC031    　
            //文化程度:	ZPB005   　	技术等级:	无/未说明 　	职业资格证书:
            var reader = OracleHelper.ExecuteReader(sql);
            while (reader.Read())
            {
                sb.AppendFormat("，其中：男{0}人，女{1}人，不限{2}人</p>", reader["ZPC024"], reader["ZPC025"], reader["ZPC026"]);
                sb.AppendFormat("<p><label>身高:</label> {0} </p>", ShowVal(reader["ZPC032"]));
                sb.AppendFormat("<p><label>视力:</label> {0} </p>", ShowVal(reader["ZPC033"]));
                sb.AppendFormat("<p><label>户口要求:</label>{0}", GetAA11Type(reader["ZPC031"].ToString(), "ZPC031"));
            }
            sb.Append("</p>");
            return sb.ToString();
        }
    }
}