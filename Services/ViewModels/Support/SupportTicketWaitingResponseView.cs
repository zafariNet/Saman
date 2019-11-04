using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Support
{
    public class SupportTicketWaitingResponseView:BaseView
    {
        /// <summary>
        /// تاریخ ارسال تیکت
        /// </summary>
        public string SendTicketDate { get; set; }

        /// <summary>
        /// شماره تیکت
        /// </summary>
        public string TicketNumber { get; set; }

        /// <summary>
        /// تاریخ احتمال پاسخ
        /// </summary>
        public string ResponsePossibilityDate { get; set; }

        /// <summary>
        /// اارسال پیامک و ایمیل
        /// </summary>
        public bool SendNotificationToCustomer { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Comment { get; set; }
    }
}
