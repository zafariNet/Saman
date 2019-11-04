using System.Dynamic;
using Infrastructure.Domain;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Model.Support.Validations;

namespace Model.Support
{
    /// <summary>
    /// موجودیت پشتیبانی
    /// </summary>
    public class Support : EntityBase, IAggregateRoot
    {
        public enum Creator
        {
            BySystem,
            ByOperator
        }
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// عنوان پشتیبانی
        /// </summary>
        public virtual string SupportTitle { get; set; }

        /// <summary>
        /// توضیحات پشتیبانی
        /// </summary>
        public virtual string SupportComment { get; set; }

        /// <summary>
        /// تایید شده؟
        /// </summary>
        public virtual bool Confirmed { get; set; }

        /// <summary>
        /// اعزام کارشناس
        /// </summary>
        public virtual IEnumerable<SupportExpertDispatch> SupportExpertDispatch { get; set; }

        /// <summary>
        /// نصب تلفنی
        /// </summary>
        public virtual IEnumerable<SupportPhoneInstallation> SupportPhoneInstallation { get; set; }

        /// <summary>
        /// تاخیر در نصب
        /// </summary>
        public virtual IEnumerable<SupportInstallationDelay> SupportInstallationDelay { get; set; }

        /// <summary>
        /// تحویل سرویس
        /// </summary>
        public virtual IEnumerable<SupportDeliverService> SupportDeliverService { get; set; }

        /// <summary>
        /// انتظار تیکت
        /// </summary>
        public virtual IEnumerable<SupportTicketWaiting> SupportTicketWaiting { get; set; }

        /// <summary>
        /// منتظر پاسخ تیکت
        /// </summary>
        public virtual IEnumerable<SupportTicketWaitingResponse> SupportTicketWaitingResponse { get; set; }

        /// <summary>
        /// فرم کیو سی
        /// </summary>
        public virtual IEnumerable<SupportQc> SupportQc { get; set; }

        /// <summary>
        /// کیو سی مشکل دار
        /// </summary>
        public virtual IEnumerable<SupportQcProblem> SupportQcProblem { get; set; }

        /// <summary>
        /// وضعیت پشتیبانی
        /// </summary>
        public virtual SupportStatus SupportStatus { get; set; }

        /// <summary>
        /// بسته یا باز بودن
        /// </summary>
        public virtual bool Closed { get; set; }

        /// <summary>
        /// ایجاد شده توسط
        /// </summary>
        public virtual Creator CreateBy { get; set; } 

        protected override void Validate()
        {
            if(this.Customer==null)
                base.AddBrokenRule(SupportBusinessRule.CustomerRequired);
            if(string.IsNullOrEmpty(this.SupportTitle))
                base.AddBrokenRule(SupportBusinessRule.SupportTitleRequired);
            if(string.IsNullOrEmpty(this.SupportComment))
                base.AddBrokenRule(SupportBusinessRule.SupportCommentRequired);
        }
    }
}
