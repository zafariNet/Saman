using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Support.Validations;

namespace Model.Support
{
    public class SupportStatus:EntityBase,IAggregateRoot
    {
        /// <summary>
        /// نام وضعیت پشتیبانی
        /// </summary>
        public virtual string SupportStatusName { get; set; }

        /// <summary>
        /// وضعیت های وابسته
        /// </summary>
        public virtual IEnumerable<SupportStatusRelation> SuportStatusRelations { get; set; }

        /// <summary>
        /// آیا از وضعیت های اولیه است ؟
        /// </summary>
        public virtual bool IsFirstSupportStatus { get; set; }

        /// <summary>
        /// آیا از وضعیت های نهایی است؟
        /// </summary>
        public virtual bool IsLastSupportState { get; set; }
        /// <summary>
        /// متن ارسال توسط پیامک
        /// </summary>
        public virtual string SmsText { get; set; }

        /// <summary>
        /// متن ارسال توسط ایمیل
        /// </summary>
        public virtual string EmailText { get; set; }

        /// <summary>
        /// ؟آیا به محض ورود پیامک ارسال شود
        /// </summary>
        public virtual bool SendSmsOnEnter { get; set; }

        /// <summary>
        /// آیا به محض ورود ایمیل ارسال شود؟
        /// </summary>
        public virtual bool SendEmailOnEnter { get; set; }

        /// <summary>
        /// کلید
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// پشتیبانی های دارای این وضعیت
        /// </summary>
        //public virtual IEnumerable<Support> Supports { get; set; }

        protected override void Validate()
        {
            if (this.SupportStatusName == null || string.IsNullOrEmpty(this.SupportStatusName))
            {
                base.AddBrokenRule(SupportStatusBusinessRule.SupprotStatusNameReqired);
            }
            if (this.SendEmailOnEnter == true && string.IsNullOrEmpty(this.EmailText))
            {
                base.AddBrokenRule(SupportStatusBusinessRule.EmailTextRequired);
            }
            if (this.SendSmsOnEnter == true && string.IsNullOrEmpty(this.SmsText))
            {
                base.AddBrokenRule(SupportStatusBusinessRule.SmsTextRequired);
            }
        }
    }
}
