using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Store.Validations;
using Model.Sales;

namespace Model.Store
{
    public class UncreditService : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// شرح خدمات غیر اعتباری
        /// </summary>
        public virtual string UncreditServiceName { get; set; }
        /// <summary>
        /// کد خدمات غیر اعتباری
        /// </summary>
        public virtual int? UnCreditServiceCode { get; set; }
        /// <summary>
        /// قیمت واحد
        /// </summary>
        public virtual int UnitPrice { get; set; }
        /// <summary>
        /// ماکزیمم تخفیفی که کارمند هنگام فروش می تواند ثبت کند
        /// </summary>
        public virtual long MaxDiscount { get; set; }
        /// <summary>
        /// مالیات و عوارض
        /// </summary>
        public virtual Int64 Imposition { get; set; }
        /// <summary>
        /// برای پاک کردن رکورد در صورتی که در دیتابیس استفاده شده باشد از این فیلد استفاده می شود
        ///اگر مقدار این فیلد 1 باشد این آیتم غیر فعال می باشد
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
        /// امتیاز
        /// </summary>
        public virtual long Bonus { get; set; }

        /// <summary>
        /// پورسانت
        /// </summary>
        public virtual long Comission { get; set; }

        /// <summary>
        /// فروشهای غیر اعتباری
        /// </summary>
        public virtual IEnumerable<UncreditSaleDetail> UncreditSaleDetails { get; protected set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.UncreditServiceName == null)
                base.AddBrokenRule(UncreditServiceBusinessRules.UncreditServiceNameRequired);
        }  
    }
}
