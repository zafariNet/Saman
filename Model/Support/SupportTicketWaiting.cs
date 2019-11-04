using System.Dynamic;
using Infrastructure.Domain;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using Model.Support.Validations;

namespace Model.Support
{
    /// <summary>
    /// موجودیت انتظار تیکت
    /// </summary>
    public class SupportTicketWaiting : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// پشتیبانی
        /// </summary>
        public virtual Support Support { get; set; }
        
        /// <summary>
        /// تاریخ حضور کارشناس
        /// </summary>
        public virtual string DateOfPersenceDate { get; set; }

        /// <summary>
        /// رنگ سیم
        /// </summary>
        public virtual string WireColor { get; set; }

        /// <summary>
        /// SNR
        /// </summary>
        public virtual string Snr { get; set; }

        /// <summary>
        /// SELT
        /// </summary>
        public virtual string Selt { get; set; }

        /// <summary>
        /// کارشماس نصب
        /// </summary>
        public virtual Employee InstallExpert { get; set; }
        /// <summary>
        /// موضوع تیکت
        /// </summary>
        public virtual string TicketSubject { get; set; }

        /// <summary>
        /// چک سر خط
        /// </summary>
        public virtual bool SourceWireCheck { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// ارسال
        /// </summary>
        public virtual bool SendNotificationToCustomer { get; set; }
        protected override void Validate()
        {
            if (this.Support == null)
                base.AddBrokenRule(SupportTicketWaitingBusinessRule.SupportRequired);
            if (string.IsNullOrEmpty(this.DateOfPersenceDate))
                base.AddBrokenRule(SupportTicketWaitingBusinessRule.DateOfPersenceDateRequired);
            if (this.InstallExpert==null)
                base.AddBrokenRule(SupportTicketWaitingBusinessRule.InstallExpertRequired);
            if (string.IsNullOrEmpty(this.TicketSubject))
                base.AddBrokenRule(SupportTicketWaitingBusinessRule.TicketSubjectRequired);
            if (string.IsNullOrEmpty(this.Comment))
                base.AddBrokenRule(SupportTicketWaitingBusinessRule.CommentRequired);
        }
    }
}
