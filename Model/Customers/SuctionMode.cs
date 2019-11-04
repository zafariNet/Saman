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
    /// موجودیت شیوه جذب
    /// </summary>
    public class SuctionMode : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// شیوه جذب مشتری
        /// </summary>
        public virtual string SuctionModeName { get; set; }
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public virtual int? SortOrder { get; set; }

        /// <summary>
        /// فعال
        /// </summary>
        public virtual bool Discontinued { get; set; }
        ///// <summary>
        ///// مشتریان
        ///// </summary>
        //public virtual IEnumerable<Customer> Customers { get; protected set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.SuctionModeName == null)
                base.AddBrokenRule(SuctionModeBusinessRules.SuctionModeNameRequired);
        }
    }
}
