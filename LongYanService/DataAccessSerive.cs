using LongYanService.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongYanService
{
    public class WhereParam
    {

        public string Where { get; set; }
        public string ParameterName { get; set; }
        public string Value { get; set; }


    }


    public class JobInfo
    {
        public string ComName { get; set; }
        public string JobName { get; set; }
        public string JobId { get; set; }

    }
    public class JobDetail : JobInfo
    {
        public string ComId { get; set; }
        /// <summary>
        /// 所属劳动部门
        /// </summary>
        public string FromPlace { get; set; }
        /// <summary>
        /// 单位类型
        /// </summary>
        public string ComType { get; set; }
        /// <summary>
        /// 企业介绍
        /// </summary>
        public string ComBrief { get; set; }
        /// <summary>
        /// 工作方式
        /// </summary>
        public string JobType { get; set; }
        /// <summary>
        /// 最低月薪
        /// </summary>
        public string LowMoney { get; set; }
        /// <summary>
        /// 招聘人数
        /// </summary>
        public string HrNum { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }
        /// <summary>
        /// 文化要求
        /// </summary>
        public string Edu { get; set; }
        /// <summary>
        /// 登记日期
        /// </summary>
        public string RegisterDate { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public string EffectDate { get; set; }
        /// <summary>
        /// 其他要求
        /// </summary>
        public string Other { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string DetailPalce { get; set; }
        public string GetWorkOther { get; set; }

        public string Contact { get; set; }
        public string Sex { get; set; }
    }

    public class DataAccessSerive
    {
        public TableJson LoadEntities(string queryPage, int currentPageIndex, List<WhereParam> whereList)
        {
            string cmdWhere = "";
            List<OracleParameter> parameters = new List<OracleParameter>();
            whereList.ForEach(p =>
            {
                cmdWhere += string.Format(p.Where, p.ParameterName);
                parameters.Add(new OracleParameter()
                {
                    ParameterName = p.ParameterName,
                    Value = p.Value
                });
            });
            string sqlSelect = @"select count(*) from LYJYGD.ZP03 w   inner join LYJYGD.ZP01 c on w.ZPA001=C.ZPA001 
            where w.ZPC006=1 and w.ZPC010=0 {0} ";
            string commandText = string.Format(sqlSelect, cmdWhere);
            int count = 0;
            try
            {
                count = int.Parse(
            OracleHelper.ExecuteScalar(OracleHelper.ConnectionString, CommandType.Text, commandText, parameters).ToString()
        );
            }
            catch (Exception ex)
            {
                return null;
            }
            int pageSize = 10;
            sqlSelect = @"

select *
  from (select a.*, rownum rn
          from (select w.ZPC001,
                       w.ZPA001,
                       w.ZPA002,
                       w.ZPB003,
                       w.ZPC002,
                       w.ZPC004
                  from LYJYGD.ZP03 w
                 inner join LYJYGD.ZP01 c
                    on w.ZPA001 = C.ZPA001
                 where w.ZPC006 = 1
                   and w.ZPC010 = 0 {0} 
                 order by w.ZPC004 desc, w.ZPA002) a)
 where rn >{1}
   and rn <{2}
 
 ";
            commandText = string.Format(sqlSelect, cmdWhere, (currentPageIndex - 1) * pageSize, currentPageIndex * pageSize + 1);
            var reader = OracleHelper.ExecuteReader(OracleHelper.ConnectionString, CommandType.Text, commandText, parameters);
            StringBuilder sb = new StringBuilder();
            List<MyLink> myLinkList = new List<MyLink>();
            while (reader.Read())
            {
                try
                {
                    MyLink mylink = new MyLink();  //target=\"_blank\"
                    mylink.Url = string.Format("<a   href=\"detail.html?id={0}\">{1}</a>", reader["ZPC001"].ToString(), reader["ZPA002"]);
                    mylink.Work = reader["ZPB003"].ToString();
                    mylink.Company = reader["ZPA002"].ToString();
                    mylink.Num = reader["ZPC002"].ToString();
                    mylink.Time = ((DateTime)reader["ZPC004"]).ToString("yyyy-MM-dd");
                    myLinkList.Add(mylink);
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            MyPage page = new MyPage();
            int pageTotals = (int)(count / pageSize) + 1;

            if (currentPageIndex - 1 > 0)
            {
                page.Previous = string.Format("{0},'{1}'", queryPage, currentPageIndex - 1);
            }
            if (currentPageIndex + 1 <= pageTotals)
            {
                page.Next = string.Format("{0},'{1}'", queryPage, currentPageIndex + 1);
            }
            page.SelPage = pageTotals.ToString();
            page.Desc = string.Format("&nbsp;&nbsp;页次：<strong><font color=\"red\">{0}</font>/{1}</strong>页" +
                        " &nbsp;共<b>{2}</b>条信息<b>{3}</b>条信息/页  &nbsp;</div>",
                        currentPageIndex, page.SelPage, count, pageSize
                        );
            TableJson tableJson = new TableJson();
            tableJson.myLinkList = myLinkList;
            tableJson.mypage = page;
            return tableJson;
        }


        public JobDetail GetJobDetail(string id)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter()
            {
                ParameterName = "id",
                Value = id
            });
            string str = "";
            string sqlSelect = @" select w.ZPA001, w.ZPC001,w.ZPA002,w.ZPC008,c.ZPA016,c.ZPA017,w.ZPB003,w.ZPB011,w.ZPC003,w.ZPC002,
            w.ZPB006,w.ZPB005,w.ZPC004,w.ZPC005,w.ZPB007,w.ZPC002,w.ZPB004            
            from LYJYGD.ZP03 w inner join LYJYGD.ZP01 c on w.ZPA001=C.ZPA001 where w.ZPC001=:id";
            var reader = OracleHelper.ExecuteReader(OracleHelper.ConnectionString, CommandType.Text, sqlSelect, parameters);
            var jobDetail = new JobDetail();
            while (reader.Read())
            {
                jobDetail.ComId = ShowVal(reader["ZPA001"]);
                jobDetail.ComName = ShowVal(reader["ZPA002"]);
                jobDetail.ComBrief = ShowVal(reader["ZPA017"]);
                //联系人
                jobDetail = GetJobDetailContact(jobDetail);
                jobDetail.JobName = ShowVal(reader["ZPB003"]);
                jobDetail.JobType = reader["ZPB011"] == null ? "&nbsp;&nbsp;" : "全职";

                jobDetail.LowMoney = ShowVal(reader["ZPC003"]);
                jobDetail.HrNum = reader["ZPC002"].ToString();

                jobDetail.Edu = GetAA11Type(reader["ZPB005"].ToString(), "ZPB005");
                jobDetail.RegisterDate = ((DateTime)reader["ZPC004"]).ToString("yyyy-MM-dd");
                jobDetail.EffectDate = ((DateTime)reader["ZPC005"]).ToString("yyyy-MM-dd");
                jobDetail.Other = ShowVal(reader["ZPB007"]);
                jobDetail.Sex = getSex(reader["ZPB004"].ToString());
            }
            return jobDetail;
        }

        public string LoadDetail(string id)
        {
            //select ZPA034 from LYJYGD.ZP01A2 经济类型
            // 表ZP01A2  ZPA032   联系地址 ZPA030   联系人  ZPA031   联系电话
            // ZP01   ZPA017   企业简介
            StringBuilder sb = new StringBuilder();

            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                parameters.Add(new OracleParameter()
                {
                    ParameterName = "id",
                    Value = id
                });
                string str = "";
                string sqlSelect = @" select w.ZPA001, w.ZPC001,w.ZPA002,w.ZPC008,c.ZPA016,c.ZPA017,w.ZPB003,w.ZPB011,w.ZPC003,w.ZPC002,
            w.ZPB006,w.ZPB005,w.ZPC004,w.ZPC005,w.ZPB007,w.ZPC002,c.ZPA010,w.ZPB004            
            from LYJYGD.ZP03 w inner join LYJYGD.ZP01 c on w.ZPA001=C.ZPA001 where w.ZPC001=:id";
                var reader = OracleHelper.ExecuteReader(OracleHelper.ConnectionString, CommandType.Text, sqlSelect, parameters);
                while (reader.Read())
                {
                    sb.Append("<dl><dt>企业信息</dt><dd>");
                    sb.AppendFormat("<p><label>企业名称:</label><font color='#FF0000'>{0}</font></p>",
                        ShowVal(reader["ZPA002"]));
                    sb.AppendFormat("<p><label>所属劳动部门:</label>{0}</p>", ShowVal(reader["ZPC008"]));
                    sb.AppendFormat("<p><label>单位类型:</label>{0}</p>", GetAA11Type(reader["ZPA016"].ToString(), "ZPA016"));
                    sb.AppendFormat("<p><label>所属行业:</label>{0}</p>", GetAA11Type(reader["ZPA010"].ToString(), "ZPA010"));

                    sb.Append(GetContack(reader["ZPA001"].ToString()));
                    sb.AppendFormat("<p><label>企业介绍:</label>{0}</p>", ShowVal(reader["ZPA017"]));
                    sb.Append("</dd></dl>");
                    sb.Append("<dl><dt>招聘信息</dt><dd>");
                    sb.AppendFormat("<p><label>招聘职位:</label><font color='#FF0000'>{0}</font></p>", ShowVal(reader["ZPB003"]));
                    sb.AppendFormat("<p><label>工作方式:</label>{0}</p>", reader["ZPB011"] == null ? "&nbsp;&nbsp;" : "全职");
                    sb.AppendFormat("<p><label>最低月薪:</label>{0}</p>", ShowVal(reader["ZPC003"]));
                    sb.AppendFormat("<p><label>招聘人数:</label>{0}</p>", reader["ZPC002"].ToString());
                    sb.AppendFormat("<p><label>性别要求:</label>{0}</p>", getSex(reader["ZPB004"].ToString()));
                    //getSex(reader["ZPB004"].ToString()
                    //sb.Append(GetWorkOther(reader["ZPC002"].ToString(), id));
                    sb.AppendFormat("<p><label>年龄:</label>{0}</p>", ShowVal(reader["ZPB006"]));
                    sb.AppendFormat("<p><label>文化要求:</label>{0}</p>", GetAA11Type(reader["ZPB005"].ToString(), "ZPB005"));
                    sb.AppendFormat("<p><label>登记日期:</label>{0}</p>", ((DateTime)reader["ZPC004"]).ToString("yyyy-MM-dd"));
                    sb.AppendFormat("<p><label>有效日期:</label>{0}</p>", ((DateTime)reader["ZPC005"]).ToString("yyyy-MM-dd"));
                    sb.AppendFormat("<p><label>其他要求:</label>{0}</p>", ShowVal(reader["ZPB007"]));
                    sb.Append("</dd></dl>");
                }
            }
            catch (Exception ex)
            {


            }


            return sb.ToString();
        }


        protected string getSex(string str)
        {
            switch (str)
            {
                case "1":
                    return "男";
                case "2":
                    return "女";
                //9不限
            }
            return "不限";
        }

        public List<JobInfo> GetTopJobInfoList()
        {
            List<JobInfo> jobList = new List<JobInfo>();
            string sqlSelect = @" 
select *
  from (select *
          from (select ZPC001,
                       w.ZPA001,
                       w.ZPA002,
                       w.ZPB003,
                       w.ZPC002,
                       w.ZPC004,
                       rank() over(partition by w.ZPA001 order by w.ZPC004 desc, ZPC001) rank
                  from LYJYGD.ZP03 w
                 where w.ZPC006 = 1
                   and w.ZPC010 = 0) a
         where rank = 1
         order by ZPC004 desc) where rownum < 7  
                ";
            var reader = OracleHelper.ExecuteReader(sqlSelect);
            while (reader.Read())
            {
                try
                {
                    jobList.Add(
                        new JobInfo()
                        {
                            ComName = reader["ZPA002"].ToString(),
                            JobName = reader["ZPB003"].ToString(),
                            JobId = reader["ZPC001"].ToString()
                        }
                        );
                }
                catch (Exception ex)
                {

                }

            }
            return jobList;
        }

        private string ShowVal(object val)
        {
            if (val == null)
            {
                return "&nbsp;&nbsp;";
            }
            if (string.IsNullOrEmpty(val.ToString().Trim()))
            {
                return "&nbsp;&nbsp;";
            }
            return val.ToString();
        }

        /// <summary>
        /// 取得对应编码名称
        /// </summary>
        /// <returns></returns>
        private string GetAA11Type(string val, string name)
        {
            string sql = string.Format("select AAA103 from  LYJYGD.AA11 where AAA102='{0}' and AAA100='{1}'", val, name);
            var reader = OracleHelper.ExecuteReader(sql);
            while (reader.Read())
            {
                return reader[0].ToString();
            }
            return "";
        }

        private string GetContack(string cid)
        {
            string sql = string.Format("select ZPA032,ZPA030,ZPA031,ZPA043 from  LYJYGD.ZP01A2  where ZPA001='{0}'", cid);
            var reader = OracleHelper.ExecuteReader(sql);
            StringBuilder sb = new StringBuilder();
            while (reader.Read())
            {
                sb.AppendFormat("<p><label>详细地址:</label>{0}</p>", ShowVal(reader["ZPA032"]));
                sb.AppendFormat("<p><label>联系人:</label>{0}</p>", ShowVal(reader["ZPA030"]));
                sb.AppendFormat("<p><label>联系电话:</label>{0}</p>", ShowVal(reader["ZPA031"]));
                sb.AppendFormat("<p><label>电子邮件:</label>{0}</p>", ShowVal(reader["ZPA043"]));
            }
            return sb.ToString();
        }

        private JobDetail GetJobDetailContact(JobDetail job)
        {
            string sql = string.Format("select ZPA032,ZPA030,ZPA031 from  LYJYGD.ZP01A2  where ZPA001='{0}'", job.ComId);
            var reader = OracleHelper.ExecuteReader(sql);
            StringBuilder sb = new StringBuilder();
            while (reader.Read())
            {
                job.DetailPalce = ShowVal(reader["ZPA032"]);
                job.LinkMan = ShowVal(reader["ZPA030"]);
                job.Phone = ShowVal(reader["ZPA031"]);
            }
            return job;
        }

        private JobDetail GetJobDetailWorkOther(JobDetail job)
        {
            return null;

            //string sql = string.Format(" select * from LYJYGD.ZP03A1  where ZPC001='{0}'", wid);
            /////招聘人数:	1人，其中：男0人，女1人，不限0人　	招聘对象:	　
            ////年龄：	不限 　	身高:ZPC032   	 	视力:ZPC033   	 	户口要求:ZPC031    　
            ////文化程度:	ZPB005   　	技术等级:	无/未说明 　	职业资格证书:
            //var reader = OracleHelper.ExecuteReader(sql);
            //while (reader.Read())
            //{
            //    job.
            //    sb.AppendFormat("，其中：男{0}人，女{1}人，不限{2}人</p>", reader["ZPC024"], reader["ZPC025"], reader["ZPC026"]);
            //    sb.AppendFormat("<p><label>身高:</label> {0} </p>", ShowVal(reader["ZPC032"]));
            //    sb.AppendFormat("<p><label>视力:</label> {0} </p>", ShowVal(reader["ZPC033"]));
            //    sb.AppendFormat("<p><label>户口要求:</label>{0}", GetAA11Type(reader["ZPC031"].ToString(), "ZPC031"));
            //}
            //sb.Append("</p>");
            //return sb.ToString();
        }

        /// <summary>
        /// ZP03A1单位招聘信息副表
        /// </summary>
        private string GetWorkOther(string num, string wid)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p><label>招聘人数:</label>{0}", num);
            string sql = string.Format(" select * from LYJYGD.ZP03A1  where ZPC001='{0}'", wid);
            ///招聘人数:	1人，其中：男0人，女1人，不限0人　	招聘对象:	　
            //年龄：	不限 　	身高:ZPC032   	 	视力:ZPC033   	 	户口要求:ZPC031    　
            //文化程度:	ZPB005   　	技术等级:	无/未说明 　	职业资格证书:
            var reader = OracleHelper.ExecuteReader(sql);
            while (reader.Read())
            {
                sb.AppendFormat("，其中：男{0}人，女{1}人，不限{2}人</p>", reader["ZPC024"], reader["ZPC025"], reader["ZPC026"]);
                sb.AppendFormat("<p><label>身高:</label> {0} </p>", ShowVal(reader["ZPC032"]));
                sb.AppendFormat("<p><label>视力:</label> {0} </p>", ShowVal(reader["ZPC033"]));
                sb.AppendFormat("<p><label>户口要求:</label>{0}", GetAA11Type(reader["ZPC031"].ToString(), "ZPC031"));
            }
            sb.Append("</p>");
            return sb.ToString();
        }
    }
}
