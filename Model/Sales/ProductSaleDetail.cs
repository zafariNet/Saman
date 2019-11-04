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
#endregion

namespace Model.Sales
{
    public class ProductSaleDetail : UncreditSaleDetailBase, IAggregateRoot
    {
        public ProductSaleDetail()
        {
            
        }

        private IList<BusinessRule> _bussinessRules = new List<BusinessRule>();
        /// <summary>
        /// کالا
        /// </summary>
        public virtual ProductPrice ProductPrice { get; set; }

        /// <summary>
        /// آیتمهای برگشت شده مربوط به این آیتم
        /// </summary>
        public virtual ProductSaleDetail RollbackedProductSaleDetail { get; set; }

        /// <summary>
        /// اگر آیتم برگش از فروش بود فروش اصلی ذخیره میشود
        /// </summary>
        public virtual ProductSaleDetail MainSaleDetail { get; set; }

        /// <summary>
        /// امتیاز و پورسانت
        /// </summary>
        public virtual BonusComission BonusComission { get; set; }

        #region Deliver

        /// <summary>
        /// انبارهای فرعی ای که این کالا در آن موجود است
        /// </summary>
        private IEnumerable<Store.Store> StoresThatContainThisProduct
        {
            get
            {
                return ProductPrice.Product.Stores;
            }
        }

        // انباری که کالا را تحویل داده
        public virtual Store.Store DeliverStore { get; protected set; }

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
            
            // چک شود حساب مشتری به اندازه کافی بستانکار باشد
            // ??

            // پیدا کردن انبارهای فرعی کارمند تحویل
            IEnumerable<Store.Store> stores = deliverEmployee.Stores;

            bool atLeastOneStoreContainsThisProduct = false;

            // مشخص کردن اولین انباری که کالای مورد نظر در آن وجود دارد
            foreach (Store.Store store in stores)
            {
                // اگر انبار حاوی کالای مورد نظر باشد
                if (store.Products.Contains(ProductPrice.Product))
                {
                    atLeastOneStoreContainsThisProduct = true;

                    // اگر تعداد کالا در انبار کافی باشد
                    if (store.StoreProducts.First(w => w.Product == ProductPrice.Product).UnitsInStock >= Units)
                    {
                        // تحویل
                        SetDeliver(deliverEmployee, deliverNote);

                        // ذخیره کردن انباری که کالا را تحویل داده در دیتابیس
                        DeliverStore = store;

                        // کسر از انبار
                        store.StoreProducts.First(w => w.Product == ProductPrice.Product).UnitsInStock -= Units;
                        return;
                    }
                }
            }

            // اگر کامپایلر به اینجا رسید یعنی انبارها حاوی کالای مورد نظر نبوده یا موجودی کافی نبوده و تحویل انجام نشده
            if (!atLeastOneStoreContainsThisProduct)
            {
                //  هیچ یک از انبارها حاوی کالای مورد نظر نبوده
                _bussinessRules.Add(SaleBusinessRules.StoresNotContainThisProduct);
            }
            else
            {
                //  حداقل یکی از انبارها حاوی کالای مورد نظر بوده ولی موجودی کافی نیست
                _bussinessRules.Add(SaleBusinessRules.UnitsInStockNotEnough);
            }
        }

        #endregion

        #region Validation
        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.ProductPrice == null)
                base.AddBrokenRule(ProductSaleDetailBusinessRules.ProductRequired);
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
                && entity is ProductSaleDetail
                && this == (ProductSaleDetail)entity;
        }

        public override int GetHashCode()
        {
            return this.ProductPrice.ID.GetHashCode();
        }

        /// <summary>
        /// عملگر تساوی منطقی دو موجودیت
        /// </summary>
        /// <param name="entity1">موجودیت اول</param>
        /// <param name="entity2">موجودیت دوم</param>
        /// <returns>True/False</returns>
        public static bool operator ==(ProductSaleDetail entity1, ProductSaleDetail entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;

            if ((object)entity1 == null || (object)entity2 == null)
                return false;

            if (entity1.ProductPrice.ID.ToString() == entity2.ProductPrice.ID.ToString())
                return true;

            return false;
        }

        /// <summary>
        /// عملگر نامساوی منطقی دو موجودیت
        /// </summary>
        /// <param name="entity1">موجودیت اول</param>
        /// <param name="entity2">موجودیت دوم</param>
        /// <returns>True/False</returns>
        public static bool operator !=(ProductSaleDetail entity1, ProductSaleDetail entity2)
        {
            return !(entity1 == entity2);
        }
        #endregion

    }
}
