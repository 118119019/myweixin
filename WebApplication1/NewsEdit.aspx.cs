using CommonService.Serilizer;
using HtmlAgilityPack;
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
    public partial class NewsEdit : BaseAuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                List<NewsItem> list = new List<NewsItem>();
                using (LiteDatabase database = new LiteDatabase(LiteDbService.dbFilePath))
                {
                    list = database.GetCollection<NewsItem>("NewsItem").FindAll().ToList<NewsItem>();
                    List<NewsType> list2 = database.GetCollection<NewsType>("NewsType").FindAll().ToList<NewsType>();
                    ddlType.DataSource = list2;
                    ddlType.DataTextField = "Name";
                    ddlType.DataValueField = "Id";
                    ddlType.DataBind();
                }
                string id = base.Request["id"] ?? "0";
                ViewState.Add("id", id);
                if (id != "0")
                {
                    NewsItem item = list.Find(p => p.Id == int.Parse(id));
                    if (item != null)
                    {
                        txtTitle.Text = item.Name;
                        if (!string.IsNullOrEmpty(item.HmtlContent))
                        {
                            hidContent.Value = item.HmtlContent.Replace("<", "&lt;").Replace(">", "&gt;").Replace("#", "%23");
                        }
                    }
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string id = this.ViewState["id"].ToString();
            if (string.IsNullOrEmpty(id))
            {
                return;
            }            
            NewsItem item = new NewsItem
            {
                HmtlContent = hidContent.Value.Replace("&lt;", "<").Replace("&gt;", ">").Replace("%23", "#"),
                Type = int.Parse(this.ddlType.SelectedValue),
                Name = this.txtTitle.Text.Trim(),
                Id = int.Parse(id)
            };
            if (id == "0")
            {
                using (LiteDatabase db = new LiteDatabase(LiteDbService.dbFilePath))
                {
                    db.GetCollection<NewsItem>("NewsItem").Insert(item);
                  
                }
            }
            else
            {
                using (LiteDatabase db = new LiteDatabase(LiteDbService.dbFilePath))
                {
                    db.GetCollection<NewsItem>("NewsItem").Update(int.Parse(id), item);
                }
            }             
            RedirectUrl("NewsList.aspx");
        }
    }
}