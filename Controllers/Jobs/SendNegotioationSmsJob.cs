using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using Infrastructure.Persian;
using Quartz;
using Services.Interfaces;

namespace Controllers.Jobs
{
    public class SendNegotioationSmsJob:IJob
    {
        public void Execute(IJobExecutionContext contect)
        {
            var timeFormat = new DateTimeFormatInfo
            {
                ShortTimePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern,
                AMDesignator = "AM",
                PMDesignator = "PM"
            };
            IList<NegotioationSms> list=new List<NegotioationSms>();

            var con = ConfigurationManager.ConnectionStrings["SamanCnn"].ToString();
            using (var myConnection=new SqlConnection(con))
            {
                string query =
                    "Select t1.ADSLPhone,t1.FirstName,t1.LastName,t2.NegotiationTime,t3.Title,t4.Mobile,t4.EmployeeID from Cus.Customer t1 Inner Join Lead.Negotiation t2 on t1.CustomerID=t2.CustomerID inner join Lead.LeadTitleTemplate t3 on t2.LeadTitleTemplateID=t3.LeadTitleTemplateID inner join emp.employee t4 on t2.ReferedEmployeeID=t4.EmployeeID where t2.RememberTime='" + DateTime.Now.ToString("t", timeFormat) + "' and t2.NegotiationDate='" + PersianDateTime.Now.Substring(0,10) + "' and SendSms=1";
                var oCmd = new SqlCommand(query, myConnection);
                myConnection.Open();
                using (SqlDataReader drd=oCmd.ExecuteReader())
                {
                    while (drd.Read())
                    {
                        list.Add(new NegotioationSms()
                        {
                            ADSLPhone = drd["ADSLPhone"].ToString(),
                            EmployeeID = Guid.Parse(drd["EmployeeID"].ToString()),
                            FirstName = drd["FirstName"].ToString(),
                            LastName = drd["LastName"].ToString(),
                            Mobile = drd["Mobile"].ToString(),
                            NegotationTime = drd["NegotiationTime"].ToString(),
                            Title = drd["Title"].ToString()
                        });
                    }

                }
                ISmsWebService smsService=new ISmsWebService();
                foreach (var item in list)
                {
                    var i=smsService.SendSms(
                        string.Format("همکار گرامی , کاربر شماره {0} در ساعت {1} منتظر تماس و پیگیری شماست",
                            item.ADSLPhone, item.NegotationTime), item.Mobile);
                    //string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Data/Log.txt");
                    //using (StreamWriter sw = new StreamWriter(path, true))
                    //{
                    //    sw.WriteLine(item.NegotationTime + DateTime.Now.ToString("t", timeFormat) + " " + item.Mobile + " " + i);
                    //}
                }
            }
        }
    }

    public class NegotioationSms
    {
        public string ADSLPhone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NegotationTime { get; set; }
        public string Title { get; set; }
        public string Mobile { get; set; }
        public Guid EmployeeID { get; set; }
    }
}
