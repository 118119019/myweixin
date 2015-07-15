using net91com.Core.Util;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using WebApplication1;
using WebApplication1.Service;

namespace WebApplication1
{
    public class Global : HttpApplication
    {
        private MySchedulerService myscheduler = MySchedulerService.GetInstance(); 
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
          //  myscheduler.Run();
           
        }

        void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码
          //  myscheduler.ShutDown();
        }

        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码

        }
    }


}
