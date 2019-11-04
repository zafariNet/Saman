using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Store.Validations;

namespace Model.Store
{
    public class ProductLog : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// طبقه کالایی که قرار است به انبار وارد شود یا از آن خارج شود
        /// </summary>
        public virtual Product Product { get; set; }
        /// <summary>
        /// تاریخ ورود به انبار یا خروج از انبار
        /// </summary>
        public virtual string LogDate { get; set; }
        /// <summary>
        /// تعداد ورود یا خروج
        /// </summary>
        public virtual int UnitsIO { get; set; }
        /// <summary>
        /// قیمت خرید کالا
        /// </summary>
        public virtual Int64 PurchaseUnitPrice { get; set; }
        /// <summary>
        /// جمع سطر
        ///قیمت خرید ضربدر تعداد
        /// </summary>
        public virtual Int64 TotalLine { get; protected set; }
        /// <summary>
        /// تاریخ خرید
        /// </summary>
        public virtual string PurchaseDate { get; set; }
        /// <summary>
        /// نام فروشنده
        /// </summary>
        public virtual string SellerName { get; set; }
        /// <summary>
        /// شماره فاکتور خرید کالا
        /// </summary>
        public virtual string PurchaseBillNumber { get; set; }
        /// <summary>
        /// تأیید و بستن
        /// </summary>
        public virtual bool Closed { get; set; }
        /// <summary>
        /// شماره سریال ورود به انبار
        ///این شماره سریال پس از تأیید از 0001 شروع می شود و بصورت اتوماتیک ثبت می شود
        /// </summary>
        public virtual string InputSerialNumber { get; set; }
        /// <summary>
        /// شماره سریال کالا از
        /// </summary>
        public virtual string ProductSerialFrom { get; set; }
        /// <summary>
        /// شماره سریال کالا تا
        /// </summary>
        public virtual string ProductSerialTo { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }
        /// <summary>
        /// انبار مجازی
        /// </summary>
        public virtual Store Store { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.LogDate == null)
                base.AddBrokenRule(ProductLogBusinessRules.LogDateRequired);
            if (this.Product == null)
                base.AddBrokenRule(ProductLogBusinessRules.ProductRequired);
        }
    }
}
