using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;
using Infrastructure.Domain;
using Model.Support.Validations;

namespace Model.Support
{
    /// <summary>
    /// منتظر پاسخ تیکت
    /// </summary>
    public class SupportTicketWaitingResponse:EntityBase,IAggregateRoot
    {
        /// <summary>
        /// پشتیبانی
        /// </summary>
        public virtual Support Support { get; set; }

        /// <summary>
        /// تاریخ ارسال تیکت
        /// </summary>
        public virtual string SendTicketDate { get; set; }

        /// <summary>
        /// شماره تیکت
        /// </summary>
        public virtual string TicketNumber { get; set; }

        /// <summary>
        /// تاریخ احتمال پاسخ
        /// </summary>
        public virtual string ResponsePossibilityDate { get; set; }
        
        /// <summary>
        /// اارسال پیامک و ایمیل
        /// </summary>
        public virtual bool SendNotificationToCustomer { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Comment { get; set; }
        protected override void Validate()
        {
            if (this.Support == null)
                base.AddBrokenRule(SupportTicketWaitingResponseBusinessRule.SupportRequired);
            if (string.IsNullOrEmpty(this.SendTicketDate))
                base.AddBrokenRule(SupportTicketWaitingResponseBusinessRule.SendTicketDateRequired);
            if (string.IsNullOrEmpty(this.TicketNumber))
                base.AddBrokenRule(SupportTicketWaitingResponseBusinessRule.TicketNumberRequired);
            if (string.IsNullOrEmpty(this.ResponsePossibilityDate))
                base.AddBrokenRule(SupportTicketWaitingResponseBusinessRule.ResponsePossibilityDateRequired);
            if (string.IsNullOrEmpty(this.Comment))
                base.AddBrokenRule(SupportTicketWaitingResponseBusinessRule.CommentRequired);
        }
    }
}
