using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Employees
{
    public class LocalPhoneStoreEmployeeView : BaseView
    {
        /// <summary>
        /// شناسه داخلی
        /// </summary>
        public Guid LocalPhoneStoreID { get; set; }

        /// <summary>
        /// شماره داخلی
        /// </summary>
        public string LocalPhoneNumber { get; set; }

        /// <summary>
        /// شناسه استریسک
        /// </summary>
        public string AsteriskID { get; set; }

        /// <summary>
        /// شناسه کارمند
        /// </summary>
        public Guid OwnerEmployeeID { get; set; }
        /// <summary>
        /// نام کارمند
        /// </summary>
        public string OwnerEmployeeName { get; set; }

        /// <summary>
        /// دقیقه خطرناک
        /// </summary>
        public int DangerousSeconds { get; set; }

        /// <summary>
        /// تعداد زنگ حطرناک
        /// </summary>
        public int DangerousRing { get; set; }

        /// <summary>
        /// ارسال اس ام اس در صورت آنلاین بودن کاربر
        /// </summary>
        public bool SendSmsToOnLineUserOnDangerous { get; set; }

        /// <summary>
        /// ارسال اس ام اس در صورت آفلاین بودن کاربر
        /// </summary>
        public bool SendSmsToOffLineUserOnDangerous { get; set; }

        /// <summary>
        /// متن ارسال پیامک
        /// </summary>
        public string SmsText { get; set; }
    }
}
