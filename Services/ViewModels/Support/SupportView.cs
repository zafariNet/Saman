using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Support
{
    public class SupportView:BaseView
    {
        public string CustomerName { get; set; }

        public Guid CustomerID { get; set; }

        /// <summary>
        /// تلفن مشتری
        /// </summary>
        public string ADSLPhone { get; set; }
        /// <summary>
        /// عنوان پشتیبانی
        /// </summary>
        public string SupportTitle { get; set; }

        /// <summary>
        /// توضیحات پشتیبانی
        /// </summary>
        public string SupportComment { get; set; }

        /// <summary>
        /// تایید شده؟
        /// </summary>
        public bool Confirmed { get; set; }

        /// <summary>
        /// شناسه وضعیت
        /// </summary>
        public Guid SupportStatusID { get; set; }

        /// <summary>
        /// نام وضعیت
        /// </summary>
        public string SupportStatusName { get; set; }

        /// <summary>
        /// اعزام کارشناس
        /// </summary>
        public  IEnumerable<SupportExpertDispatchView> SupportExpertDispatch { get; set; }

        /// <summary>
        /// نصب تلفنی
        /// </summary>
        public  IEnumerable<SupportPhoneInstallationView> SupportPhoneInstallation { get; set; }

        /// <summary>
        /// تاخیر در نصب
        /// </summary>
        public  IEnumerable<SupportInstallationDelayView> SupportInstallationDelay { get; set; }

        /// <summary>
        /// تحویل سرویس
        /// </summary>
        public  IEnumerable<SupportDeliverServiceView> SupportDeliverServices { get; set; }

        /// <summary>
        /// انتظار تیکت
        /// </summary>
        public  IEnumerable<SupportTicketWaitingView> SupportTicketWaiting { get; set; }

        /// <summary>
        /// منتظر پاسخ تیکت
        /// </summary>
        public  IEnumerable<SupportTicketWaitingResponseView> SupportTicketWaitingResponse { get; set; }

        /// <summary>
        /// فرم کیو سی
        /// </summary>
        public  IEnumerable<SupportQcView> SupportQc { get; set; }

        /// <summary>
        /// کیو سی مشکل دار
        /// </summary>
        public  IEnumerable<SupportQcProblemView> SupportQcProblem { get; set; }

        /// <summary>
        /// وضعیت پشتیبانی
        /// </summary>
        public  SupportStatusView SupportStatus { get; set; }

        public string LevelTitle { get; set; }

    }
}
