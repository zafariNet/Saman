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
    /// موجودیت کیو سی
    /// </summary>
    public class SupportQc:EntityBase,IAggregateRoot
    {
        public enum State
        {
            Perfect=1,
            Good=2,
            Average=3,
            bad=4
        }

        /// <summary>
        /// پشتیبانی
        /// </summary>
        public virtual Support Support { get; set; }

        /// <summary>
        /// ساعت ورود
        /// </summary>
        public virtual string InputTime { get; set; }
        /// <summary>
        /// ساعت خروج
        /// </summary>
        public virtual string OutputTime { get; set; }

        /// <summary>
        /// رضایت از رفتار کارشناس
        /// </summary>
        public virtual State ExpertBehavior { get; set; }
        /// <summary>
        /// رضایت از پوشش کارشناس
        /// </summary>
        public virtual State ExpertCover { get; set; }

        /// <summary>
        /// رضایت از فروش و خدمات
        /// </summary>
        public virtual State SaleAndService { get; set; }

        /// <summary>
        /// مبلغ دریافتی
        /// </summary>
        public virtual long RecivedCost { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// اعلام به مسئول
        /// </summary>
        public virtual bool SendNotificationToMaster { get; set; }

        /// <summary>
        /// ارسال پیامک و ایمیل
        /// </summary>
        public virtual bool SendNotificationToCustomer { get; set; }



        public virtual bool HasProblem{
            get
            {
                //if (this.RecivedCost != this.)
                //    return true;
                return false;
            }
        }
        protected override void Validate()
        {
            if (this.Support == null)
                base.AddBrokenRule(SupportQcBusinessRule.SupportRequired);
            if (string.IsNullOrEmpty(this.InputTime))
                base.AddBrokenRule(SupportQcBusinessRule.InputTimeRequired);
            if (string.IsNullOrEmpty(this.OutputTime))
                base.AddBrokenRule(SupportQcBusinessRule.OutputTimeRequired);
            if (string.IsNullOrEmpty(this.Comment))
                base.AddBrokenRule(SupportQcBusinessRule.CommentRequired);
        }
    }
}
