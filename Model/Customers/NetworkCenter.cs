using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;
using Model.Store;
using Model.Employees;
using Model.Base.Validations;

namespace Model.Customers
{
    public class NetworkCenter : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// شبکه
        /// </summary>
        public virtual Network Network { get; set; }
        /// <summary>
        /// مرکز مخابراتی
        /// </summary>
        public virtual Center Center { get; set; }
        /// <summary>
        /// وضعیت که چهار حالت ممکن است اتفاق بیفتد:‏
        ///-1: مشخص نشده
        ///1: پوشش
        ///2: عدم پوشش
        ///3: عدم امکان موقت
        /// </summary>
        public virtual NetworkCenterStatus Status { get; set; }

        /// <summary>
        /// اجازه فروش
        /// </summary>
        public virtual bool CanSale { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Network == null)
                base.AddBrokenRule(NetworkCenterBusinessRules.NetworkRequired);

            if (this.Center == null)
                base.AddBrokenRule(NetworkCenterBusinessRules.CenterRequired);
        }
    }
}
