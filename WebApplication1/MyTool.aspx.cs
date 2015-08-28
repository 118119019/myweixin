
using LongYanService;
using NLog;
using Oracle.ManagedDataAccess.Client;
using Quartz;
using Quartz.Impl;
using Quartz.Xml.JobSchedulingData20;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.Custom;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using Senparc.Weixin.MP.AdvancedAPIs.Groups;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using WebApplication1.Service;

namespace WebApplication1
{
    public partial class MyTool : BaseAuthPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string sendCountTxt = Server.MapPath("sendcount.txt");
                try
                {
                    ddlSendCount.SelectedValue = File.ReadAllText(sendCountTxt);
                }
                catch
                {
                    File.WriteAllText(sendCountTxt, "6");
                    ddlSendCount.SelectedValue = "6";
                }



                for (int i = 0; i < 23; i++)
                {
                    ddlHour.Items.Add(
                        new ListItem(i.ToString() + "点", i.ToString())
                        );
                }
                for (int i = 0; i < 60; i++)
                {
                    ddlMinute.Items.Add(
                        new ListItem(i.ToString() + "分", i.ToString())
                        );
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Server.MapPath("quartz_jobs.xml"));
                XmlNodeList elemList = xmlDoc.GetElementsByTagName("cron-expression");
                var node = elemList[0];
                string[] cronList = node.InnerText.Split(' ');//0 1 9 ? * TUE
                ddlMinute.SelectedValue = cronList[1];
                ddlHour.SelectedValue = cronList[2];




                txtResult.Text = OracleHelper.CanConnect();
                if (txtResult.Text != "连接成功")
                {
                    var accessToken = AccessTokenContainer.TryGetToken(WebConfigurationManager.AppSettings["LongNameAppId"],
                    WebConfigurationManager.AppSettings["LongNameAppSecret"]);
                    var json = GroupsApi.Get(accessToken);
                    var groupId = json.groups.Find(p => p.name == "开发小组").id.ToString();
                    var content = " oracle数据库无法连接 异常信息为" + txtResult.Text;
                    var sendResult = GroupMessageApi.SendTextGroupMessageByGroupId(accessToken, groupId, content, false);
                }
            }
        }

        protected void btnSendAll_Click(object sender, EventArgs e)
        {
            string tempStr = new WeixinHelpService().GetTemp(Server);

            var accessToken = AccessTokenContainer.TryGetToken(WebConfigurationManager.AppSettings["LongNameAppId"],
              WebConfigurationManager.AppSettings["LongNameAppSecret"]);
            OpenIdResultJson json = UserApi.Get(accessToken, "");

            var imgResult = MediaApi.GetOthersMediaList(accessToken, UploadMediaFileType.image, 0, 10000);
            SendImg(accessToken, imgResult);
            imgResult = MediaApi.GetOthersMediaList(accessToken, UploadMediaFileType.image, 0, 10000);

            SendGroupMessage(tempStr, accessToken, imgResult, "服务号");
        }

        protected void btnShortSendAll_Click(object sender, EventArgs e)
        {
            string tempStr = new WeixinHelpService().GetTemp(Server);

            var accessToken = AccessTokenContainer.TryGetToken(WebConfigurationManager.AppSettings["ShortWeixinAppId"],
              WebConfigurationManager.AppSettings["ShortWeixinSecret"]);
            OpenIdResultJson json = UserApi.Get(accessToken, "");

            var imgResult = MediaApi.GetOthersMediaList(accessToken, UploadMediaFileType.image, 0, 10000);
            SendImg(accessToken, imgResult);
            imgResult = MediaApi.GetOthersMediaList(accessToken, UploadMediaFileType.image, 0, 10000);

            SendGroupMessage(tempStr, accessToken, imgResult, "订阅号");
        }
        private void SendImg(string accessToken, MediaList_OthersResult imgResult)
        {
            for (int i = 0; i < 6; i++)
            {
                string imgName = string.Format("send{0}.jpg", i);
                if (imgResult.item.Find(p => p.name == imgName) == null)
                {
                    var filePath = Server.MapPath("~/image/" + imgName);
                    var mediaId = MediaApi.UploadForeverMedia(accessToken, filePath).media_id;
                }
            }
        }
        private void SendGroupMessage(string tempStr, string accessToken, MediaList_OthersResult imgResult, string strAction)
        {
            string sendCountTxt = Server.MapPath("sendcount.txt");
            var sendCount = int.Parse(File.ReadAllText(sendCountTxt));
            NewsModel[] newsList = new NewsModel[sendCount];
            var dataSevice = new DataAccessSerive();
            var jobList = dataSevice.GetTopJobInfoList();
            if (jobList.Count > 0)
            {
                List<Article> articles = new List<Article>();
                int i = 0;
                string imgName;
                foreach (var job in jobList)
                {
                    if (i == sendCount)
                    {
                        break;
                    }
                    imgName = "send" + i.ToString() + ".jpg";
                    string imgUrl = string.Format("{0}/image/{1}", WebConfigurationManager.AppSettings["domain"], imgName);
                    var jobDetail = dataSevice.GetJobDetail(job.JobId);
                    var news = new NewsModel();
                    news.author = "";
                    news.content = tempStr.Replace("[ComName]", job.ComName)
                          .Replace("[ComBrief]", jobDetail.ComBrief)
                       .Replace("[DetailPalce]", jobDetail.DetailPalce)
                       .Replace("[LinkMan]", jobDetail.LinkMan)
                       .Replace("[Phone]", jobDetail.Phone)
                       .Replace("[JobName]", jobDetail.JobName)
                       .Replace("[JobType]", jobDetail.JobType)
                       .Replace("[LowMoney]", jobDetail.LowMoney)
                       .Replace("[HrNum]", jobDetail.HrNum)
                       .Replace("[Edu]", jobDetail.Edu)
                       .Replace("[RegisterDate]", jobDetail.RegisterDate)
                       .Replace("[EffectDate]", jobDetail.EffectDate)
                       .Replace("[Other]", jobDetail.Other)
                       .Replace("[Sex]", jobDetail.Sex)
                       ;
                    //news.content_source_url = WebConfigurationManager.AppSettings["domain"] + "/html/detail.html?id=" + job.JobId;
                    news.content_source_url = WebConfigurationManager.AppSettings["website"];
                    news.digest = job.ComName + "诚聘" + job.JobName;
                    news.show_cover_pic = "0";
                    news.thumb_media_id = imgResult.item.Find(p => p.name == imgName).media_id;
                    news.title = job.ComName + "诚聘" + job.JobName;
                    newsList[i] = news;
                    i++;
                }
                UploadForeverMediaResult mediaResult = MediaApi.UploadNews(accessToken, 100000, newsList);
                try
                {
                    GroupMessageApi.SendGroupMessageByGroupId
                    (accessToken, "-1", mediaResult.media_id, GroupMessageType.mpnews, true);
                    txtResult.Text += "提交成功一次" + strAction + " 推送成功";
                }
                catch (Exception ex)
                {
                    txtResult.Text += "提交成功一次订" + strAction + " 推送失败 " + ex.Message;
                }
            }
        }



        protected void btnSaveQuartzCfg_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("quartz_jobs.xml"));
            XmlNodeList elemList = xmlDoc.GetElementsByTagName("cron-expression");
            var node = elemList[0];
            string[] cronList = node.InnerText.Split(' ');//0 1 9 ? * TUE
            cronList[1] = ddlMinute.SelectedValue;
            cronList[2] = ddlHour.SelectedValue;

            node.InnerText = string.Join(" ", cronList);
            xmlDoc.Save(Server.MapPath("quartz_jobs.xml"));
        }



        protected void btnSaveSendCount_Click(object sender, EventArgs e)
        {
            string sendCountTxt = Server.MapPath("sendcount.txt");
            File.WriteAllText(sendCountTxt, ddlSendCount.SelectedValue);
        }


        #region 已经无用 调试用

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            //获取 accessToken
            var accessToken = AccessTokenContainer.TryGetToken(WebConfigurationManager.AppSettings["LongNameAppId"],
                WebConfigurationManager.AppSettings["LongNameAppSecret"]);


            OpenIdResultJson json = UserApi.Get(accessToken, "");

            txtResult.Text += json.count;

            foreach (var item in json.data.openid)
            {
                var userinfo = UserApi.Info(accessToken, item);
                txtResult.Text += item + " : " + userinfo.nickname + " " + userinfo.sex.ToString() + "   ";
            }

            var result = GroupsApi.Get(accessToken);
            List<GroupsJson_Group> list = result.groups;
            string content = "文本内容<a href='http://www.baidu.com'>百度</a>";
            string groupId = "100";//分组Id


            var dataSevice = new DataAccessSerive();
            var jobList = dataSevice.GetTopJobInfoList();
            if (jobList.Count > 0)
            {
                List<Article> articles = new List<Article>();
                int i = 0;
                foreach (var job in jobList)
                {
                    string imgUrl = string.Format("{0}/image/{1}.jpg", WebConfigurationManager.AppSettings["domain"], i);
                    var art1 = new Article()
                    {
                        Title = job.ComName + "诚聘" + job.JobName,
                        Url = WebConfigurationManager.AppSettings["domain"] + "/html/detail.html?id=" + job.JobId,
                        PicUrl = imgUrl,
                        Description = ""
                    };
                    articles.Add(art1);
                    i++;
                }
                var rest = Senparc.Weixin.MP.AdvancedAPIs.Custom.CustomApi.SendNews(accessToken, "oJvzIt8go_mNhCAVE-M0D5EexLpU", articles);
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
        #endregion


    }
}