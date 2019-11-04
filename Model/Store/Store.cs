using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Store.Validations;
using Model.Employees;

namespace Model.Store
{
    public class Store : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// نام انبار
        /// </summary>
        public virtual string StoreName { get; set; }
        /// <summary>
        /// کارمند - مشخص می کند که انبار متعلق به کدام کارشناس است
        /// </summary>
        public virtual Employee OwnerEmployee { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }

        /// <summary>
        /// انبار کالاها
        /// </summary>
        public virtual IEnumerable<StoreProduct> StoreProducts { get; protected set; }
        /// <summary>
        /// لاگ های ورود و خروج
        /// </summary>
        //public virtual IEnumerable<ProductLog> ProductLogs { get; set; }
        /// <summary>
        /// کالاهای موجود در انبار
        /// </summary>
        public virtual IEnumerable<Product> Products
        {
            get
            {
                return StoreProducts.Select(s => s.Product);
            }
        }
        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.OwnerEmployee == null)
                base.AddBrokenRule(StoreBusinessRules.OwnerEmployeeRequired);
        }  
    }
}
