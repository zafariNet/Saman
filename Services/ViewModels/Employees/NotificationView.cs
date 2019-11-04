using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Employees
{
    public class NotificationView : BaseView
    {
        public string NotificationTitle { get; set; }

        /// <summary>
        /// توضیحات پیام
        /// </summary>
        public string NotificationComment { get; set; }

        /// <summary>
        /// نوع پیام
        /// </summary>
        public string NotificationType { get; set; }

        /// <summary>
        /// ارجاع شده به کارمند
        /// </summary>
        public string ReferedEmployeeName { get; set; }

        public string VisitedDate { get; set; }
    }
}
