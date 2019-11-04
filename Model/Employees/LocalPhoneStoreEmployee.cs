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
    /// موجودیت کارمندان و داخلی هایشان
    /// </summary>
    public class LocalPhoneStoreEmployee : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// داخلی مورد نظر
        /// </summary>
        public virtual LocalPhoneStore LocalPhoneStore { get; set; }

        /// <summary>
        /// کامند مربوطه
        /// </summary>
        public virtual Employee OwnerEmployee { get; set; }

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
        /// متن ارسال اس ام اس
        /// </summary>
        public virtual string SmsText { get; set; }

        

        protected override void Validate()
        {
            if (this.LocalPhoneStore == null)
                base.AddBrokenRule(LocalPhoneEmployeeBusinessRule.LocalPhoneStoreRequired);
            if (this.OwnerEmployee == null)
                base.AddBrokenRule(LocalPhoneEmployeeBusinessRule.OwnerEmployeeRequired);
        }
    }
}
