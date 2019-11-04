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
    /// موجودیت احتمال خرید
    /// </summary>
    public class BuyPossibility : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// احتمال خرید 
        /// </summary>
        public virtual string BuyPossibilityName { get; set; }
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public virtual int? SortOrder { get; set; }

        ///// <summary>
        ///// مشتریان
        ///// </summary>
        //public virtual IEnumerable<Customer> Customers { get; protected set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.BuyPossibilityName == null)
                base.AddBrokenRule(BuyPossibilityBusinessRules.BuyPossibilityNameRequired);
        }
    }
}
