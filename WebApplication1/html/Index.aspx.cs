using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.html
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtRegDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtEffectDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
    }
}