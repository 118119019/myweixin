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
    public partial class NewsDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var id = Request["id"] ?? "0";
                this.ViewState.Add("id", id);
                if (id != "0")
                {
                    using (var db = new LiteDatabase(LiteDbService.dbFilePath))
                    {
                        var col = db.GetCollection<NewsItem>("NewsItem");
                        var cfg = col.FindById(int.Parse(id));
                        if (cfg != null)
                        {
                            content.InnerHtml = cfg.HmtlContent;
                        }
                    }
                }
            }
        }
    }
}