using Infrastructure.Domain;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SystemCounters :  IAggregateRoot
    {
        #region Properties

        /// <summary>
        /// شناسه
        /// </summary>
        public virtual Guid ID { get; set; }
        /// <summary>
        /// آخرین سریال شماره فاکتو
        /// </summary>
        public virtual int LastFactorSerialNumber { get; set; }

        /// <summary>
        /// آخرین شماره سریال قبض صندوق
        /// </summary>
        public virtual int LastCashSerialNumber { get; set; }

        /// <summary>
        /// همه مشتریان
        /// </summary>
        public virtual int AllCustomersCount { get; set; }

        /// <summary>
        /// ثبت نام
        /// </summary>
        public virtual int RegisterCustomerCount { get; set; }

        /// <summary>
        /// پشتیبانی
        /// </summary>
        public virtual int SupportCustomerCount { get; set; }

        /// <summary>
        /// بایگانی فعال
        /// </summary>
        public virtual int ActiveArchiveCustomerCount { get; set; }

        /// <summary>
        /// واحد پشتیبانی
        /// </summary>
        public virtual int SupportUnitCustomerCount { get; set; }

        /// <summary>
        /// عدم پشتیبانی موقت
        /// </summary>
        public virtual int TemproryNotSupportedCustomerCount { get; set; }

        /// <summary>
        /// انتظار برای رانژه
        /// </summary>
        public virtual int WaitForRanjeCustomerCount { get; set; }

        /// <summary>
        /// تماس های نهایی نشده
        /// </summary>
        public virtual int NotFinalizedCallsCustomerCount { get; set; }

        /// <summary>
        /// بایگانی غیر فعال
        /// </summary>
        public virtual int InActiveArchiveCustomerCount { get; set; }

        /// <summary>
        /// درحال جمع آوری
        /// </summary>
        public virtual int RemoveingRanjeCustomerCount { get; set; }

        /// <summary>
        /// تکمیل مدارک
        /// </summary>
        public virtual int CompeletingDocumentCustomerCount { get; set; }

        /// <summary>
        /// ارسال برای رانژه
        /// </summary>
        public virtual int SendToRanjeCustomerCount { get; set; }

        /// <summary>
        /// شارژ و سایر خدمات ثانویه
        /// </summary>
        public virtual int ChargeAndOthersCustomerCount { get; set; }

        /// <summary>
        /// جمع آوری شده
        /// </summary>
        public virtual int RemovedUsersCustomerCount { get; set; }
        /// <summary>
        /// مشکل دار ها
        /// </summary>

        public virtual int HasProblemsCustomerCount { get; set; }

        /// <summary>
        /// عدم پشتیبانی
        /// </summary>
        public virtual int NotSupportedCustomerCount { get; set; }


        #endregion

        protected  void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
