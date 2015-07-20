using HtmlAgilityPack;
using LongYanService;
using NLog;
using Quartz;
using Quartz.Impl;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace TimePushConsole
{
    public class MySchedulerService
    {

        private static MySchedulerService uniqueInstance;

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        // 定义私有构造函数，使外界不能创建该类实例
        private MySchedulerService()
        {
        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static MySchedulerService GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            // 双重锁定只需要一句判断就可以了
            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new MySchedulerService();
                    }
                }
            }
            return uniqueInstance;
        }

        public ISchedulerFactory sf;
        public IScheduler sched;
        private Logger log = LogManager.GetCurrentClassLogger();
        public void Run()
        {
            if (sf == null)
            {
                NameValueCollection properties = new NameValueCollection();
                properties["quartz.scheduler.instanceName"] = "XmlConfiguredInstance";

                // set thread pool info
                properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
                properties["quartz.threadPool.threadCount"] = "5";
                properties["quartz.threadPool.threadPriority"] = "Normal";

                // job initialization plugin handles our xml reading, without it defaults are used
                properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz";

                string jobXmlUrl = ConfigurationManager.AppSettings.Get("domain") + "/quartz_jobs.xml";
                string filepath = "quartz_jobs.xml";
                CommonUtility.HttpUtility.DownloadFile(jobXmlUrl, filepath);

                properties["quartz.plugin.xml.fileNames"] = filepath;
                sf = new StdSchedulerFactory(properties);
                log.Info("------- Initialization Complete -----------");
                log.Info("------- Not Scheduling any Jobs - relying on XML definitions --");
                log.Info("------- Starting Scheduler ----------------");
            }
            sched = sf.GetScheduler();
            // start the schedule 
            sched.Start();
            log.Info("------- Started Scheduler -----------------");
        }

        public void ShutDown()
        {
            if (sched != null)
            {
                // shut down the scheduler
                log.Info("------- Shutting Down ---------------------");
                sched.Shutdown(true);
                log.Info("------- Shutdown Complete -----------------");
                SchedulerMetaData metaData = sched.GetMetaData();
                log.Info("Executed " + metaData.NumberOfJobsExecuted + " jobs.");
            }
        }
    }
    public class SimpleJob : IJob
    {
        public const string Message = "msg";
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ResetPsdMailItem mailCfg = new ResetPsdMailItem()
          {

              Email = "xcbrmbtest@126.com",
              Id = 1,
              Name = "xcbrmbtest",
              Psd = "vubupcmbsjktodbj",
              Url = ""
          };
        private string GetTemp(string url)
        {
            HtmlDocument doc = new HtmlDocument();
            string path = "quartz_jobs.xml";
            //CommonUtility.HttpUtility.DownloadFile(url, path);
            //File.ReadAllText(path)
            doc.LoadHtml(CommonUtility.HttpUtility.Get(url));
            HtmlNode trNode = doc.DocumentNode.SelectSingleNode("//div[@id='content']");
            string str = trNode.InnerHtml;
            return trNode.InnerHtml;
        }


        public virtual void Execute(IJobExecutionContext context)
        {
            JobKey jobKey = context.JobDetail.Key;
            logger.Info("工作执行" + string.Format("推送! - {0}", DateTime.Now.ToString()));
            try
            {
                string webPath = ConfigurationManager.AppSettings.Get("domain");
                string tempStr = GetTemp(webPath + "/Temp.html");

                var accessToken = AccessTokenContainer.TryGetToken(
                ConfigurationManager.AppSettings.Get("ShortWeixinAppId"),
                ConfigurationManager.AppSettings.Get("ShortWeixinSecret"));
                PushNews(accessToken, webPath, tempStr);

                accessToken = AccessTokenContainer.TryGetToken(
                ConfigurationManager.AppSettings.Get("LongNameAppId"),
                ConfigurationManager.AppSettings.Get("LongNameAppSecret"));
                PushNews(accessToken, webPath, tempStr);
                var MailService = new MailSendFunc();
                MailService.SendMail("118119019@qq.com", "正常运行推送一次", "运行推送正常", mailCfg);
            }
            catch (Exception ex)
            {
                var MailService = new MailSendFunc();
                MailService.SendMail("118119019@qq.com", ex.Message, " 定时运行推送异常", mailCfg);
                logger.ErrorException(DateTime.Now.ToString() + " 定时执行推送失败 " + ex.Message, ex);
            }
            string message = context.JobDetail.JobDataMap.GetString(Message);
            logger.Info(string.Format("SimpleJob: {0} executing at {1}", jobKey, DateTime.Now.ToString()));
            logger.Info(string.Format("SimpleJob: msg: {0}", message));
        }


        public void PushNews(string accessToken, string webPath, string tempStr)
        {
            OpenIdResultJson json = UserApi.Get(accessToken, "");
            var imgResult = MediaApi.GetOthersMediaList(accessToken, UploadMediaFileType.image, 0, 1000);
            for (int i = 0; i < 3; i++)
            {
                string imgName = i.ToString() + ".jpg";
                if (imgResult.item.Find(p => p.name == imgName) == null)
                {
                    var filePath = webPath + "image\\" + imgName;
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
                    string domain = ConfigurationManager.AppSettings.Get("domain");
                    string imgUrl = string.Format("{0}/image/{1}.jpg", domain, i);
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
                        content_source_url = domain + "/html/detail.html?id=" + job.JobId,
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
                    Console.WriteLine("提交一次  推送成功");
                    logger.Info(DateTime.Now.ToString() + " 提交一次  推送成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("提交一次  推送失败 " + ex.Message);
                    var MailService = new MailSendFunc();
                    MailService.SendMail("118119019@qq.com", ex.Message, "提交一次推送异常", mailCfg);
                    logger.ErrorException(DateTime.Now.ToString() + " 提交一次  推送失败 " + ex.Message, ex);
                }
            }
        }

    }
}
