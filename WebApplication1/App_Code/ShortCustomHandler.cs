using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace WebApplication1
{
    public class ShortCustomHandler : CustomMessageHandler
    {
        private   string agentUrl = WebConfigurationManager.AppSettings["ShortWeixinAgentUrl"];//这里使用了www.weiweihi.com微信自动托管平台
        private   string agentToken = WebConfigurationManager.AppSettings["ShortWeixinAgentToken"];//Token
        private   string wiweihiKey = WebConfigurationManager.AppSettings["ShortWeixinAgentWeiweihiKey"];//WeiweihiKey专门用于对接www.Weiweihi.com平台，获取方式见：http://www.weiweihi.com/ApiDocuments/Item/25#51
        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {

            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            if (requestMessage.Content != "")
            {
                var result = new StringBuilder();
                result.AppendLine("您好，欢迎关注龙岩就业微信公众平台！");
                result.AppendLine("");
                result.AppendLine("");
                result.AppendLine("龙岩市人力资源市场网");
                result.AppendLine("<a href=' www.fjlylm.com'>www.fjlylm.com</a>");
                result.AppendLine("<a href='http://www.fjlylm.com/zwxq.asp?id=58356'>龙岩畅丰专用汽车有限公司 最新招聘信息</a>");
                result.AppendLine("<a href='http://www.fjlylm.com/zwxq.asp?id=58301'>德泓（福建）光电科技有限公司 最新招聘信息</a>");
                result.AppendLine("<a href='http://www.fjlylm.com/zwxq.asp?id=57765'>福建绿河谷农牧有限公司  最新招聘信息</a>");
                result.AppendLine("更多信息请点击下面菜单");
                result.AppendLine("访问网站招聘求职就业资讯");
                responseMessage.Content = result.ToString(); 
            }
            //if (requestMessage.Content == "约束")
            //{
            //    responseMessage.Content = "<a href=\"http://weixin.senparc.com/FilterTest/\">点击这里</a>进行客户端约束测试（地址：http://weixin.senparc.com/FilterTest/）。";
            //}
            //if (requestMessage.Content == "托管" || requestMessage.Content == "代理")
            //{
            //    //开始用代理托管，把请求转到其他服务器上去，然后拿回结果
            //    //甚至也可以将所有请求在DefaultResponseMessage()中托管到外部。

            //    DateTime dt1 = DateTime.Now;//计时开始

            //    var responseXml = MessageAgent.RequestXml(this, agentUrl, agentToken, RequestDocument.ToString());//获取返回的XML
            //    //上面的方法也可以使用扩展方法：this.RequestResponseMessage(this,agentUrl, agentToken, RequestDocument.ToString());

            //    /* 如果有WeiweihiKey，可以直接使用下面的这个MessageAgent.RequestWeiweihiXml()方法。
            //     * WeiweihiKey专门用于对接www.weiweihi.com平台，获取方式见：http://www.weiweihi.com/ApiDocuments/Item/25#51
            //     */
            //    //var responseXml = MessageAgent.RequestWeiweihiXml(weiweihiKey, RequestDocument.ToString());//获取Weiweihi返回的XML

            //    DateTime dt2 = DateTime.Now;//计时结束

            //    //转成实体。
            //    /* 如果要写成一行，可以直接用：
            //     * responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
            //     * 或
            //     * 
            //     */
            //    responseMessage = responseXml.CreateResponseMessage() as ResponseMessageText;

            //    responseMessage.Content += string.Format("\r\n\r\n代理过程总耗时：{0}毫秒", (dt2 - dt1).Milliseconds);
            //}
            //else if (requestMessage.Content == "测试" || requestMessage.Content == "退出")
            //{
            //    /* 
            //    * 这是一个特殊的过程，此请求通常来自于微微嗨（http://www.weiweihi.com）的“盛派网络小助手”应用请求（http://www.weiweihi.com/User/App/Detail/1），
            //    * 用于演示微微嗨应用商店的处理过程，由于微微嗨的应用内部可以单独设置对话过期时间，所以这里通常不需要考虑对话状态，只要做最简单的响应。
            //    */
            //    if (requestMessage.Content == "测试")
            //    {
            //        //进入APP测试
            //        responseMessage.Content = "您已经进入【盛派网络小助手】的测试程序，请发送任意信息进行测试。发送文字【退出】退出测试对话。10分钟内无任何交互将自动退出应用对话状态。";
            //    }
            //    else
            //    {
            //        //退出APP测试
            //        responseMessage.Content = "您已经退出【盛派网络小助手】的测试程序。";
            //    }
            //}
            //else
            //{
            //    var result = new StringBuilder();
            //    result.AppendFormat("您刚才发送了文字信息：{0}\r\n\r\n", requestMessage.Content);

            //    if (CurrentMessageContext.RequestMessages.Count > 1)
            //    {
            //        result.AppendFormat("您刚才还发送了如下消息（{0}/{1}）：\r\n", CurrentMessageContext.RequestMessages.Count, CurrentMessageContext.StorageData);
            //        for (int i = CurrentMessageContext.RequestMessages.Count - 2; i >= 0; i--)
            //        {
            //            var historyMessage = CurrentMessageContext.RequestMessages[i];
            //            result.AppendFormat("{0} 【{1}】{2}\r\n",
            //                                historyMessage.CreateTime.ToShortTimeString(),
            //                                historyMessage.MsgType.ToString(),
            //                                (historyMessage is RequestMessageText)
            //                                    ? (historyMessage as RequestMessageText).Content
            //                                    : "[非文字类型]"
            //                );
            //        }
            //        result.AppendLine("\r\n");
            //    }

            //    result.AppendFormat("如果您在{0}分钟内连续发送消息，记录将被自动保留（当前设置：最多记录{1}条）。过期后记录将会自动清除。\r\n", WeixinContext.ExpireMinutes, WeixinContext.MaxRecordCount);
            //    result.AppendLine("\r\n");
            //    result.AppendLine("您还可以发送【位置】【图片】【语音】【视频】等类型的信息（注意是这几种类型，不是这几个文字），查看不同格式的回复。\r\nSDK官方地址：http://weixin.senparc.com");

            //    responseMessage.Content = result.ToString();
            //}
            return responseMessage;
        }

        public ShortCustomHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
            //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
            //比如MessageHandler<MessageContext>.GlobalWeixinContext.ExpireMinutes = 3。
            WeixinContext.ExpireMinutes = 3;
        }
    }
}