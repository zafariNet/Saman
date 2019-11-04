using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Fiscals.Validations;
using Model.Employees;

namespace Model.Fiscals
{
    public class MoneyAccountEmployee : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// کارمند تأیید کننده دریافت
        /// </summary>
        public virtual Employee Employee { get; set; }
        /// <summary>
        /// حساب مالی جهت تأیید دریافت
        /// </summary>
        public virtual MoneyAccount MoneyAccount { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Employee == null)
                base.AddBrokenRule(MoneyAccountEmployeeBusinessRules.EmployeeRequired);
            if (this.MoneyAccount == null)
                base.AddBrokenRule(MoneyAccountEmployeeBusinessRules.MoneyAccountRequired);
        }  
    }
}
