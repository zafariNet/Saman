using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers;
using Model.Sales;

namespace Model.Employees
{
    public class BonusComission:EntityBase,IAggregateRoot
    {

        /// <summary>
        /// مشتری مربوطه
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// فروش کالا
        /// </summary>
        public virtual ProductSaleDetail ProductSaleDetail { get; set; }

        /// <summary>
        /// خدمات غیر اعتباری
        /// </summary>
        public virtual UncreditSaleDetail UnCreditSaleDetail { get; set; }

        /// <summary>
        /// خدمات اعتباری
        /// </summary>
        public virtual CreditSaleDetail CreditSaleDetail { get; set; }

        /// <summary>
        /// تاریخ تایید پیک
        /// </summary>
        public virtual string CourierDeliverDate { get; set; }

        /// <summary>
        /// تاریخ تحویل کالا
        /// </summary>
        public virtual string SaleDeliverDate { get; set; }

        /// <summary>
        /// پیک
        /// </summary>
        public virtual Courier Courier { get; set; }

        /// <summary>
        /// پورسانت
        /// </summary>
        public virtual long Comission { get; set; }

        public virtual long Bonus { get; set; }

        public virtual bool IsRollback { get; set; }

        public virtual long UnDeliveredComission { get; set; }

        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
