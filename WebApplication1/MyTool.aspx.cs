
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
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)Page.FindControl("img" + i.ToString());
                img.ImageUrl = string.Format("image/{0}.jpg?time={1}", i, DateTime.Now.ToString());
            }
            if (!IsPostBack)
            {
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
                ddlWeek.SelectedValue = cronList[5];

                //保存
                //xmldoc.Save(@"E:\Test\Test\tt.xml");


                var accessToken = AccessTokenContainer.TryGetToken(WebConfigurationManager.AppSettings["LongNameAppId"],
                WebConfigurationManager.AppSettings["LongNameAppSecret"]);
                var json = GroupsApi.Get(accessToken);
                //foreach (var item in json.groups)
                //{
                //    ddlGropu.Items.Add(new ListItem(item.name, item.id.ToString()));
                //}
                //ddlGropu.Items.Add(new ListItem("全部", "-1"));

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

                //if (ddlGropu.SelectedValue != "-1")
                //{
                //    GroupMessageApi.SendGroupMessageByGroupId
                //      (accessToken, ddlGropu.SelectedValue, mediaResult.media_id, GroupMessageType.mpnews);
                //}
                //else
                //{
                try
                {
                    GroupMessageApi.SendGroupMessageByGroupId
              (accessToken, "-1", mediaResult.media_id, GroupMessageType.mpnews, true);
                    txtResult.Text += "提交成功一次服务号 推送成功";
                }
                catch (Exception ex)
                {

                    txtResult.Text += "提交成功一次服务号 推送失败 " + ex.Message;
                }



                // }


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

        protected void btnShortSendAll_Click(object sender, EventArgs e)
        {
            string tempStr = new WeixinHelpService().GetTemp(Server);

            var accessToken = AccessTokenContainer.TryGetToken(WebConfigurationManager.AppSettings["ShortWeixinAppId"],
              WebConfigurationManager.AppSettings["ShortWeixinSecret"]);
            OpenIdResultJson json = UserApi.Get(accessToken, "");

            var imgResult = MediaApi.GetOthersMediaList(accessToken, UploadMediaFileType.image, 0, 1000);
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
                try
                {
                    GroupMessageApi.SendGroupMessageByGroupId
                    (accessToken, "-1", mediaResult.media_id, GroupMessageType.mpnews, true);
                    txtResult.Text += "提交成功一次订阅号 推送成功";
                }
                catch (Exception ex)
                {
                    txtResult.Text += "提交成功一次订阅号 推送失败 " + ex.Message;
                }
            }
        }

        protected void btnSaveImg_Click(object sender, EventArgs e)
        {
            var shortAccessToken = AccessTokenContainer.TryGetToken(WebConfigurationManager.AppSettings["ShortWeixinAppId"],
                                 WebConfigurationManager.AppSettings["ShortWeixinSecret"]);
            var longAccessToken = AccessTokenContainer.TryGetToken(WebConfigurationManager.AppSettings["LongNameAppId"],
                                 WebConfigurationManager.AppSettings["LongNameAppSecret"]);
            var shortImgResult = MediaApi.GetOthersMediaList(shortAccessToken, UploadMediaFileType.image, 0, 1000);
            var longImgResult = MediaApi.GetOthersMediaList(longAccessToken, UploadMediaFileType.image, 0, 1000);

            for (int i = 0; i < 3; i++)
            {
                string handleNum = i.ToString();
                FileUpload fileUpload = (FileUpload)Page.FindControl("uploadImgUrl" + handleNum);
                Label lab = (Label)Page.FindControl("labImgError" + handleNum);
                if (fileUpload.HasFile)
                {
                    string fileExt = Path.GetExtension(fileUpload.FileName);
                    if (IsAllowableFileType(fileExt))
                    {
                        try
                        {
                            string saveName = handleNum + ".jpg";
                            string filePath = Server.MapPath("image") + "\\" + handleNum + ".jpg";
                            //保存图片
                            fileUpload.SaveAs(filePath);
                            lab.Text = "本地图片更新OK：〈br>";
                            //上传微信公众号后台
                            UploadImgToWeixin(shortAccessToken, shortImgResult, saveName, filePath);

                            UploadImgToWeixin(longAccessToken, longImgResult, saveName, filePath);

                            lab.Text += "微信公众号后台图片更新OK";
                        }
                        catch (Exception ex)
                        {
                            lab.Text = "发生错误：" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        lab.Text = "只允许上传.jpg文件！";
                    }
                }
                else
                {
                    lab.Text = "未上传图片！";
                }
            }

        }

        private static void UploadImgToWeixin(string accessToken, MediaList_OthersResult imgResult, string saveName, string filePath)
        {
            foreach (var item in imgResult.item)
            {
                if (item.name == saveName)
                {
                    MediaApi.DeleteForeverMedia(accessToken, item.media_id);
                }
            }
            MediaApi.UploadForeverMedia(accessToken, filePath);

        }
        protected bool IsAllowableFileType(string FileName)
        {
            //从web.config读取判断文件类型限制
            string strFileTypeLimit = ".jpg|.gif|.png|.bmp";
            //当前文件扩展名是否包含在这个字符串中
            if (strFileTypeLimit.IndexOf(Path.GetExtension(FileName).ToLower()) != -1)
            {
                return true;
            }
            else
                return false;
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
            cronList[5] = ddlWeek.SelectedValue;
            node.InnerText = string.Join(" ", cronList);
            xmlDoc.Save(Server.MapPath("quartz_jobs.xml"));
        }
    }
}