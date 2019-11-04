using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels
{
    public class SystemCountersView
    {
        #region Properties

        /// <summary>
        /// شناسه
        /// </summary>
        public  Guid ID { get; set; }
        /// <summary>
        /// آخرین سریال شماره فاکتو
        /// </summary>
        public int LastFactorSerialNumber { get; set; }

        /// <summary>
        /// آخرین شماره سریال قبض صندوق
        /// </summary>
        public int LastCashSerialNumber { get; set; }

        /// <summary>
        /// همه مشتریان
        /// </summary>
        public int AllCustomersCount { get; set; }

        /// <summary>
        /// ثبت نام
        /// </summary>
        public int RegisterCustomerCount { get; set; }

        /// <summary>
        /// پشتیبانی
        /// </summary>
        public int SupportCustomerCount { get; set; }

        /// <summary>
        /// بایگانی فعال
        /// </summary>
        public int ActiveArchiveCustomerCount { get; set; }

        /// <summary>
        /// واحد پشتیبانی
        /// </summary>
        public int SupportUnitCustomerCount { get; set; }

        /// <summary>
        /// عدم پشتیبانی موقت
        /// </summary>
        public int TemproryNotSupportedCustomerCount { get; set; }

        /// <summary>
        /// انتظار برای رانژه
        /// </summary>
        public int WaitForRanjeCustomerCount { get; set; }

        /// <summary>
        /// تماس های نهایی نشده
        /// </summary>
        public int NotFinalizedCallsCustomerCount { get; set; }

        /// <summary>
        /// بایگانی غیر فعال
        /// </summary>
        public int InActiveArchiveCustomerCount { get; set; }

        /// <summary>
        /// درحال جمع آوری
        /// </summary>
        public int RemoveingRanjeCustomerCount { get; set; }

        /// <summary>
        /// تکمیل مدارک
        /// </summary>
        public int CompeletingDocumentCustomerCount { get; set; }

        /// <summary>
        /// ارسال برای رانژه
        /// </summary>
        public int SendToRanjeCustomerCount { get; set; }

        /// <summary>
        /// شارژ و سایر خدمات ثانویه
        /// </summary>
        public int ChargeAndOthersCustomerCount { get; set; }

        /// <summary>
        /// جمع آوری شده
        /// </summary>
        public int RemovedUsersCustomerCount { get; set; }
        /// <summary>
        /// مشکل دار ها
        /// </summary>

        public int HasProblemsCustomerCount { get; set; }

        /// <summary>
        /// عدم پشتیبانی
        /// </summary>
        public int NotSupportedCustomerCount { get; set; }


        #endregion
    }
}
