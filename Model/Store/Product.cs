using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Store.Validations;

namespace Model.Store
{
    public class Product : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// نام دسته بندی کالا
        /// </summary>
        public virtual ProductCategory ProductCategory { get; set; }
        /// <summary>
        /// نام طبقه کالا
        /// </summary>
        public virtual string ProductName { get; set; }
        /// <summary>
        /// کد کالا
        /// </summary>
        public virtual Int32 ProductCode { get; set; }
        /// <summary>
        /// تعداد موجودی انبار اصلی بدون در نظر گرفتن موجودی انبارهای فرعی
        /// </summary>
        public virtual int UnitsInStock { get; set; }
        /// <summary>
        /// در صورتی که طبقه کالا در دیتابیس به کار رفته باشد به جای پاک کردن، آن را غیر فعال می کنیم
        ///برای غیرفعال کردن مقدار این فیلد را 1 قرار می دهیم
        /// </summary>
        public virtual bool Discontinued { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }

        /// <summary>
        /// مرتب سازی
        /// </summary>
        public virtual int SortOrder { get; set; }

        /// <summary>
        /// فروش کالا
        /// </summary>
        public virtual IEnumerable<Sales.ProductSaleDetail> ProductSaleDetails { get; protected set; }
        /// <summary>
        /// لاگ کالاها
        /// </summary>
        public virtual IEnumerable<ProductLog> ProductLogs { get; protected set; }
        /// <summary>
        /// قیمتها
        /// </summary>
        public virtual IEnumerable<ProductPrice> ProductPrices { get; protected set; }
        /// <summary>
        /// انبار کالاها
        /// </summary>
        public virtual IEnumerable<StoreProduct> StoreProducts { get; protected set; }
        /// <summary>
        /// انبارهای فرعی ای که شامل این کالا هستند
        /// </summary>
        public virtual IEnumerable<Store> Stores
        {
            get
            {
                return StoreProducts.Select(s => s.Store);
            }
        }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            //if (this.ProductCategory == null)
            //    base.AddBrokenRule(ProductBusinessRules.ProductCategoryRequired);
            if (this.ProductName == null)
                base.AddBrokenRule(ProductBusinessRules.ProductNameRequired);
        }
    }
}
