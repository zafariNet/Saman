using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Store.Validations;

namespace Model.Store
{
    public class ProductCategory : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// نام طبقه کالا
        /// </summary>
        public virtual string ProductCategoryName { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }
        /// <summary>
        /// در صورتی که طبقه کالا در دیتابیس به کار رفته باشد به جای پاک کردن، آن را غیر فعال می کنیم
        ///برای غیرفعال کردن مقدار این فیلد را 1 قرار می دهیم
        /// </summary>
        public virtual bool Discontinued { get; set; }

        /// <summary>
        /// کالاها
        /// </summary>
        public virtual IEnumerable<Product> Products { get; protected set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.ProductCategoryName == null)
                base.AddBrokenRule(ProductCategoryBusinessRules.ProductCategoryNameRequired); 
        }
    }
}
