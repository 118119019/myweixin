using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LY.DataAccess
{
    public class NewsItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string HmtlContent { get; set; }
    }
}
