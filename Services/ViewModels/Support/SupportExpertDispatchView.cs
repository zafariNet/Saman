using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Support
{
    public class SupportExpertDispatchView:BaseView
    {

        /// <summary>
        /// تاریخ اعزام
        /// </summary>
        public string DispatchDate { get; set; }

        /// <summary>
        /// ساعت اعزام
        /// </summary>
        public string DispatchTime { get; set; }

        /// <summary>
        /// کارشناس نصب
        /// </summary>
        public string ExpertEmployeeName { get; set; }


        /// <summary>
        /// شناسه کارشناس نصب
        /// </summary>
        public string ExpertEmployeeID { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// ارسال پیامک و ایمیل به کاربر
        /// </summary>
        public bool SendNotificationToCustomer { get; set; }

        public bool IsNewInstallation { get; set; }
    }
}
