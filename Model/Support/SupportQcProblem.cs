using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Employees;
using Model.Support.Validations;

namespace Model.Support
{
    /// <summary>
    /// موجودیت کیو سی مشکل دار
    /// </summary>
    public class SupportQcProblem : EntityBase, IAggregateRoot
    {
        public enum State
        {
            Perfect,
            Good,
            Average,
            bad
        }

        /// <summary>
        /// پشتیبانی
        /// </summary>
        public virtual Support Support { get; set; }

        /// <summary>
        /// زمان ورود کاشناس
        /// </summary>
        public virtual string InputTime { get; set; }
        /// <summary>
        /// زمان خروج کارشناس
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
        /// ارسال پیامک و ایمیل
        /// </summary>
        public virtual bool SendNotificationToCustomer { get; set; }

        /// <summary>
        /// مغایرت مالی
        /// </summary>
        public virtual string FiscallConfillict { get; set; }

        /// <summary>
        /// کارشناس نصب
        /// </summary>
        public virtual Employee InstallerEmployee { get; set; }

        /// <summary>
        /// مشکل 
        /// </summary>
        public virtual bool Problem { get; set; }
        protected override void Validate()
        {
            if (this.Support == null)
                base.AddBrokenRule(SupportQcProblemBusinnessRule.SupportRequired);
            if (string.IsNullOrEmpty(this.InputTime))
                base.AddBrokenRule(SupportQcProblemBusinnessRule.InputTimeRequired);
            if (string.IsNullOrEmpty(this.OutputTime))
                base.AddBrokenRule(SupportQcProblemBusinnessRule.OutputTimeRequired);
            if(string.IsNullOrEmpty(this.FiscallConfillict))
                base.AddBrokenRule(SupportQcProblemBusinnessRule.FiscallConfillictRequired);
            if (string.IsNullOrEmpty(this.Comment))
                base.AddBrokenRule(SupportQcProblemBusinnessRule.CommentRequired);
        }
    }
}
