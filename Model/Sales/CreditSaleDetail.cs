#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;
using Infrastructure.Domain;
using Model.Store;
using Model.Sales.Validations;
using Model.Employees;

#endregion

namespace Model.Sales
{
    /// <summary>
    /// جزییات فروش اعتباری
    /// </summary>
    public class CreditSaleDetail : UncreditSaleDetailBase, IAggregateRoot
    {
        private IList<BusinessRule> _bussinessRules = new List<BusinessRule>();
        /// <summary>
        /// خدمات اعتباری (شارژ)‏
        /// </summary>
        public virtual CreditService CreditService { get; set; }
        /// <summary>
        /// آیتمهای اعتباری برگشت شده مربوط به این آیتم
        /// </summary>
        public virtual CreditSaleDetail RollbackedCreditSaleDetail { get; set; }

        /// <summary>
        /// اگر این آیتم برگشت از فروش است فروش اصلی ذخیره میشود
        /// </summary>
        public virtual CreditSaleDetail MainSaleDetail { get; set; }


        /// <summary>
        /// مبلغ برگشتی به شبکه
        /// </summary>
        public virtual long RollbackNetworkPrice { get; set; }
        /// <summary>
        /// قیمت خرید خدمت اعتباری
        /// </summary>
        public virtual long PurchaseUnitPrice { get; set; }

        /// <summary>
        /// امیاز و پورسانت این فروش
        /// </summary>
        public virtual BonusComission BonusComission { get; set; }

        #region Deliver

        public virtual void Deliver(Employee deliverEmployee, string deliverNote)
        {
            // چک شود که فاکتور برگشت از فروش نباشد
            if (Sale.IsRollbackSale)
            {
                _bussinessRules.Add(SaleBusinessRules.ThisIsRollbackCannotDeliver);
                return;
            }

            // چک شود که آیتم قبلا تحویل نشده باشد
            if (DeliverEmployee != null)
            {
                _bussinessRules.Add(SaleBusinessRules.AlreadyDeliverd);
                return;
            }

            // چک شود که فاکتور تأیید شده باشد
            if (!Sale.Closed)
            {
                _bussinessRules.Add(SaleBusinessRules.SaleIsNotClosedCantDeliver);
                return;
            }

            // چک شود که این آیتم برگشت نخورده باشد
            if (Rollbacked)
            {
                _bussinessRules.Add(SaleBusinessRules.RollbackedAndCantDeliver);
                return;
            }

            // بروز کردن موجودی شبکه
            // اگر موجودی کافی بود
            if (CreditService.Network.Balance >= PurchaseUnitPrice * Units ||
                // یا اجازه منفی شدن موجودی در مورد شبکه مورد نظر وجود داشت
                (CreditService.Network.DeliverWhenCreditLow))
            {
                // تحویل
                SetDeliver(deliverEmployee, deliverNote);
                // موجودی شبکه به اندازه مبلغ خرید کاهش می یابد
                CreditService.Network.Balance -= PurchaseUnitPrice * Units;

                //============================================
                //                                            
                // محل ارتباط با شبکه و کم کردن موجودی شبکه
                //                                            
                //============================================
            }
            else
            {
                _bussinessRules.Add(SaleBusinessRules.NetworkBalanceNotEnough);
                return;
            }

        }

        #endregion

        #region Validaton
        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.CreditService == null)
                base.AddBrokenRule(CreditSaleDetailBusinessRules.CreditServiceRequired);
            if(MainSaleDetail!=null)
                if(MainSaleDetail.LineTotal<this.LineTotal)
                    base.AddBrokenRule(SaleBusinessRules.LineTotalIsGrater);

            foreach (BusinessRule businessRule in _bussinessRules)
            {
                base.AddBrokenRule(businessRule);
            }
        }

        #endregion

        #region Operator Methods
        /// <summary>
        /// برابر بودن دو موجودیت
        /// </summary>
        /// <param name="entity">موجودیت</param>
        /// <returns></returns>
        public override bool Equals(object entity)
        {
            return entity != null
                && entity is CreditSaleDetail
                && this == (CreditSaleDetail)entity;
        }

        public override int GetHashCode()
        {
            return this.CreditService.ID.GetHashCode();
        }

        /// <summary>
        /// عملگر تساوی منطقی دو موجودیت
        /// </summary>
        /// <param name="entity1">موجودیت اول</param>
        /// <param name="entity2">موجودیت دوم</param>
        /// <returns>True/False</returns>
        public static bool operator ==(CreditSaleDetail entity1, CreditSaleDetail entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;

            if ((object)entity1 == null || (object)entity2 == null)
                return false;

            if (entity1.CreditService.ID.ToString() == entity2.CreditService.ID.ToString())
                return true;

            return false;
        }

        /// <summary>
        /// عملگر نامساوی منطقی دو موجودیت
        /// </summary>
        /// <param name="entity1">موجودیت اول</param>
        /// <param name="entity2">موجودیت دوم</param>
        /// <returns>True/False</returns>
        public static bool operator !=(CreditSaleDetail entity1, CreditSaleDetail entity2)
        {
            return !(entity1 == entity2);
        }
        #endregion
    }
}
