#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Store;
using Model.Sales.Validations;
using Model.Employees;
using Infrastructure.Persian;
#endregion

namespace Model.Sales
{
    public class BaseSaleDetail : EntityBase
    {
        /// <summary>
        /// صورتحساب
        /// </summary>
        public virtual Sale Sale { get; set; }

        #region ctor

        public BaseSaleDetail()
        {
            DeliverDate = null;
        }

        #endregion

        #region تعداد، مالیات و تخفیف
        /// <summary>
        /// قیمت واحد
        /// این قیمت از جدول خدمات اعتباری به اینجا کپی میشود که اگر در آینده قیمت خدمات تغییر کرد با مشکل مواجه نشویم
        /// </summary>
        public virtual long UnitPrice { get; set; }
        /// <summary>
        /// تعداد
        /// </summary>
        public virtual int Units { get; set; }
        /// <summary>
        /// تخفیف هر واحد
        /// </summary>
        public virtual long Discount { get; set; }
        /// <summary>
        /// تخفیف سطر
        ///حاصل ضرب تخفیف در تعداد می باشد
        /// </summary>
        public virtual long LineDiscount
        {
            get
            {
                return Discount * Units;
            }
        }
        /// <summary>
        /// مالیات و عوارض هر واحد
        /// </summary>
        public virtual long Imposition { get; set; }
        /// <summary>
        /// مالیات و عوارض سطر
        /// حاصل ضرب مالیات در تعداد می باشد
        /// </summary>
        public virtual long LineImposition
        {
            get
            {
                return Imposition * Units;
            }
        }

        /// <summary>
        /// جمع سطر
        ///حاصل جمع قیمت به اضافه مالیات منهای تخفیف می باشد
        /// </summary>
        public virtual long LineTotal
        {
            get
            {
                if (Sale.IsRollbackSale)
                    return RollbackPrice;
                else
                    return Units * UnitPrice - LineDiscount + LineImposition;
            }
            
        }

        public virtual long LineTotalWithoutDiscountAndImposition
        {
            get
            {
                return Units * UnitPrice;
            }
        }
        #endregion

        #region برگشت از فروش

        /// <summary>
        /// توضیحات برگشت از فروش - در مورد فاکتورهای برگشتی کاربرد دارد
        /// </summary>
        public virtual string RollbackNote { get; set; }
        /// <summary>
        /// مبلغ برگشتی
        /// </summary>
        public virtual long RollbackPrice { get; set; }

        #endregion

        #region تحویل
        /// <summary>
        /// توضیحات تحویل
        /// </summary>
        public virtual string DeliverNote { get; protected set; }
        /// <summary>
        /// کارمند تحویل
        /// </summary>
        public virtual Employee DeliverEmployee { get; protected set; }
        /// <summary>
        /// تاریخ تحویل
        /// </summary>
        public virtual string DeliverDate { get; protected set; }
        /// <summary>
        /// آیا تحویل شده؟
        /// </summary>
        public virtual bool Delivered
        {
            get
            {
                return DeliverDate != null;
            }
        }
        /// <summary>
        /// وضعیت تحویل یا برگشت
        /// </summary>
        public virtual SaleDetailStatus Status
        {
            get
            {
                if (Delivered && Rollbacked) return SaleDetailStatus.DeliveredAndRollbacked;
                if (Delivered) return SaleDetailStatus.Delivered;
                if (Rollbacked) return SaleDetailStatus.Rollbacked;
                return SaleDetailStatus.Nothing;
            }
        }

        // آیا این آیتم در فاکتور دیگر برگشت شده؟
        public virtual bool Rollbacked
        {
            get
            {
                //if (Sale.RollbackedSales != null && Sale.RollbackedSales.Count() > 0)
                //    foreach (Sale childSale in Sale.RollbackedSales)
                //        if (//(childSale.CreditSaleDetails != null && childSale.CreditSaleDetails.Contains(this)) ||
                //            //(childSale.UncreditSaleDetails != null && childSale.UncreditSaleDetails.Contains(this)) ||
                //            (childSale.ProductSaleDetails != null && childSale.ProductSaleDetails.Contains(this)))
                //            return true;

                return false;
            }
        }

        // آیا این آیتم مربوط به فاکتور برگشت از فروش است؟
        public virtual bool IsRollbackDetail
        {
            get
            {
                return Sale.IsRollbackSale ;
            }
        }
        /// <summary>
        /// آیا قابل تحویل است؟
        /// </summary>
        public virtual bool CanDeliver
        {
            get
            {
                return !Sale.IsRollbackSale && !Delivered && Sale.Closed && !Rollbacked;
            }
        }

        public virtual bool CanRollback
        {
            get
            {
                return !Sale.IsRollbackSale && !Rollbacked && Sale.Closed;
            }
        }

        /// <summary>
        /// تحویل
        /// </summary>
        /// <param name="deliverEmployee">کارمند تحویل</param>
        /// <param name="deliverNote">توضیحات تحویل</param>
        protected virtual void SetDeliver(Employee deliverEmployee, string deliverNote)
        {
            DeliverEmployee = deliverEmployee;
            DeliverNote = deliverNote;
            DeliverDate = PersianDateTime.Now;
        }

        
        #endregion

        #region Validation

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Sale == null)
                base.AddBrokenRule(ProductSaleDetailBusinessRules.SaleRequired);
        }

        #endregion
    }
}
