#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;
using Infrastructure.Domain;
using Model.Customers;
using Model.Employees;
using Model.Fiscals.Validations;
#endregion

namespace Model.Fiscals
{
    
    public enum DocType
    {
        ATMReceipie = 1, // رسید عابربانک
        BankFish = 2, // فیش بانکی
        Cheque = 3, // چک
        PointOfSale = 4, // قبض صندوق
        //Others = 5 // سایر
        POSReceipie=5,//رسید POS
        Internet=6,//پرداخت اینترنتی
        Mahan=7,
        Others=8
    }

    public enum ConfirmEnum
    {
        NotChecked = 1, // بررسی نشده
        Confirmed = 2, // تأیید شده
        NotConfirmed = 3 // تأیید نشده
    }

    public enum ChargeStatus
    {
        NotChecked = 1, // بررسی نشده
        Charged = 2, // شارژ شده
        DisAgreement=3// تایید نشده
    }

    public class Fiscal : EntityBase, IAggregateRoot
    {
        
        /// <summary>
        /// مشتری
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// حساب پولی
        /// </summary>
        public virtual MoneyAccount MoneyAccount { get; set; }
        /// <summary>
        /// شماره سند مالی
        /// </summary>
        public virtual string DocumentSerial { get; set; }
        /// <summary>
        /// نوع سند: 1: رسید عابربانک - 2: فیش بانکی - 3: چک - 4: قبض صندوق
        /// </summary>
        public virtual DocType DocumentType{ get; set; }
        /// <summary>
        /// مبلغ مالی
        /// در صورتی که مبلغ مورد نظر دریافت باشد این عدد مثبت و در صورتی که پرداخت باشد این عدد منفی ذخیره می شود
        /// </summary>
        public virtual long Cost { get; set; }
        /// <summary>
        /// تاریخ واریز وجه (هنگامی که به حساب بانکی شرکت واریز شود)‏
        /// </summary>
        public virtual string InvestDate{ get; set; }
        /// <summary>
        /// شماره سریال مالی که در زمان تایید سند مالی به صورت اتوماتیک تولید میشود
        /// </summary>
        public virtual long AccountingSerialNumber { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }
        /// <summary>
        /// کارمندانی که می توانند این تراکنش را تأیید کنند
        /// </summary>
        public virtual IEnumerable<Employee> EmployeesWhoCanConfirm
        {
            get
            {
                return MoneyAccount.EmployeesWhoCanConfirm;
            }
        }
        /// <summary>
        /// وضعیت شارژ که برای پاسخگویی به سیستم ویپ می باشد
        /// </summary>
        public virtual ChargeStatus ChargeStatus { get; set; }
        /// <summary>
        /// شماره پیگیری که بصورت اتوماتیک ایجاد می شود و برای پیگیری تلفنی از طریق ویپ می باشد
        /// </summary>
        public virtual long FollowNumber { get; set; }
        /// <summary>
        /// شماره سریال که پس از تأیید ایجاد می شود و پیشوند آن از حسابهای پولی خوانده می شود
        /// </summary>
        public virtual long SerialNumber { get; set; }
        /// <summary>
        /// اگر مالی ایجاد شده مخصوص شارژ مشتری باشد مقدار این فیلد درست می شود
        /// </summary>
        public virtual bool ForCharge { get; set; }

        /// <summary>
        /// 1: Barresi nashode, 2: Taeed Shodeh, 3: Taeed Nashodeh
        /// </summary>
        public virtual ConfirmEnum Confirm { get; set; }
        /// <summary>
        /// مبلغی که توسط تأیید کننده مالی تأیید شده است. این مبلغ حداکثر به اندازه مبلغ مالی می باشد
        /// </summary>
        public virtual long? ConfirmedCost { get; set; }
        /// <summary>
        /// تاریخ تأیید مالی
        /// </summary>
        public virtual string ConfirmDate { get; set; }

        /// <summary>
        /// کارمند تأیید کننده
        /// </summary>
        public virtual Employee ConfirmEmployee { get; set; }

        /// <summary>
        /// شماره رسید مالی منحصر به فرد
        /// </summary>
        public virtual long FiscalReciptNumber { get; set; }

        protected override void Validate()
        {
            if (this.Customer == null)
                base.AddBrokenRule(FiscalBusinessRules.CustomerRequired);
            if (this.MoneyAccount == null)
                base.AddBrokenRule(FiscalBusinessRules.MoneyAccountRequired);
        }
    }
}
