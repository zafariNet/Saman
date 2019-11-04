using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Support
{
    public class SupportQcProblemView:BaseView
    {
        /// <summary>
        /// زمان ورود کاشناس
        /// </summary>
        public string InputTime { get; set; }
        /// <summary>
        /// زمان خروج کارشناس
        /// </summary>
        public string OutputTime { get; set; }

        /// <summary>
        /// رضایت از رفتار کارشناس
        /// </summary>
        public int ExpertBehavior { get; set; }
        /// <summary>
        /// رضایت از پوشش کارشناس
        /// </summary>
        public int ExpertCover { get; set; }

        /// <summary>
        /// رضایت از فروش و خدمات
        /// </summary>
        public int SaleAndService { get; set; }



        /// <summary>
        /// مبلغ دریافتی
        /// </summary>
        public long RecivedCost { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// ارسال پیامک و ایمیل
        /// </summary>
        public bool SendNotificationToCustomer { get; set; }

        /// <summary>
        /// مغایرت مالی
        /// </summary>
        public string FiscallConfillict { get; set; }

        /// <summary>
        /// کارشناس نصب
        /// </summary>
        public string InstallerEmployeeName { get; set; }

        /// <summary>
        /// مشکل 
        /// </summary>
        public bool Problem { get; set; }
    }
}
