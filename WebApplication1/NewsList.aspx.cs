using CommonService.Serilizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.News;
using LY.DataAccess;
using LiteDB;

namespace WebApplication1
{
    public partial class NewsList : BaseAuthPage
    {
        protected List<NewsType> typeList;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                List<NewsItem> list = new List<NewsItem>();
                using (LiteDatabase database = new LiteDatabase(LiteDbService.dbFilePath))
                {
                    list = database.GetCollection<NewsItem>("NewsItem").FindAll().ToList<NewsItem>();
                    typeList = database.GetCollection<NewsType>("NewsType").FindAll().ToList<NewsType>();
                }
                rptList1.DataSource = list;
                rptList1.DataBind();
            }
        }
        protected void rptList1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Docommand(e.CommandName, Convert.ToString(e.CommandArgument));
        }
        protected void Docommand(string command, string id)
        {
            if (command == "Edit")
            {
                base.RedirectUrl("NewsEdit.aspx?id=" + id);
            }
            if (command == "Del")
            {
                List<NewsItem> list = new List<NewsItem>();
                using (LiteDatabase database = new LiteDatabase(LiteDbService.dbFilePath))
                {
                    LiteCollection<NewsItem> collection = database.GetCollection<NewsItem>("NewsItem");
                    collection.Delete(int.Parse(id));
                    list = collection.FindAll().ToList();
                    typeList = database.GetCollection<NewsType>("NewsType").FindAll().ToList();
                }
                rptList1.DataSource = list;
                rptList1.DataBind();
            }

        }
        protected string showTypeName(string type)
        {
            NewsType type2 = this.typeList.Find(P => P.Id == int.Parse(type));
            if (type2 != null)
            {
                return type2.Name;
            }
            return "";

        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            RedirectUrl("NewsEdit.aspx");
        }


    }
}