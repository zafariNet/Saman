using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Support
{
    public class SupportDeliverServiceView:BaseView
    {
        /// <summary>
        /// تاریخ تحویل
        /// </summary>
        public string DeliverDate { get; set; }

        /// <summary>
        /// ساعت ورود
        /// </summary>
        public string TimeInput { get; set; }

        /// <summary>
        /// ساعت خروج
        /// </summary>
        public string TimeOutput { get; set; }

        /// <summary>
        /// مبلغ دریافتی
        /// </summary>
        public long AmountRecived { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// ارسال پیامک و ایمیل
        /// </summary>
        public bool SendNotificationToCustomer { get; set; }
    }
}
