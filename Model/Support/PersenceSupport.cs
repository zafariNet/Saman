#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Support.Validations;
using Model.Customers;
using Model.Employees;
#endregion

namespace Model.Support
{
    public class PersenceSupport : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// مشتری مشکل دار
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// نوع خدمات حضوری 
        ///1: نصب اولیه
        ///2: پشتیبانی حضوری
        /// </summary>
        public virtual short SupportType { get; set; }
        /// <summary>
        /// شرح مشکل (فقط برای پشتیبانی حضوری)
        /// </summary>
        public virtual string Problem { get; set; }
        /// <summary>
        /// تاریخ هماهنگی با مشتری
        /// </summary>
        public virtual string PlanDate { get; set; }
        /// <summary>
        /// ساعت هماهنگی از
        /// </summary>
        public virtual string PlanTimeFrom { get; set; }
        /// <summary>
        /// ساعت هماهنگی تا
        /// </summary>
        public virtual string PlanTimeTo { get; set; }
        /// <summary>
        /// توضیحات (مربوط به هماهنگی)‏
        /// </summary>
        public virtual string PlanNote { get; set; }
        /// <summary>
        /// کارشناس نصب (پس از نصب تکمیل می شود)
        /// </summary>
        public virtual Employee Installer { get; set; }
        /// <summary>
        /// تحویل شد/نشد (پس از نصب تکمیل می شود)‏
        /// </summary>
        public virtual bool Delivered { get; set; }
        /// <summary>
        /// مبلغ دریافتی (دستی) (پس از نصب تکمیل می شود)‏‏
        /// </summary>
        public virtual Int64 ReceivedCost { get; set; }
        /// <summary>
        /// مبلغ دریافتی بابت خدمات اضافه (دستی) (پس از نصب تکمیل می شود)‏
        /// </summary>
        //public virtual Int64 ReceivedCostForExtraServices { get; set; }
        /// <summary>
        /// کاربر به اینترنت وصل شد/ نشد (پس از نصب تکمیل می شود)‏
        /// </summary>
        public virtual bool ConnectedToInternet { get; set; }
        /// <summary>
        /// تاریخ تحویل  (پس از نصب تکمیل می شود)‏
        /// </summary>
        public virtual string DeliverDate { get; set; }
        /// <summary>
        /// ساعت تحویل (پس از نصب تکمیل می شود)‏
        /// </summary>
        public virtual string DeliverTime { get; set; }
        /// <summary>
        /// توضیحات تحویل  (پس از نصب تکمیل می شود)‏
        /// </summary>
        public virtual string DeliverNote { get; set; }

        #region Validation
        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Problem == null)
                base.AddBrokenRule(PersenceSupportBusinessRules.ProblemRequired);
            if (this.Customer == null)
                base.AddBrokenRule(PersenceSupportBusinessRules.CustomerRequired);
        }
        #endregion
    }
}
