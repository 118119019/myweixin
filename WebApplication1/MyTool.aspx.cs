
using NLog;
using Oracle.ManagedDataAccess.Client;
using Quartz;
using Quartz.Impl;
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
using WebApplication1.DataAccess;
using WebApplication1.Service;

namespace WebApplication1
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var accessToken = AccessTokenContainer.TryGetToken(WebConfigurationManager.AppSettings["LongNameAppId"],
                WebConfigurationManager.AppSettings["LongNameAppSecret"]);
                var json = GroupsApi.Get(accessToken);
                foreach (var item in json.groups)
                {
                    ddlGropu.Items.Add(new ListItem(item.name, item.id.ToString()));
                }
                ddlGropu.Items.Add(new ListItem("全部", "-1"));

                txtResult.Text = OracleHelper.CanConnect();
                if (txtResult.Text != "连接成功")
                {


                    var groupId = json.groups.Find(p => p.name == "开发小组").id.ToString();
                    var content = " oracle数据库无法连接 异常信息为" + txtResult.Text;
                    var sendResult = GroupMessageApi.SendTextGroupMessageByGroupId(accessToken, groupId, content, false);
                }

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
            //var accessToken = AccessTokenContainer.GetToken(_appId);
            //发送给指定分组文本小心
            // var sendResult = GroupMessageApi.SendTextGroupMessageByGroupId(accessToken, groupId, content, false);
            //http://rmb0595.gotoip3.com/cms/image/1.jpg

            //imgUrl = string.Format("{0}/image/{1}.jpg", WebConfigurationManager.AppSettings["domain"],
            //  "1");
            //art1 = new Article()
            //{
            //    PicUrl = imgUrl,
            //    Description = "福建网络",
            //    Title = "网龙集团" + "招聘",
            //    Url = ""
            //};
            //articles.Add(art1);
            //imgUrl = string.Format("{0}/image/{1}.jpg", WebConfigurationManager.AppSettings["domain"],
            //   "2");
            //art1 = new Article()
            //{
            //    PicUrl = imgUrl,
            //    Description = "福建网络",
            //    Title = "百度91无线" + "招聘",
            //    Url = ""
            //};
            // articles.Add(art1);


        }

        protected void btnSendAll_Click(object sender, EventArgs e)
        {
            string tempStr = new WeixinHelpService().GetTemp(Server);

            var accessToken = AccessTokenContainer.TryGetToken(WebConfigurationManager.AppSettings["LongNameAppId"],
              WebConfigurationManager.AppSettings["LongNameAppSecret"]);
            OpenIdResultJson json = UserApi.Get(accessToken, "");

            var imgResult = MediaApi.GetOthersMediaList(accessToken, UploadMediaFileType.image, 0, 4);
            for (int i = 0; i < 3; i++)
            {
                string imgName = i.ToString() + ".jpg";
                if (imgResult.item.Find(p => p.name == imgName) == null)
                {
                    var filePath = Server.MapPath("~/image/" + imgName);
                    var mediaId = MediaApi.UploadForeverMedia(accessToken, filePath).media_id;
                }
            }
            imgResult = MediaApi.GetOthersMediaList(accessToken, UploadMediaFileType.image, 0, 4);

            NewsModel[] newsList = new NewsModel[3];
            var dataSevice = new DataAccessSerive();
            var jobList = dataSevice.GetTopJobInfoList();
            if (jobList.Count > 0)
            {
                List<Article> articles = new List<Article>();
                int i = 0;
                foreach (var job in jobList)
                {
                    string imgUrl = string.Format("{0}/image/{1}.jpg", WebConfigurationManager.AppSettings["domain"], i);

                    var jobDetail = dataSevice.GetJobDetail(job.JobId);
                    var news = new NewsModel()
                  {
                      author = "",
                      content = tempStr.Replace("[ComName]", job.ComName)
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
                      ,
                      content_source_url = WebConfigurationManager.AppSettings["domain"] + "/html/detail.html?id=" + job.JobId,
                      digest = job.ComName + "诚聘" + job.JobName,
                      show_cover_pic = "0",
                      thumb_media_id = imgResult.item.Find(p => p.name == i.ToString() + ".jpg").media_id,
                      title = job.ComName + "诚聘" + job.JobName
                  };
                    newsList[i] = news;
                    i++;
                }


                UploadForeverMediaResult mediaResult = MediaApi.UploadNews(accessToken, 100000, newsList);

                if (ddlGropu.SelectedValue != "-1")
                {
                    GroupMessageApi.SendGroupMessageByGroupId
                      (accessToken, ddlGropu.SelectedValue, mediaResult.media_id, GroupMessageType.mpnews);
                }
                else
                {
                    GroupMessageApi.SendGroupMessageByGroupId
                     (accessToken, "-1", mediaResult.media_id, GroupMessageType.mpnews, true);
                }


                //foreach (var item in json.data.openid)
                //{
                //    try
                //    {
                //        //   var rest = CustomApi.SendNews(accessToken, item, articles);
                //    }
                //    catch (Exception ex)
                //    {

                //        throw;
                //    }

                //}
            }
        }
    }
}