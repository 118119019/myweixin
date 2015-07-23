using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.News
{
    public partial class NewsIndex : System.Web.UI.Page
    {
        protected string TitleName { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    var id = Request["id"];
                    TitleName = id == "1" ? "培训" : "资讯";
                    var cfgList = new NewsCfgOp().GetNewsCfgListByType(int.Parse(id));
                    rptList1.DataSource = cfgList;
                    rptList1.DataBind();

                }
                catch (Exception ex)
                {


                }
            }
        }
    }
}