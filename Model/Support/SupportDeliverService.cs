using System.Dynamic;
using System.Security;
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
    /// موجودیت تحویل سرویس
    /// </summary>
    public class SupportDeliverService : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// پشتیبانی
        /// </summary>
        public virtual Support Support { get; set; }
        
        /// <summary>
        /// تاریخ تحویل
        /// </summary>
        public virtual string DeliverDate { get; set; }
        
        /// <summary>
        /// ساعت ورود
        /// </summary>
        public virtual string TimeInput { get; set; }
        
        /// <summary>
        /// ساعت خروج
        /// </summary>
        public virtual string TimeOutput { get; set; }

        /// <summary>
        /// مبلغ دریافتی
        /// </summary>
        public virtual long AmountRecived { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// ارسال پیامک و ایمیل
        /// </summary>
        public virtual bool SendNotificationToCustomer { get; set; }

        protected override void Validate()
        {
            if (this.Support == null)
                base.AddBrokenRule(SupportDeliverServiceBusinessRule.SupportRequired);
            if (string.IsNullOrEmpty(this.DeliverDate))
                base.AddBrokenRule(SupportDeliverServiceBusinessRule.DeliverDateRequired);
            if (string.IsNullOrEmpty(this.TimeInput))
                base.AddBrokenRule(SupportDeliverServiceBusinessRule.TimeInputRequired);
            if (string.IsNullOrEmpty(this.TimeOutput))
                base.AddBrokenRule(SupportDeliverServiceBusinessRule.TimeOutputRequired);
            if (string.IsNullOrEmpty(this.Comment))
                base.AddBrokenRule(SupportInstallationDelayBusinessRule.CommentRequired);
        }
    }
}
