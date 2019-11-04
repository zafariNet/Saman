using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;
using Infrastructure.Domain;
using Model.Employees;
using Model.Fiscals.Validations;
using Model.Store;

namespace Model.Fiscals
{
    /// <summary>
    /// حسابهای پولی اعم از دریافت و پرداخت و نیز حسابهای بانکی شرکت در این جدول ذخیره می شود
    /// </summary>
    public class MoneyAccount : EntityBase, IAggregateRoot
    {

        /// <summary>
        /// نام حساب پولی
        /// مثلا نقدی، صندوق، حساب بانکی و ...
        /// </summary>
        public virtual string AccountName { get; set; }
        /// <summary>
        /// آیا این حساب از نوع دریافت می باشد؟
        /// میتواند هم دریافت باشد هم پرداخت
        /// مثلا "نقدی". هم میتواند دریافت بصورت نقدی صورت گیرد هم پرداخت
        /// </summary>
        public virtual bool Receipt { get; set; }
        /// <summary>
        /// آیا این حساب از نوع پرداخت می باشد؟
        /// میتواند هم دریافت باشد هم پرداخت
        /// مثلا "نقدی". هم میتواند دریافت بصورت نقدی صورت گیرد هم پرداخت
        /// </summary>
        public virtual bool Pay { get; set; }
        /// <summary>
        /// آیا این حساب از نوع حساب بانکی می باشد؟
        /// در صورتی که مقدار آن 1 باشد، یکی از حسابهای بانکی شرکت در این فیلد ذخیره می شود
        /// </summary>
        public virtual bool IsBankAccount { get; set; }
        /// <summary>
        /// شماره حساب بانکی (در صورتی که حساب پولی از نوع حساب بانکی باشد)
        /// </summary>
        public virtual string BAccountNumber { get; set; }
        /// <summary>
        /// مشخصات و توضیحات حساب بانکی
        /// </summary>
        public virtual string BAccountInfo { get; set; }
        /// <summary>
        /// فعال یا غیر فعال بودن حساب پولی
        /// </summary>
        public virtual bool Discontinued { get; set; }
        /// <summary>
        /// سورت
        /// </summary>
        public virtual int SortOrder { get; set; }
        /// <summary>
        /// پیشوند شماره سریال مالی
        /// </summary>
        public virtual string FiscalPrefix { get; set; }
        /// <summary>
        /// آیدی حساب پولی مربوط به نرم افزار حسابداری
        /// </summary>
        public virtual string AccountingSoftwareID { get; set; }
        /// <summary>
        /// امور مالی
        /// </summary>
        public virtual IEnumerable<Fiscal> Fiscals { get; protected set; }
        /// <summary>
        /// کارمندان مجاز به تأیید
        /// </summary>
        public virtual IEnumerable<MoneyAccountEmployee> MoneyAccountEmployees { get; protected set; }

        /// <summary>
        /// دارای شماره رسید مالی منحصر به فرد
        /// </summary>
        public virtual bool HasUniqueSerialNumber { get; set; }
        /// <summary>
        /// آیا شماره سریال حتما 9 رقم باشد؟
        /// </summary>
        public virtual bool Has9Digits { get; set; }
        /// <summary>
        /// کارمندانی که می توانند این حساب مالی را تأیید کنند
        /// </summary>
        public virtual IEnumerable<Employee> EmployeesWhoCanConfirm
        {
            get
            {
                return MoneyAccountEmployees.Select(s => s.Employee);
            }
        }

        /// <summary>
        /// شبکه ها
        /// </summary>
        public virtual IEnumerable<Network> Networks { get; protected set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.AccountName == null)
                base.AddBrokenRule(MoneyAccountBusinessRules.AccountNameRequired);
        }
    
    }
}
