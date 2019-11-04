using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;
using Infrastructure.Domain;
using Model.Store.Validations;
using Model.Sales;

namespace Model.Store
{
    public class CreditService : EntityBase, IAggregateRoot
    {
        #region Properties
        /// <summary>
        /// شبکه ای که این خدمات اعتباری به آن تعلق دارد
        /// </summary>
        public virtual Network Network { get; set; }
        /// <summary>
        /// شرح (مثلا شارژ یک ماهه 2 گیگ)
        /// </summary>
        public virtual string ServiceName { get; set; }
        /// <summary>
        /// کد خدمات اعتباری که از 3000 شروع می شود
        /// </summary>
        public virtual int CreditServiceCode { get; set; }
        /// <summary>
        /// قیمت واحد
        /// </summary>
        public virtual long UnitPrice { get; set; }
        /// <summary>
        /// قیمت خرید-
        ///هنگامی که عملیات تحویل انجام می شود، مبلغی معادل این قیمت از اعتبار شبکه کسر میگردد
        /// </summary>
        public virtual long PurchaseUnitPrice { get; set; }
        /// <summary>
        /// قیمت واحد برای توزیع کنندگان و نمایندگی ها
        /// </summary>
        public virtual long ResellerUnitPrice { get; set; }
        /// <summary>
        /// ماکزیمم مقدار تخفیفی که هنگام فروش توسط کاربر می تواند به ثبت برسد
        /// </summary>
        public virtual long MaxDiscount { get; set; }
        /// <summary>
        /// مالیات و عوارض
        /// </summary>
        public virtual int Imposition { get; set; }
        /// <summary>
        /// در صورتی که این رکورد در دیتابیس استفاده شده باشد و صورتحسابهایی از آن استفاده کرده باشند نمیتوان آن را حذف کرد
        ///بنابراین برای حذف باید آن را غیر فعال کرد تا در لیست قابل فروشها نمایش داده نشوند
        ///برای غیر فعال کردن مقدار این فیلد را برابر 1 قرار می دهیم
        /// </summary>
        public virtual bool Discontinued { get; set; }
        /// <summary>
        /// تعداد روزهایی که پس از آن این آیتم منقضی می شود
        ///مثلا در مورد شارژ یک ماهه باید مقدار 30 در اینجا وارد شود
        /// </summary>
        public virtual int ExpDays { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }
        /// <summary>
        /// عددی صحیح که جهت مشخص کردن ترتیب نمایش به کار می رود و خروجی طبق این عدد سورت می شود
        /// </summary>
        public virtual Int32 SortOrder { get; set; }

        /// <summary>
        /// پورسانت
        /// </summary>
        public virtual long Comission { get; set; }

        /// <summary>
        /// امتیاز
        /// </summary>
        public virtual long Bonus { get; set; }

        #endregion

        #region IEnumeables
        /// <summary>
        /// فروش اعتباری
        /// </summary>
        public virtual IEnumerable<CreditSaleDetail> CreditSaleDetails { get; protected set; }
        #endregion

        #region Validation
        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Network == null)
                base.AddBrokenRule(CreditServiceBusinessRules.NetworkRequired);
            if (this.ServiceName == null)
                base.AddBrokenRule(CreditServiceBusinessRules.ServiceNameRequired);
        }
        #endregion
    }
}
