using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LiteDB;
using LY.DataAccess;

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
                    string id = base.Request["id"];
                    this.TitleName = (id == "1") ? "培训" : "资讯";
                    using (LiteDatabase db = new LiteDatabase(LiteDbService.dbFilePath))
                    {

                        List<NewsItem> list = db.GetCollection<NewsItem>("NewsItem").
                                Find(p => p.Type == int.Parse(id)).ToList();
                        this.rptList1.DataSource = list;
                        this.rptList1.DataBind();
                        NewsType type = db.GetCollection<NewsType>("NewsType").FindById(int.Parse(id));
                        if (type != null)
                        {
                            TitleName = type.Name;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}