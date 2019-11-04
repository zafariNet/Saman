using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;

namespace Model.Customers
{
    /// <summary>
    /// مرحله مشتری
    /// </summary>
    public class CustomerLevel : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// وضعیت مشتری
        /// </summary>
        public virtual Level Level { get; set; }
        /// <summary>
        /// مشتری
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }

        public virtual int WaitingDays { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Level == null)
                base.AddBrokenRule(CustomerLevelBusinessRules.LevelRequired);
            if (this.Customer == null)
                base.AddBrokenRule(CustomerLevelBusinessRules.CustomerRequired);
        }  
    }
}
