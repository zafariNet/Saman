using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Support
{
    public class SupportTicketWaitingView:BaseView
    {
        /// <summary>
        /// تاریخ حضور کارشناس
        /// </summary>
        public string DateOfPersenceDate { get; set; }

        /// <summary>
        /// رنگ سیم
        /// </summary>
        public string WireColor { get; set; }

        /// <summary>
        /// SNR
        /// </summary>
        public string Snr { get; set; }

        /// <summary>
        /// SELT
        /// </summary>
        public string Selt { get; set; }

        /// <summary>
        /// کارشماس نصب
        /// </summary>
        public string InstallExpertName { get; set; }
        /// <summary>
        /// موضوع تیکت
        /// </summary>
        public string TicketSubject { get; set; }

        /// <summary>
        /// چک سر خط
        /// </summary>
        public bool SourceWireCheck { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// ارسال
        /// </summary>
        public bool SendNotificationToCustomer { get; set; }
    }
}
