using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Store.Validations;

namespace Model.Store
{
    public class StoreProduct : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// کالا
        /// </summary>
        public virtual Product Product { get; set; }
        /// <summary>
        /// انبار فرعی
        /// </summary>
        public virtual Store Store { get; set; }
        /// <summary>
        /// موجودی کالا در این انبار
        /// </summary>
        public virtual int UnitsInStock { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Store == null)
                base.AddBrokenRule(StoreProductBusinessRules.StoreRequired);
            if (this.Product == null)
                base.AddBrokenRule(StoreProductBusinessRules.ProductRequired);
        }  
    }
}
