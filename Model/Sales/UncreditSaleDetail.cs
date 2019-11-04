#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Sales.Validations;
using Model.Store;
using Model.Employees;
#endregion

namespace Model.Sales
{
    public class UncreditSaleDetail : UncreditSaleDetailBase, IAggregateRoot
    {
        private IList<BusinessRule> _bussinessRules = new List<BusinessRule>();

        /// <summary>
        /// خدمت غیراعتباری
        /// </summary>
        public virtual UncreditService UncreditService { get; set; }
        /// <summary>
        /// آیتمهای برگشت شده مربوط به این آیتم
        /// </summary>
        public virtual UncreditSaleDetail RollbackedUncreditSaleDetail { get; set; }

        public virtual UncreditSaleDetail MainSaleDetail { get; set; }

        /// <summary>
        /// امتیاز و پورسانت این فروش
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

            // تحویل
            SetDeliver(deliverEmployee, deliverNote);
        }

        #endregion

        #region Validation

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.UncreditService == null)
                base.AddBrokenRule(UncreditSaleDetailBusinessRules.UncreditServiceRequired);
            if (MainSaleDetail != null)
                if (MainSaleDetail.LineTotal < this.LineTotal)
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
                && entity is UncreditSaleDetail
                && this == (UncreditSaleDetail)entity;
        }

        public override int GetHashCode()
        {
            return this.UncreditService.ID.GetHashCode();
        }

        /// <summary>
        /// عملگر تساوی منطقی دو موجودیت
        /// </summary>
        /// <param name="entity1">موجودیت اول</param>
        /// <param name="entity2">موجودیت دوم</param>
        /// <returns>True/False</returns>
        public static bool operator ==(UncreditSaleDetail entity1, UncreditSaleDetail entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;

            if ((object)entity1 == null || (object)entity2 == null)
                return false;

            if (entity1.UncreditService.ID.ToString() == entity2.UncreditService.ID.ToString())
                return true;

            return false;
        }

        /// <summary>
        /// عملگر نامساوی منطقی دو موجودیت
        /// </summary>
        /// <param name="entity1">موجودیت اول</param>
        /// <param name="entity2">موجودیت دوم</param>
        /// <returns>True/False</returns>
        public static bool operator !=(UncreditSaleDetail entity1, UncreditSaleDetail entity2)
        {
            return !(entity1 == entity2);
        }
        #endregion
    }
}
