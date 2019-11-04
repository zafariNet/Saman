using Infrastructure.Domain;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Support.Validations;

namespace Model.Support
{
    /// <summary>
    /// موجودیت نصب تلفنی
    /// </summary>
    public class SupportPhoneInstallation : EntityBase, IAggregateRoot
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
        /// توضیحات
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// نصب شد؟
        /// </summary>
        public virtual bool Installed { get; set; }

        /// <summary>
        /// ارسال پیامک و ایمیل به کاربر
        /// </summary>
        public virtual bool SendNotificationToCustomer { get; set; }
        protected override void Validate()
        {
            if (this.Support == null)
                base.AddBrokenRule(SupportPhoneInstallationBusinessRule.SupportRequired);
            if (string.IsNullOrEmpty(this.InstallDate))
                base.AddBrokenRule(SupportPhoneInstallationBusinessRule.InstallDateRequired);
            if (string.IsNullOrEmpty(this.Comment))
                base.AddBrokenRule(SupportPhoneInstallationBusinessRule.CommentRequired);
        }
    }
}
