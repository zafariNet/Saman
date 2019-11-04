using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Employees;
using Model.Employees.Validations;
using Model.Sales.Validations;

namespace Model.Sales
{
    /// <summary>
    /// موجودیت پیک
    /// </summary>
    public class Courier:EntityBase,IAggregateRoot
    {
        public enum CourierTypes
        {
            Verbal,
            paid,
            Courier
        }

        public enum CourierStatuses
        {
            Pending,
            NotConfirmed,
            Confirmed,
            Canceled
        }
        /// <summary>
        /// فروش
        /// </summary>
        public virtual Sale Sale { get; set; }

        /// <summary>
        /// تاریخ اعزام پیک
        /// </summary>
        public virtual string DeliverDate { get; set; }

        /// <summary>
        /// زمان ارسال پیک
        /// </summary>
        public virtual string DeliverTime { get; set; }

        /// <summary>
        /// هزینه پیک
        /// </summary>
        public virtual long CourierCost { get; set; }
        
        /// <summary>
        /// مبلغ دریافتی
        /// </summary>
        public virtual long Amount { get; set; }
        
        /// <summary>
        /// توضیحات فروش
        /// </summary>
        public virtual string SaleComment { get; set; }
        
        /// <summary>
        /// توضیحات کارشناس
        /// </summary>
        public virtual string ExpertComment { get; set; }

        /// <summary>
        /// تعداد واحد ها
        /// </summary>
        public virtual int BuildingUnits { get; set; }

        /// <summary>
        /// نوع
        /// </summary>
        public virtual CourierTypes CourierType { get; set; }

        /// <summary>
        /// کارمند اعزام پیک
        /// </summary>
        public virtual CourierEmployee CourierEmployee { get; set; }

        /// <summary>
        /// امتیاز این پیک
        /// </summary>
        public virtual long Bonus { get; set; }

        public virtual CourierStatuses CourierStatuse { get; set; }

        protected override void Validate()
        {
            if(Sale==null)
                base.AddBrokenRule(CourierBusinessRules.SaleRequired);
            if(DeliverDate==null || string.IsNullOrEmpty(DeliverDate))
                base.AddBrokenRule(CourierBusinessRules.DeliverDateRequired);
            if (DeliverTime == null || string.IsNullOrEmpty(DeliverTime))
                base.AddBrokenRule(CourierBusinessRules.DeliverTimeRequired);
        }
    }
}
