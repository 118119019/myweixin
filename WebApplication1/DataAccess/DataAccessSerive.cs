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
                cmdWhere += string.Format(" and {0}:{1}", p.Where, p.ParameterName);
                parameters.Add(new OracleParameter()
            {
                ParameterName = p.ParameterName,
                Value = p.Value
            });
            });
            string sqlSelect = @"select count(*) from LYJYGD.ZP03 w   inner join LYJYGD.ZP01 c on w.ZPA001=C.ZPA001 
            where w.ZPC006=1 and w.ZPC010=0 {0} ";
            string commandText = string.Format(sqlSelect, cmdWhere);
            int count = int.Parse(
                OracleHelper.ExecuteScalar(OracleHelper.ConnectionString, CommandType.Text, commandText, parameters).ToString()
            );
          
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
    }
}