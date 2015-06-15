using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtResult.Text = OracleHelper.CanConnect();
            }

        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string cmdWhere = "";
            List<OracleParameter> parmetList = new List<OracleParameter>();
            //行业分类
            if (txtIndustry.Text != "")
            {
                cmdWhere += " and c.ZPA010=:Industry";
                parmetList.Add(new OracleParameter()
                {
                    ParameterName = "Industry",
                    Value = txtIndustry.Text.Trim()
                });
            }
            //文化学历
            if (txtDegree.Text != "")
            {
                cmdWhere += " and w.ZPB005=:Degree";
                parmetList.Add(new OracleParameter()
                {
                    ParameterName = "Degree",
                    Value = txtDegree.Text.Trim()
                });
            }
            //岗位ID ZPB002  
            if (txtWork.Text != "")
            {
                cmdWhere += " and w.ZPB002=:Work";
                parmetList.Add(new OracleParameter()
                {
                    ParameterName = "Work",
                    Value = txtWork.Text.Trim()
                });
            }

            string sqlSelect = @"select count(*) from LYJYGD.ZP03 w   inner join LYJYGD.ZP01 c on w.ZPA001=C.ZPA001 
            where w.ZPC006=1 and w.ZPC010=0 {0} ";

            string commandText = string.Format(sqlSelect, cmdWhere);
            int count = int.Parse(
                OracleHelper.ExecuteScalar(OracleHelper.ConnectionString, CommandType.Text, commandText, parmetList.ToArray()).ToString()
            );
            int currentPageIndex = 1;
            int pageSize = 10;


            sqlSelect = @" select * from (select rownum rn,w.ZPA001,w.ZPA002,w.ZPB003,w.ZPC002,w.ZPC004 from LYJYGD.ZP03 w  
            inner join LYJYGD.ZP01 c on w.ZPA001=C.ZPA001 
            where w.ZPC006=1 and w.ZPC010=0 {0}  order by w.ZPC004 desc,w.ZPA002)
            where rn>{1} and rn<{2}";
            commandText = string.Format(sqlSelect, cmdWhere, (currentPageIndex - 1) * pageSize, currentPageIndex * pageSize + 1);
            var reader = OracleHelper.ExecuteReader(OracleHelper.ConnectionString, CommandType.Text, commandText, parmetList.ToArray());
            StringBuilder sb = new StringBuilder();
            while (reader.Read())
            {
                sb.AppendLine(reader["ZPA001"] + " " + reader["ZPA002"] + " ");
            }
            txtResult.Text = sb.ToString();




        }
    }
}