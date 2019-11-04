using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Support
{
    public class SupportQcView:BaseView
    {
        /// <summary>
        /// ساعت ورود
        /// </summary>
        public string InputTime { get; set; }
        /// <summary>
        /// ساعت خروج
        /// </summary>
        public string OutputTime { get; set; }

        /// <summary>
        /// رضایت از رفتار کارشناس
        /// </summary>
        public string ExpertBehavior { get; set; }
        /// <summary>
        /// رضایت از پوشش کارشناس
        /// </summary>
        public string  ExpertCover { get; set; }

        /// <summary>
        /// رضایت از فروش و خدمات
        /// </summary>
        public string SaleAndService { get; set; }

        /// <summary>
        /// مبلغ دریافتی
        /// </summary>
        public long RecivedCost { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// اعلام به مسئول
        /// </summary>
        public bool SendNotificationToMaster { get; set; }

        /// <summary>
        /// ارسال پیامک و ایمیل
        /// </summary>
        public bool SendNotificationToCustomer { get; set; }

        /// <summary>
        /// کارشناس نصب
        /// </summary>
        public string  InstallerEmployeeName { get; set; }

        /// <summary>
        /// مغایرت مالی
        /// </summary>
        public bool HasProblem { get; set; }

    }
}
