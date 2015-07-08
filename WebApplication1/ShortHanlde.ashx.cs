using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WebApplication1
{
    /// <summary>
    /// ShortHanlde 的摘要说明
    /// </summary>
    public class ShortHanlde : IHttpHandler
    {

        public HttpResponse Response;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var Request = context.Request;
            Response = context.Response;
            var Server = context.Server;
            string signature = Request["signature"];
            string timestamp = Request["timestamp"];
            string nonce = Request["nonce"];
            string echostr = Request["echostr"];
            string Token = "myweixin";
            if (Request.HttpMethod == "GET")
            {
                //get method - 仅在微信后台填写URL验证时触发
                if (CheckSignature.Check(signature, timestamp, nonce, Token))
                {
                    WriteContent(echostr); //返回随机字符串则表示验证通过
                }
                else
                {
                    WriteContent("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token) + "。" +
                                "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
                }
                Response.End();
            }
            else
            {

                //post method - 当有用户向公众账号发送消息时触发
                if (!CheckSignature.Check(signature, timestamp, nonce, Token))
                {
                    WriteContent("参数错误！");
                    return;
                }

                //post method - 当有用户向公众账号发送消息时触发
                var postModel = new PostModel()
                {
                    Signature = Request.QueryString["signature"],
                    Msg_Signature = Request.QueryString["msg_signature"],
                    Timestamp = Request.QueryString["timestamp"],
                    Nonce = Request.QueryString["nonce"],
                    //以下保密信息不会（不应该）在网络上传播，请注意
                    Token = Token,
                    EncodingAESKey = "T79NgH9wXHWta4dwPqcGxx1z92YAl4hSreDiIkZfRWo",//根据自己后台的设置保持一致
                    AppId = WebConfigurationManager.AppSettings["ShortWeixinAppId"]//根据自己后台的设置保持一致
                };
                var maxRecordCount = 10;
                var messageHandler = new ShortCustomHandler(Request.InputStream, postModel, maxRecordCount);
                try
                {
                    messageHandler.Execute();
                    WriteContent(messageHandler.ResponseDocument.ToString());
                    return;
                }
                catch (Exception ex)
                {
                    using (TextWriter tw = new StreamWriter(Server.MapPath("~/App_Data/Error_" + DateTime.Now.Ticks + ".txt")))
                    {
                        tw.WriteLine(ex.Message);
                        tw.WriteLine(ex.InnerException.Message);
                        if (messageHandler.ResponseDocument != null)
                        {
                            tw.WriteLine(messageHandler.ResponseDocument.ToString());
                        }
                        tw.Flush();
                        tw.Close();
                    }
                }
                finally
                {
                    Response.End();
                }
            }
        }

        private void WriteContent(string str)
        {
            Response.Output.Write(str);
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}