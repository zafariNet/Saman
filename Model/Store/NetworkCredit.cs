using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Store.Validations;

namespace Model.Store
{
    public class NetworkCredit : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// شبکه
        /// </summary>
        public virtual Network Network { get; set; }
        /// <summary>
        /// مبلغ افزایش یا کاهش
        ///در صورتی که افزایش اعتبار داشته باشیم این مبلغ مثبت و در صورت کاهش اعتبار منفی می باشد
        ///البته در نمایش همیشه مثبت است و از یک فیلد کمکی استفاده می کنیم که مشخص می کند افزایش بوده یا کاهش
        /// </summary>
        public virtual long Amount { get; set; }
        /// <summary>
        /// موجودی لحظه ای
        /// </summary>
        public virtual long Balance
        {
            get;
            //{
            //    return 
            //}
            set;
        }
        /// <summary>
        /// تاریخ واریز (در مورد افزایش اعتبار)
        /// </summary>
        public virtual string InvestDate { get; set; }
        /// <summary>
        /// حساب بانکی شرکت
        ///این فیلد از جدول حسابهای پولی می تواند پر شود (البته حسابهایی که از نوع حساب بانکی هستند)
        /// </summary>
        public virtual Fiscals.MoneyAccount FromAccount { get; set; }
        /// <summary>
        /// شماره حسابی که جهت شارژ پنل به آن پول واریز کرده ایم
        /// </summary>
        public virtual string ToAccount { get; set; }
        /// <summary>
        /// شماره فیش/تراکنش
        /// </summary>
        public virtual string TransactionNo { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }
        

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Network == null)
                base.AddBrokenRule(NetworkCreditBusinessRules.NetworkRequired);
        }  
    }
}
