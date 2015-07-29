using HtmlAgilityPack;
using LongYanService;
using NLog;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TimePushConsole
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static MySchedulerService myscheduler = MySchedulerService.GetInstance();

        static void Main(string[] args)
        {

            try
            {
                SetConsoleCtrlHandler(cancelHandler, true);

                //SimpleJob job = new SimpleJob();
                //job.Test();
                myscheduler.Run();
            }
            catch (Exception ex)
            {
                logger.ErrorException(DateTime.Now.ToString() + " 推送失败 " + ex.Message, ex);
            }
            Console.ReadLine();
        }
        public delegate bool ControlCtrlDelegate(int CtrlType);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);
        private static ControlCtrlDelegate cancelHandler = new ControlCtrlDelegate(HandlerRoutine);

        public static bool HandlerRoutine(int CtrlType)
        {
            switch (CtrlType)
            {
                case 0:
                    Console.WriteLine("0工具被关闭中..."); //Ctrl+C关闭  
                    break;
                case 2:
                    Console.WriteLine("2工具被关闭中...");//按控制台关闭按钮关闭  
                    break;
            }
            myscheduler.ShutDown();
            Console.ReadLine();
            return false;
        }


    }
}
