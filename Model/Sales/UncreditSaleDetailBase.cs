using Infrastructure.Persian;
using Model.Base;
using Model.Employees;
using Model.Sales.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Sales
{
    public class UncreditSaleDetailBase :EntityBase
    {
         /// <summary>
        /// صورتحساب
        /// </summary>
        public virtual Sale Sale { get; set; }

        #region ctor

        public UncreditSaleDetailBase()
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
        public virtual long LineDiscount { get; set; }
        //{
        //    get
        //    {
        //        return Discount * Units;
        //    }
        //}
        /// <summary>
        /// مالیات و عوارض هر واحد
        /// </summary>
        public virtual long Imposition { get; set; }
        /// <summary>
        /// مالیات و عوارض سطر
        /// حاصل ضرب مالیات در تعداد می باشد
        /// </summary>
        public virtual long LineImposition { get; set; }
        //{
        //    get
        //    {
        //        return Imposition * Units;
        //    }
        //}

        /// <summary>
        /// جمع سطر
        ///حاصل جمع قیمت به اضافه مالیات منهای تخفیف می باشد
        /// </summary>
        public virtual long LineTotal { get; set; }
        //{
        //    get
        //    {
        //        if (Sale.IsRollbackSale)
        //            return RollbackPrice;
        //        else
        //            return Units * UnitPrice - LineDiscount + LineImposition;
        //    }
            
        //}

        public virtual long LineTotalWithoutDiscountAndImposition { get; set; }
        //{
        //    get
        //    {
        //        return Units * UnitPrice;
        //    }
        //}
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

        public virtual Employee SaleEmployee { get; set; }
        /// <summary>
        /// تاریخ تحویل
        /// </summary>
        public virtual string DeliverDate { get; protected set; }
        /// <summary>
        /// آیا تحویل شده؟
        /// </summary>
        public virtual bool Delivered { get; set; }
        //{
        //    get
        //    {
        //        return DeliverDate != null;
        //    }
        //}
        /// <summary>
        /// وضعیت تحویل یا برگشت
        /// </summary>
        public virtual SaleDetailStatus Status { get; set; }
        //{
        //    get
        //    {
        //        if (Delivered && Rollbacked) return SaleDetailStatus.DeliveredAndRollbacked;
        //        if (Delivered) return SaleDetailStatus.Delivered;
        //        if (Rollbacked) return SaleDetailStatus.Rollbacked;
        //        return SaleDetailStatus.Nothing;
        //    }
        //}

        // آیا این آیتم در فاکتور دیگر برگشت شده؟
        public virtual bool Rollbacked { get; set; }
        //{
        //    get
        //    {
        //        if (Sale.RollbackedSales != null && Sale.RollbackedSales.Count() > 0)
        //            foreach (Sale childSale in Sale.RollbackedSales)
        //                if ((childSale.CreditSaleDetails != null && childSale.CreditSaleDetails.Contains(this)) ||
        //                    (childSale.UncreditSaleDetails != null && childSale.UncreditSaleDetails.Contains(this)) ||
        //                    (childSale.ProductSaleDetails != null && childSale.ProductSaleDetails.Contains(this)))
        //                    return true;

        //        return false;
        //    }
        //}
        
        // آیا این آیتم مربوط به فاکتور برگشت از فروش است؟
        public virtual bool IsRollbackDetail { get; set; }
        //{
        //    get
        //    {
        //        return Sale.IsRollbackSale ;
        //    }
        //}
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
            Delivered = true;
            Status = SaleDetailStatus.Delivered;
        }

        

        public virtual long Bonus { get; set; }
        public virtual long UnDeliveredComission { get; set; }
        public virtual long Comission { get; set; }
        public virtual string BonusDate { get; set; }
        public virtual string ComissionDate { get; set; }

        #endregion

        #region Validation

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Sale == null)
                base.AddBrokenRule(ProductSaleDetailBusinessRules.SaleRequired);
            if(this.LineTotalWithoutDiscountAndImposition!= Units*UnitPrice)
                base.AddBrokenRule(SaleBusinessRules.LineTotalwithoutDscountAndImpositionIsLess);
            if(LineImposition!=Units*LineImposition)
                base.AddBrokenRule(SaleBusinessRules.LineImpositionError);
            if(LineDiscount!=Units*Imposition)
                base.AddBrokenRule(SaleBusinessRules.LineDiscountnError);
            

            
                
        }

        #endregion
    }
}
