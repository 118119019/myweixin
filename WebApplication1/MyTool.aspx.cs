
using NLog;
using Oracle.ManagedDataAccess.Client;
using Quartz;
using Quartz.Impl;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using Senparc.Weixin.MP.AdvancedAPIs.Groups;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Service;

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
                OracleHelper.ExecuteScalar(OracleHelper.ConnectionString, CommandType.Text, commandText, parmetList).ToString()
            );
            int currentPageIndex = 1;
            int pageSize = 10;
            sqlSelect = @" select * from (select rownum rn,w.ZPA001,w.ZPA002,w.ZPB003,w.ZPC002,w.ZPC004 from LYJYGD.ZP03 w  
            inner join LYJYGD.ZP01 c on w.ZPA001=C.ZPA001 
            where w.ZPC006=1 and w.ZPC010=0 {0}  order by w.ZPC004 desc,w.ZPA002)
            where rn>{1} and rn<{2}";
            commandText = string.Format(sqlSelect, cmdWhere, (currentPageIndex - 1) * pageSize, currentPageIndex * pageSize + 1);
            var reader = OracleHelper.ExecuteReader(OracleHelper.ConnectionString, CommandType.Text, commandText, parmetList);
            StringBuilder sb = new StringBuilder();
            while (reader.Read())
            {
                sb.AppendLine(reader["ZPA001"] + " " + reader["ZPA002"] + " ");
            }
            txtResult.Text = sb.ToString();
        }

        protected void btnStar_Click(object sender, EventArgs e)
        {
            MySchedulerService myscheduler = MySchedulerService.GetInstance();
            //JobKey jkey = new JobKey("jobName1", "jobGroup1");
            //IJobDetail job = myscheduler.sched.GetJobDetail(jkey);
            //txtResult.Text = job.Key.ToString();
            myscheduler.ShutDown();
            myscheduler.Run();
            txtResult.Text = "重启成功";
        }

        protected void btnQueryService_Click(object sender, EventArgs e)
        {
            MySchedulerService myscheduler = MySchedulerService.GetInstance();
            if (myscheduler.sched.IsStarted)
            {
                lbStatus.Text = " 服务已开始";
            }
            else
            {
                lbStatus.Text = " 服务未开始";
            }
            if (myscheduler.sched.IsShutdown)
            {
                lbStatus.Text += " 服务已关闭";
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            MySchedulerService myscheduler = MySchedulerService.GetInstance();

            myscheduler.ShutDown();

            txtResult.Text = "提交关闭服务";
        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            //获取 accessToken
            var accessToken = AccessTokenContainer.TryGetToken(WebConfigurationManager.AppSettings["LongNameAppId"],
                WebConfigurationManager.AppSettings["LongNameAppSecret"]);


            OpenIdResultJson json = Senparc.Weixin.MP.AdvancedAPIs.User.UserApi.Get(accessToken, "");

            txtResult.Text += json.count;

            foreach (var item in json.data.openid)
            {
                txtResult.Text += item + " ";
            }

            var result = GroupsApi.Get(accessToken);
            List<GroupsJson_Group> list = result.groups;
            string content = "文本内容<a href='http://www.baidu.com'>百度</a>";
            string groupId = "100";//分组Id

            //var accessToken = AccessTokenContainer.GetToken(_appId);
            //发送给指定分组
            var sendResult = GroupMessageApi.SendTextGroupMessageByGroupId(accessToken, groupId, content, false);


        }
    }
}