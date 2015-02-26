using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Work
{
    public partial class WorkList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadEntities();
        }

        private void LoadEntities()
        {
            var fbsj = ddlTime.SelectedValue;
            var gzgw = ddlWork.SelectedValue;
            var ssldbm = ddlPlace.SelectedValue;
            string url = string.Format(@"http://fjlylm.com/vizpxx.asp?ssldbm={0}&gzgw={1}&fbsj={2}", ssldbm, gzgw, fbsj);
            string content = CommonUtility.HttpUtility.Get(url, System.Text.Encoding.Default);


            int iTableStart = content.IndexOf("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"95%\" class=\"pix9\">", 0);
            if (iTableStart < 1)
            {
                return;
            }

            int iTableEnd = content.IndexOf("</table>", iTableStart);
            if (iTableEnd < 1)
            {

            }

            string strWeb = content.Substring(iTableStart, iTableEnd - iTableStart);

            //生成HtmlDocument
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(strWeb);
            List<MyLink> myLinkList = new List<MyLink>();
            
            

        }
    }
    
}