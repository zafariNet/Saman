using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Quartz;
using Services.Interfaces;

namespace Controllers.Jobs
{
    public class SendTaskSmsJob:IJob
    {
        public void Execute(IJobExecutionContext contect)
        {
            var timeFormat = new DateTimeFormatInfo
            {
                ShortTimePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern,
                AMDesignator = "AM",
                PMDesignator = "PM"
            };
            var con = ConfigurationManager.ConnectionStrings["SamanCnn"].ToString();
            using (var myConnection = new SqlConnection(con))
            {
                string query = "select t1.ToDoTitle,t1.t2.Mobile, from cus.Task where ";
                var oCmd = new SqlCommand(query, myConnection);
                myConnection.Open();
                using (SqlDataReader drd = oCmd.ExecuteReader())
                {
                    while (drd.Read())
                    {
                        ISmsWebService smsService = new ISmsWebService();
                    }
                }
            }
        }
    }
}
