using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Support.Validations;

namespace Model.Support
{
    public class SupportInstallationDelay:EntityBase,IAggregateRoot
    {

        /// <summary>
        /// پشتیبانی
        /// </summary>
        public virtual Support Support { get; set; }

        /// <summary>
        /// تاریخ نصب
        /// </summary>
        public virtual string InstallDate { get; set; }

        /// <summary>
        /// تاریخ تماس بعدی
        /// </summary>
        public virtual string NextCallDate { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// ارسال پیامک و ایمیل به کاربر
        /// </summary>
        public virtual bool SendNotificationToCustomer { get; set; }

        protected override void Validate()
        {
            if (this.Support == null)
                base.AddBrokenRule(SupportInstallationDelayBusinessRule.SupportRequired);
            if (string.IsNullOrEmpty(this.InstallDate))
                base.AddBrokenRule(SupportInstallationDelayBusinessRule.InstallDateRequired);
            if (string.IsNullOrEmpty(this.NextCallDate))
                base.AddBrokenRule(SupportInstallationDelayBusinessRule.NextCallDateRequired);
            if (string.IsNullOrEmpty(this.Comment))
                base.AddBrokenRule(SupportInstallationDelayBusinessRule.CommentRequired);
        }
    }
}
