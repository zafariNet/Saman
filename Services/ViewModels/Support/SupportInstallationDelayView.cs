using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Support
{
    public class SupportInstallationDelayView:BaseView
    {

        /// <summary>
        /// تاریخ نصب
        /// </summary>
        public string InstallDate { get; set; }

        /// <summary>
        /// تاریخ تماس بعدی
        /// </summary>
        public string NextCallDate { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// ارسال پیامک و ایمیل به کاربر
        /// </summary>
        public bool SendNotificationToCustomer { get; set; }
    }
}
