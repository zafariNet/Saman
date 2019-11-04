using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers;
using Model.Employees;
using Model.Employees.Validations;
using Model.Sales.Validations;

namespace Model.Sales
{
    /// <summary>
    /// موجودیت پرداخت به عوامل تبلیغاتی
    /// </summary>
    public class CampaignPayment:EntityBase,IAggregateRoot
    {
        /// <summary>
        /// روش جذب
        /// </summary>
        public virtual SuctionModeDetail SuctionModeDetail { get; set; }

        /// <summary>
        /// عامل تبلیغاتی
        /// </summary>
        public virtual CampaignAgent CampaignAgent { get; set; }

        /// <summary>
        /// تاریخ پرداخت
        /// </summary>
        public virtual string PaymentDate { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }

        /// <summary>
        /// میزان پرداخت
        /// </summary>
        public virtual long Amount { get; set; }


        protected override void Validate()
        {
            if(this.SuctionModeDetail==null)
                AddBrokenRule(CampaignPaymentBusinessRule.SuctionModeDetailRequired);
            if(this.CampaignAgent == null)
                AddBrokenRule(CampaignPaymentBusinessRule.CampaignAgentRequired);
            if(string.IsNullOrEmpty(this.PaymentDate))
                AddBrokenRule(CampaignPaymentBusinessRule.PaymentDateRequired);
            if(this.Amount<=0)
                AddBrokenRule(CampaignPaymentBusinessRule.AmountRequired);
        }
    }
}
