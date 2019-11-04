using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Store.Validations;

namespace Model.Store
{
    public class ProductPrice : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// طبقه کالا
        /// </summary>
        public virtual Product Product { get; set; }
        /// <summary>
        /// شرح کالا
        /// </summary>
        public virtual string ProductPriceTitle { get; set; }
        /// <summary>
        /// کد محصول
        /// </summary>
        public virtual int ProductPriceCode { get; set; }
        /// <summary>
        /// قیمت واحد
        /// </summary>
        public virtual long UnitPrice { get; set; }
        /// <summary>
        /// ماکزیمم تخفیفی که کارمند هنگام فروش میتواند ثبت کند
        /// </summary>
        public virtual long MaxDiscount { get; set; }
        /// <summary>
        /// مالیات و عوارض
        /// </summary>
        public virtual long Imposition { get; set; }
        /// <summary>
        /// در صورتی که کالا در دیتابیس به کار رفته باشد به جای پاک کردن، آن را غیر فعال می کنیم
        ///برای غیرفعال کردن مقدار این فیلد را 1 قرار می دهیم 
        /// </summary>
        public virtual bool Discontinued { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }
        /// <summary>
        /// عددی صحیح که جهت مشخص کردن ترتیب نمایش به کار می رود و خروجی طبق این عدد سورت می شود
        /// </summary>
        public virtual int? SortOrder { get; set; }

        /// <summary>
        /// پورسانت
        /// </summary>
        public virtual long Comission { get; set; }

        /// <summary>
        /// امتیاز فروش
        /// </summary>
        public virtual long Bonus { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.ProductPriceTitle == null)
                base.AddBrokenRule(ProductPriceBusinessRules.ProductPriceTitleRequired);
            if (this.Product == null)
                base.AddBrokenRule(ProductPriceBusinessRules.ProductRequired);
            if (this.ProductPriceCode == null)
                base.AddBrokenRule(ProductPriceBusinessRules.ProductPriceCodeRequired);
        }  
    }
}
