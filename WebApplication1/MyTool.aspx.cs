
using NLog;
using Oracle.ManagedDataAccess.Client;
using Quartz;
using Quartz.Impl;
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
            var accessToken = AccessTokenContainer.TryGetToken(WebConfigurationManager.AppSettings["LongNameAppId"],
              WebConfigurationManager.AppSettings["LongNameAppSecret"]);
            OpenIdResultJson json = UserApi.Get(accessToken, "");
            NewsModel[] newsList = new NewsModel[3];


            MediaList_NewsResult newsResult = MediaApi.GetNewsMediaList(accessToken, 0, 10);

            foreach (var item in newsResult.item)
            {

                txtResult.Text += item.media_id + " ";
                foreach (var tt in item.content.news_item)
                {
                    txtResult.Text += tt.thumb_media_id + " ";
                }


            }


            // MediaApi.UploadNews(accessToken, "", 0, null);

            var imgResult = MediaApi.GetOthersMediaList(accessToken, Senparc.Weixin.MP.UploadMediaFileType.image, 0, 4);
            //img列表
            foreach (var item in imgResult.item)
            {
                txtResult.Text += item.media_id + " " + item.name;
            }

            var dataSevice = new DataAccessSerive();
            var jobList = dataSevice.GetTopJobInfoList();
            if (jobList.Count > 0)
            {
                List<Article> articles = new List<Article>();
                int i = 0;
                foreach (var job in jobList)
                {
                    string imgUrl = string.Format("{0}/image/{1}.jpg", WebConfigurationManager.AppSettings["domain"], i);
                    if (i == 0)
                    {
                        var news = new NewsModel()
                        {
                            author = "",
                            content = @"
<p>
    <img width='100%' src='https://mmbiz.qlogo.cn/mmbiz/iaXDmvibibwTLXGYrdN08iabrTlgiaRDb9uvHAhGeOLibAZdiaF5BiapfMD08dvHIXKUvHnqvcIWK9XrD1G9pamRhEMWibA/0'/>
</p>
 
<fieldset style='border: 0px; margin: 0.5em 0px; padding: 0px; box-sizing: border-box;' class='wxqq-borderTopColor'>
    <section style='margin-left: 1%;border: 1px solid rgb(0, 187, 236); border-top-left-radius: 0px; border-top-right-radius: 5px; border-bottom-right-radius: 5px; 
border-bottom-left-radius: 0px; font-size: 1em; font-family: inherit; font-weight: inherit; text-align: inherit; text-decoration: inherit; color: rgb(52, 54, 60); 
background-color: rgb(255, 255, 255); box-sizing: border-box;' class='wxqq-borderTopColor wxqq-borderRightColor wxqq-borderBottomColor wxqq-borderLeftColor'>
        <section style='margin-top: 5%; float: left; margin-right: 8px; margin-left: -8px; font-size: 0.8em; font-family: inherit; font-style: inherit;
            font-weight: inherit; text-decoration: inherit; color: rgb(255, 255, 255); background-color: transparent; border-color: rgb(0,187,236); box-sizing: border-box;' 
        class='wxqq-borderTopColor wxqq-borderRightColor wxqq-borderBottomColor wxqq-borderLeftColor'>
            <span style='display: inline-block; padding: 0.3em 0.5em; border-top-left-radius: 0px; border-top-right-radius: 0.5em; border-bottom-right-radius: 0.5em; 
                border-bottom-left-radius: 0px; font-size: 1em; background-color: rgb(0,187,236); font-family: inherit; box-sizing: border-box;' class='wxqq-bg'>
        <section class='wxqq-borderTopColor' style='box-sizing: border-box;'>
                福建丹海床垫有限公司
            </section></span>
            <section style='width: 0px; border-right-width: 4px; border-right-style: solid; border-right-color: rgb(0,187,236); border-top-width: 4px; 
                border-top-style: solid; border-top-color: rgb(0,187,236); border-left-width: 4px !important; border-left-style: solid !important; 
        border-left-color: transparent !important; border-bottom-width: 4px !important; border-bottom-style: solid !important; border-bottom-color: transparent !important;
        box-sizing: border-box;' class='wxqq-borderTopColor wxqq-borderRightColor wxqq-borderBottomColor wxqq-borderLeftColor'></section>
        </section>
        <section style='margin-top: 5%; padding: 0px 8px; font-size: 1.5em; font-family: inherit; font-weight: inherit; text-align: inherit; text-decoration: inherit; box-sizing: border-box;' class='wxqq-borderTopColor'>
            <section class='wxqq-borderTopColor' style='box-sizing: border-box;'>
                <br/>
            </section>
        </section>
        <section style='clear: both; box-sizing: border-box;' class='wxqq-borderTopColor'></section>
        <section style='padding: 8px; box-sizing: border-box;' class='wxqq-borderTopColor'>
            <p>
                公司简介
            </p>
            <p>
                <br/>
            </p>
            <p>
                职位详情
            </p>
            <blockquote class='wxqq-borderTopColor wxqq-borderRightColor wxqq-borderBottomColor wxqq-borderLeftColor' 
style='margin: 0px; padding: 15px; border: 3px dashed rgb(0, 187, 236); border-top-left-radius: 10px; 
border-top-right-radius: 10px; border-bottom-right-radius: 10px; border-bottom-left-radius: 10px;'>
                <p>
                    <span style='font-family: inherit; font-size: 1em; font-weight: inherit; text-align: inherit; text-decoration: inherit;'>
招聘职位: &nbsp; &nbsp;部门经理 　 &nbsp; &nbsp;</span><br/>
                </p>
                <p>
                    工作方式: &nbsp; &nbsp;全职 　&nbsp; &nbsp; &nbsp;
                </p>
                <p>
                    最低月薪: &nbsp; &nbsp;4500　 &nbsp; &nbsp;
                </p>
                <p>
                    招聘人数: &nbsp; &nbsp;2人，其中：男0人，女0人，不限2人　 &nbsp; &nbsp;招聘对象: &nbsp; &nbsp;
                </p>
                <p>
                    年龄： &nbsp; &nbsp;不限 　 &nbsp; &nbsp;身高: &nbsp; &nbsp;视力: &nbsp; &nbsp;户口要求: &nbsp; &nbsp;不限 　 &nbsp; &nbsp;
                </p>
                <p>
                    文化程度: &nbsp; &nbsp;大专 　 &nbsp; &nbsp;技术等级: &nbsp; &nbsp;无/未说明 　 &nbsp; &nbsp;职业资格证书: &nbsp;&nbsp;
                </p>
                <p>
                    要求专业: &nbsp; &nbsp;用工形式: &nbsp; &nbsp;联系方式: &nbsp; &nbsp;
                </p>
                <p>
                    登记日期: &nbsp; &nbsp;2015-6-8　 &nbsp; &nbsp;有效日期: &nbsp; &nbsp;2015-7-8　 &nbsp; &nbsp;
                </p>
                <p>
                    其它要求: &nbsp; &nbsp;具有良好的沟通能力及销售技巧
                </p>
            </blockquote>
            <p>
                联系方式
            </p>
            <blockquote class='wxqq-borderTopColor wxqq-borderRightColor wxqq-borderBottomColor wxqq-borderLeftColor'
style='margin: 0px; padding: 15px; border: 3px dashed rgb(0, 187, 236); border-top-left-radius: 10px; border-top-right-radius: 10px; border-bottom-right-radius: 10px; border-bottom-left-radius: 10px;'>
                <p class='ue_t'>
                    可在这输入内容， 微信编辑器 - 微信图文排版,微信图文编辑器,微信公众号编辑器，微信编辑首选。
                </p>
            </blockquote>
            <p>
                <br/>
            </p>
            <p>
                <br/>
            </p>
        </section>
    </section>
</fieldset>
<p>
    <br/>
</p>
<p>
    <br/>
</p>
<p>
    <img width='100%' src='https://mmbiz.qlogo.cn/mmbiz/iaXDmvibibwTLUAqg2cUWlqgcoLtmRHicBkdPW9GUw6kA9UBX4m1jibPj9tgFQicbnxYrs3kNE1cxu4pqfYTsby1x2cw/0'/>
</p>

",
                            content_source_url = WebConfigurationManager.AppSettings["domain"] + "/html/detail.html?id=" + job.JobId,
                            digest = job.ComName + "诚聘" + job.JobName,
                            show_cover_pic = "1",
                            thumb_media_id = "", //imgResult.item.Find(p => p.name == i.ToString() + ".jpg").media_id,
                            title = job.ComName + "诚聘" + job.JobName
                        };
                        newsList[0] = news;
                    }
                    i++;
                }

                UploadForeverMediaResult mediaResult = MediaApi.UploadNews(accessToken, 1000, newsList);


                //Senparc.Weixin.MP.AdvancedAPIs.GroupMessage.GroupMessageApi.SendGroupMessageByGroupId
                //    (accessToken,groupId,)

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