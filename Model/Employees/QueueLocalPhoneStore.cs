using Infrastructure.Domain;
using Model.Base;
using Model.Employees.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Employees
{
    /// <summary>
    /// موجودیت صف داخلی
    /// </summary>
    public class QueueLocalPhoneStore:EntityBase,IAggregateRoot
    {
        /// <summary>
        /// شماره داخلی
        /// </summary>
        public virtual Employee OwnerEmployee { get; set; }

        /// <summary>
        /// صف مربوطه
        /// </summary>
        public virtual Queue Queue { get; set; }

        /// <summary>
        /// دقیقه خطرناک
        /// </summary>
        public virtual int DangerousSeconds { get; set; }

        /// <summary>
        /// تعداد زنگ حطرناک
        /// </summary>
        public virtual int DangerousRing { get; set; }

        /// <summary>
        /// ارسال اس ام اس در صورت آنلاین بودن کاربر
        /// </summary>
        public virtual bool SendSmsToOnLineUserOnDangerous { get; set; }

        /// <summary>
        /// ارسال اس ام اس در صورت آفلاین بودن کاربر
        /// </summary>
        public virtual bool SendSmsToOffLineUserOnDangerous { get; set; }

        /// <summary>
        /// آیا صف را ببیند
        /// </summary>
        public virtual bool CanViewQueue { get; set; }

        public virtual string SmsText { get; set; }

        protected override void Validate()
        {
            if (this.OwnerEmployee == null)
                base.AddBrokenRule(QueueLocalPhoneBusinessRule.LocalPhoneRequired);
            if (this.Queue == null)
                base.AddBrokenRule(QueueLocalPhoneBusinessRule.QueueRequired);
        }
    }
}
