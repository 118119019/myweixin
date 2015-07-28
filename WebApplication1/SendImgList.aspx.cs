using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class SendImgList : BaseAuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)Page.FindControl("img" + i.ToString());
                img.ImageUrl = string.Format("image/send{0}.jpg?time={1}", i, DateTime.Now.ToString());
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

            for (int i = 0; i < 6; i++)
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
                            string saveName = "Send" + handleNum + ".jpg";
                            string filePath = Server.MapPath("image") + "\\" + saveName;
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

    }
}