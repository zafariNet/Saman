using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Employees;
using Model.Support.Validations;

namespace Model.Support
{
    /// <summary>
    /// موجودیت اعزام کارشناس
    /// </summary>
    public class SupportExpertDispatch:EntityBase,IAggregateRoot
    {
        /// <summary>
        /// پشتیبانی
        /// </summary>
        public virtual Support Support { get; set; }

        /// <summary>
        /// تاریخ اعزام
        /// </summary>
        public virtual string DispatchDate { get; set; }

        /// <summary>
        /// ساعت اعزام
        /// </summary>
        public virtual string DispatchTime { get; set; }

        /// <summary>
        /// کارشناس نصب
        /// </summary>
        public virtual Employee ExpertEmployee { get; set; }

        /// <summary>
        /// نام هماهنگ کننده
        /// </summary>
        public virtual string CoordinatorName { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Comment { get; set; }

        public virtual bool IsNewInstallation { get; set; }

        /// <summary>
        /// ارسال پیامک و ایمیل به کاربر
        /// </summary>
        public virtual bool SendNotificationToCustomer { get; set; }
        protected override void Validate()
        {
            if(this.Support ==null )
                base.AddBrokenRule(SupporExpertDispatchBusinessRule.SupportRequired);
            if (string.IsNullOrEmpty(this.DispatchDate))
                base.AddBrokenRule(SupporExpertDispatchBusinessRule.DispatchDateRequired);
            if (string.IsNullOrEmpty(this.DispatchTime))
                base.AddBrokenRule(SupporExpertDispatchBusinessRule.DispatchTimeRequired);
            if(string.IsNullOrEmpty(this.Comment))
                base.AddBrokenRule(SupporExpertDispatchBusinessRule.CommentRequired);
        }
    }
}
