using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongYanService.Model
{
    public class TableJson
    {
        public List<MyLink> myLinkList { get; set; }
        public MyPage mypage { get; set; }
    }
    public class MyLink
    {
        public string Company { get; set; }
        public string Url { get; set; }
        public string Work { get; set; }
        public string Num { get; set; }
        public string Time { get; set; }
    }

    public class MyPage
    {
        public string Previous { get; set; }
        public string Next { get; set; }

        public string Desc { get; set; }
        public string SelPage { get; set; }
    }
}
