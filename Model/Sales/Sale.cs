#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;
using Infrastructure.Domain;
using Model.Customers;
using Model.Employees;
using Model.Sales.Validations;
using Infrastructure.Persian;
#endregion

namespace Model.Sales
{
    public class Sale : EntityBase, IAggregateRoot
    {
        private IList<BusinessRule> _bussinessRules = new List<BusinessRule>();

        #region Ctor

        public Sale()
        {
            ProductSaleDetails = new List<ProductSaleDetail>();
            CreditSaleDetails = new List<CreditSaleDetail>();
            UncreditSaleDetails = new List<UncreditSaleDetail>();
        }

        #endregion

          
        /// <summary>
        /// مشتری
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// فروشی که قرار است برگشت بخورد
        ///توضیح اینکه از این فیلد جهت برگشت زدن استفاده می شود. در صورتی که این فیلد پر شده باشد یعنی این فاکتور برگشت از فروش است. مقدار این فیلد برابر فروش رکوردی دیگر از همین موجودیت است
        /// </summary>
        public virtual Sale MainSale { get; set; }
        /// <summary>
        /// همه برگشت از فروش های مربوط به این فروش
        /// </summary>
        public virtual IList<Sale> RollbackedSales { get; protected set; }
        /// <summary>
        /// شماره فاکتور
        /// </summary>
        public virtual string SaleNumber { get; set; }

        /// <summary>
        /// پیک های مربوط به این فروش
        /// </summary>
        public virtual Courier Couriers { get; set; }

        public virtual bool HasCourier
        {
            get;
            set;
        }

        public virtual bool CanDeliver
        {
            get
            {
                return ProductSaleDetails.Where(w => w.CanDeliver).Count() > 0 ||
                    CreditSaleDetails.Where(w => w.CanDeliver).Count() > 0 ||
                    UncreditSaleDetails.Where(w => w.CanDeliver).Count() > 0;
            }
        }

        public virtual bool CanRollback
        {
            get
            {
                return ProductSaleDetails.Where(w => w.CanRollback).Count() > 0 || 
                    CreditSaleDetails.Where(w => w.CanRollback).Count() > 0 || 
                    UncreditSaleDetails.Where(w => w.CanRollback).Count() > 0;
            }
        }
        #region تأیید فروش

        /// <summary>
        /// تأیید شده و بسته شده
        ///توضیح اینکه رکورد فروش می تواند قبل از تأیید تغییر کند و مثلا اقلام آن اضافه و کم شود
        ///ولی پس از بسته شدن هیچ گونه تغییری امکان پذیر نمی باشد
        /// </summary>
        public virtual bool Closed { get; protected set; }
        /// <summary> 
        /// کارمندی که رکورد را بسته است
        /// </summary>
        public virtual Employee CloseEmployee { get; protected set; }
        /// <summary>
        /// تاریخ بستن
        /// </summary>
        public virtual string CloseDate { get; protected set; }

        /// <summary>
        /// Close Sale
        /// </summary>
        /// <param name="closeEmployee">Employee who closed</param>
        public virtual void Close(Employee closeEmployee)
        {
            Closed = true;
            CloseEmployee = closeEmployee;
            CloseDate = PersianDateTime.Now;
            // بدهکار کردن حساب مشتری
            if(!IsRollbackSale)
            Customer.Balance -= SaleTotal;
        }

        #endregion

        #region Totals

        /// <summary>
        /// جمع فروش اعتباری
        /// </summary>
        public virtual long CreditSaleDetailsTotal
        {
            get
            {
                return CreditSaleDetails.Sum(s => s.LineTotal);
            }
        }

        public virtual long CreditSaleDetailsTotalWithoutDiscountAndImposition
        {
            get
            {
                return CreditSaleDetails.Sum(s => s.LineTotalWithoutDiscountAndImposition);
            }
        }

        /// <summary>
        /// جمع فروش غیر اعتباری
        /// </summary>
        public virtual long UncreditSaleDetailsTotal
        {
            get
            {
                return UncreditSaleDetails.Sum(s => s.LineTotal);
            }
        }

        public virtual long UncreditSaleDetailsTotalWithoutDiscountAndImposition
        {
            get
            {
                return UncreditSaleDetails.Sum(s => s.LineTotalWithoutDiscountAndImposition);
            }
        }

        /// <summary>
        /// جمع قیمت آیتمهای تحویل شده
        /// </summary>
        public virtual long SumCostOfDeliveredItems
        {
            get
            {
                return 
                      UncreditSaleDetails.Where(w => w.Delivered).Sum(s => s.LineTotal)
                    + CreditSaleDetails.Where(w => w.Delivered).Sum(s => s.LineTotal)
                    + ProductSaleDetails.Where(w => w.Delivered).Sum(s => s.LineTotal);
            }
        }
        /// <summary>
        /// جمع فروش کالا
        /// </summary>
        public virtual long ProductSaleDetailsTotal
        {
            get
            {
                return ProductSaleDetails.Sum(s => s.LineTotal);
            }
        }
        public virtual long ProductSaleDetailsTotalWithoutDiscountAndImposition
        {
            get
            {
                return ProductSaleDetails.Sum(s => s.LineTotalWithoutDiscountAndImposition);
            }
        }
        /// <summary>
        /// جمع فاکتور
        /// </summary>
        public virtual long SaleTotal
        {
            get
            {
                return CreditSaleDetailsTotal + UncreditSaleDetailsTotal + ProductSaleDetailsTotal;
            }
        }

        public virtual long SaleTotalWithoutDiscountAndImposition
        {
            get
            {
                return ProductSaleDetailsTotalWithoutDiscountAndImposition + UncreditSaleDetailsTotalWithoutDiscountAndImposition + CreditSaleDetailsTotalWithoutDiscountAndImposition;
            }
        }

        public virtual long TotalDiscount
        {
            get
            {
                long sumCreditDiscount = CreditSaleDetails.Sum(s => s.LineDiscount);// *CreditSaleDetails.Sum(x => x.Units);
                long sumProductDiscount = ProductSaleDetails.Sum(s => s.LineDiscount);// *ProductSaleDetails.Sum(x => x.Units);
                long sumUncreditDiscount = UncreditSaleDetails.Sum(s => s.LineDiscount);// *UncreditSaleDetails.Sum(x => x.Units);

                return sumCreditDiscount + sumProductDiscount + sumUncreditDiscount;
            }
        }

        public virtual long TotalImposition
        {
            get
            {
                long sumCreditImposition = CreditSaleDetails.Sum(s => s.LineImposition);// *CreditSaleDetails.Sum(x => x.Units);
                long sumProductImposition = ProductSaleDetails.Sum(s => s.LineImposition);/// * ProductSaleDetails.Sum(x => x.Units);
                long sumUncreditImposition = UncreditSaleDetails.Sum(s => s.LineImposition);// * UncreditSaleDetails.Sum(x => x.Units);

                return sumCreditImposition + sumProductImposition+ sumUncreditImposition;
            }
        }

        #endregion

        // آیا فاکتور برگشت از فروش است؟
        public virtual bool IsRollbackSale
        {
            get
            {
                try
                {
                    Sale msale = MainSale;
                    return MainSale != null;
                }
                catch
                {
                    return false;
                }

            }
        }



        #region Sale Details
        /// <summary>
        /// فروش اعتباری
        /// </summary>
        public virtual IList<CreditSaleDetail> CreditSaleDetails { get; protected set; }
        /// <summary>
        /// فروش کالا
        /// </summary>
        public virtual IList<ProductSaleDetail> ProductSaleDetails { get; protected set; }
        /// <summary>
        /// فروشهای غیر اعتباری
        /// </summary>
        public virtual IList<UncreditSaleDetail> UncreditSaleDetails { get; protected set; }
        #endregion

        #region Validation

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Customer == null)
                base.AddBrokenRule(SaleBusinessRules.CustomerRequired);
            //if(UncreditSaleDetails.Any(x => x.Discount>x.UncreditService.MaxDiscount)&& !IsRollbackSale)
            //    base.AddBrokenRule(SaleBusinessRules.LineDiscountnGrater);
            //if (UncreditSaleDetails.Any(x => x.Imposition > x.UncreditService.Imposition) && !IsRollbackSale)
            //    base.AddBrokenRule(SaleBusinessRules.LineDiscountnGrater);

            //if (CreditSaleDetails.Any(x => x.Discount > x.CreditService.MaxDiscount) && !IsRollbackSale)
            //    base.AddBrokenRule(SaleBusinessRules.LineDiscountnGrater);
            //if (CreditSaleDetails.Any(x => x.Imposition > x.CreditService.Imposition) && !IsRollbackSale)
            //    base.AddBrokenRule(SaleBusinessRules.LineDiscountnGrater);

            //if (ProductSaleDetails.Any(x => x.Discount > x.ProductPrice.MaxDiscount) && !IsRollbackSale)
            //    base.AddBrokenRule(SaleBusinessRules.LineDiscountnGrater);
            //if (ProductSaleDetails.Any(x => x.Imposition > x.ProductPrice.Imposition) && !IsRollbackSale)
            //    base.AddBrokenRule(SaleBusinessRules.LineDiscountnGrater);

            foreach (BusinessRule businessRule in _bussinessRules)
            {
                base.AddBrokenRule(businessRule);
            }
        }

        #endregion

        #region افزودن آیتم به فاکتور

        #region افزودن خدمات اعتباری

        public virtual void AddSaleDetail(CreditSaleDetail creditSaleDetail)
        {

            #region اگر تخفیف از حد مجاز بیشتر بود، به اندازه حداکثر شود

            if (creditSaleDetail.Discount > creditSaleDetail.CreditService.MaxDiscount)
            {
                creditSaleDetail.Discount = creditSaleDetail.CreditService.MaxDiscount;
            }

            #endregion

            #region اگر آیتم تکراری وارد شد، فقط تعداد و تخفیف آیتم موجود آپدیت شود

            if (CreditSaleDetails.Contains(creditSaleDetail))
            {
                CreditSaleDetails.FirstOrDefault(w => w == creditSaleDetail).Units += creditSaleDetail.Units;
                CreditSaleDetails.FirstOrDefault(w => w == creditSaleDetail).Discount += creditSaleDetail.Discount;
            }

            #endregion

            #region در غیر اینصورت آیتم ذخیره شود

            else
            {
                creditSaleDetail.Sale = this;
                CreditSaleDetails.Add(creditSaleDetail);
            }

            #endregion

        }

        public virtual void AddSaleDetails(IEnumerable<CreditSaleDetail> creditSaleDetails)
        {
            if (creditSaleDetails != null && creditSaleDetails.Count() > 0)
                foreach (CreditSaleDetail creditSaleDetail in creditSaleDetails)
                {
                    AddSaleDetail(creditSaleDetail);
                }
        }

        #endregion

        #region افزودن خدمات غیر اعتباری

        public virtual void AddSaleDetail(UncreditSaleDetail uncreditSaleDetail)
        {
            #region اگر تخفیف از حد مجاز بیشتر بود، به اندازه حداکثر شود

            if (uncreditSaleDetail.Discount > uncreditSaleDetail.UncreditService.MaxDiscount)
            {
                uncreditSaleDetail.Discount = uncreditSaleDetail.UncreditService.MaxDiscount;
            }

            #endregion

            #region اگر آیتم تکراری وارد شد، فقط تعداد و تخفیف آیتم موجود آپدیت شود

            if (UncreditSaleDetails.Contains(uncreditSaleDetail))
            {
                UncreditSaleDetails.FirstOrDefault(w => w == uncreditSaleDetail).Units += uncreditSaleDetail.Units;
                UncreditSaleDetails.FirstOrDefault(w => w == uncreditSaleDetail).Discount += uncreditSaleDetail.Discount;
            }

            #endregion

            #region در غیر اینصورت آیتم ذخیره شود

            else
            {
                uncreditSaleDetail.Sale = this;
                UncreditSaleDetails.Add(uncreditSaleDetail);
            }

            #endregion

        }

        public virtual void AddSaleDetails(IEnumerable<UncreditSaleDetail> uncreditSaleDetails)
        {
            if (uncreditSaleDetails != null && uncreditSaleDetails.Count() > 0)
                foreach (UncreditSaleDetail uncreditSaleDetail in uncreditSaleDetails)
                {
                    AddSaleDetail(uncreditSaleDetail);
                }
        }



        #endregion

        #region افزودن کالا

        public virtual void AddSaleDetail(ProductSaleDetail productSaleDetail)
        {
            #region اگر تخفیف از حد مجاز بیشتر بود، به اندازه حداکثر شود

            if (productSaleDetail.Discount > productSaleDetail.ProductPrice.MaxDiscount)
            {
                productSaleDetail.Discount = productSaleDetail.ProductPrice.MaxDiscount;
            }

            #endregion

            #region اگر آیتم تکراری وارد شد، فقط تعداد و تخفیف آیتم موجود آپدیت شود

            if (ProductSaleDetails.Contains(productSaleDetail))
            {
                ProductSaleDetails.FirstOrDefault(w => w == productSaleDetail).Units += productSaleDetail.Units;
                ProductSaleDetails.FirstOrDefault(w => w == productSaleDetail).Discount += productSaleDetail.Discount;
            }

            #endregion

            #region در غیر اینصورت آیتم ذخیره شود

            else
            {
                productSaleDetail.Sale = this;
                ProductSaleDetails.Add(productSaleDetail);
            }

            #endregion

        }

        public virtual void AddSaleDetails(IEnumerable<ProductSaleDetail> productSaleDetails)
        {
            if (productSaleDetails != null && productSaleDetails.Count() > 0)
                foreach (ProductSaleDetail productSaleDetail in productSaleDetails)
                {
                    AddSaleDetail(productSaleDetail);
                }
        }

        #endregion

        #endregion

        #region افزودن فاکتور برگشت از فروش به این فاکتور
        /// <summary>
        /// Add Rollback sale
        /// </summary>
        /// <param name="RollbackSale">Rollbacked Sale</param>
        public virtual void AddRollbackSale(Sale RollbackSale)
        {
            #region چک کردن اینکه فاکتور جاری فاکتور برگشت نباشد

            if (IsRollbackSale)
            {
                _bussinessRules.Add(SaleBusinessRules.ThisIsRollback);
                return;
            }
            #endregion

            #region آیا آیتم های فاکتور برگشتی با آیتم های فاکتور اصلی متفاوت نیست؟

            foreach (ProductSaleDetail productSaleDetail in RollbackSale.ProductSaleDetails)
            {
                if (!this.ProductSaleDetails.Contains(productSaleDetail))
                {
                    _bussinessRules.Add(SaleBusinessRules.SomeProductsNotExists);        
                    return;
                }
            }
            foreach (CreditSaleDetail creditSaleDetail in RollbackSale.CreditSaleDetails)
            {
                if (!this.CreditSaleDetails.Contains(creditSaleDetail))
                {
                    _bussinessRules.Add(SaleBusinessRules.SomeCreditsNotExists);
                    return;
                }
            }
            foreach (UncreditSaleDetail uncreditSaleDetail in RollbackSale.UncreditSaleDetails)
            {
                if (!this.UncreditSaleDetails.Contains(uncreditSaleDetail))
                {
                    _bussinessRules.Add(SaleBusinessRules.SomeUncreditsNotExists);
                    return;
                }
            }

            #endregion

            #region آیتمهای موجود در فاکتور برگشت قبلاً برگشت نشده باشند

            foreach (ProductSaleDetail productSaleDetail in RollbackSale.ProductSaleDetails)
            {
                if (ProductSaleDetails.FirstOrDefault(w => w == productSaleDetail).Status == SaleDetailStatus.Rollbacked)
                {
                    _bussinessRules.Add(SaleBusinessRules.SomeProductsBeforeRollbacked);
                    return;
                }
            }
            foreach (CreditSaleDetail creditSaleDetail in RollbackSale.CreditSaleDetails)
            {
                if (CreditSaleDetails.FirstOrDefault(w => w == creditSaleDetail).Status == SaleDetailStatus.Rollbacked)
                {
                    _bussinessRules.Add(SaleBusinessRules.SomeCreditsBeforeRollbacked);
                    return;
                }
            }
            foreach (UncreditSaleDetail uncreditSaleDetail in RollbackSale.UncreditSaleDetails)
            {
                if (UncreditSaleDetails.FirstOrDefault(w => w == uncreditSaleDetail).Status == SaleDetailStatus.Rollbacked)
                {
                    _bussinessRules.Add(SaleBusinessRules.SomeUncreditsBeforeRollbacked);
                    return;
                }
            }

            #endregion



            #region برای تک تک آیتم های فاکتور برگشتی

            #region خدمات اعتباری

            foreach (CreditSaleDetail creditSaleDetail in RollbackSale.CreditSaleDetails)
            {
                 //تخفیف صفر شود
                //creditSaleDetail.Discount = creditSaleDetail.Discount;
                // مالیات صفر شود
                //creditSaleDetail.Imposition = creditSaleDetail.Imposition;

                //creditSaleDetail.LineDiscount = creditSaleDetail.LineDiscount;

                //creditSaleDetail.LineImposition = creditSaleDetail.LineImposition;

                //creditSaleDetail.Rollbacked = true;
                creditSaleDetail.IsRollbackDetail = true;

                creditSaleDetail.ModifiedDate = PersianDateTime.Now;

                //if (creditSaleDetail.Delivered)
                //    creditSaleDetail.Status = SaleDetailStatus.DeliveredAndRollbacked;
                //else
                //    creditSaleDetail.Status = SaleDetailStatus.Rollbacked;

                // تعداد 1 شود
                //creditSaleDetail.Units = 1;

                // چک کردن مبلغ برگشتی که بیش از مبلغ فاکتور نباشد
                if (creditSaleDetail.UnitPrice > CreditSaleDetails.First(w => w == creditSaleDetail).UnitPrice)
                {
                    _bussinessRules.Add(SaleBusinessRules.RollbackCreditGreaterThanMain);
                    return;
                }

                // اگر تحویل شده باشد موجودی شبکه بروز شود
                if (CreditSaleDetails.First(w => w == creditSaleDetail).Status == SaleDetailStatus.Delivered)
                {
                    // چک کردن مبلغ برگشتی به شبکه که بیش از قیمت خرید نباشد
                    if (creditSaleDetail.PurchaseUnitPrice > CreditSaleDetails.First(w => w == creditSaleDetail).UnitPrice)
                    {
                        _bussinessRules.Add(SaleBusinessRules.RollbackCreditkGreaterThanMain);

                        return;
                    }

                    // بروز رسانی موجودی شبکه
                    CreditSaleDetails.First(w => w == creditSaleDetail).CreditService.Network.Balance +=
                        // به اندازه ای که از کاربر پرسیده می شود به شبکه برمی گردد
                        creditSaleDetail.PurchaseUnitPrice;
                }
            }

            #endregion

            #region خدمات غیر اعتباری

            foreach (UncreditSaleDetail uncreditSaleDetail in RollbackSale.UncreditSaleDetails)
            {
                //// تخفیف صفر شود
                uncreditSaleDetail.Discount = uncreditSaleDetail.Discount;
                //// مالیات صفر شود
                uncreditSaleDetail.Imposition = uncreditSaleDetail.Imposition;

                uncreditSaleDetail.LineDiscount = uncreditSaleDetail.LineDiscount;

                uncreditSaleDetail.LineImposition = uncreditSaleDetail.LineImposition;
                
                // تعداد 1 شود
                //uncreditSaleDetail.Units = uncreditSaleDetail.Units;

                //uncreditSaleDetail.Rollbacked = true;
                uncreditSaleDetail.IsRollbackDetail = true;

                uncreditSaleDetail.ModifiedDate = PersianDateTime.Now;

                //if (uncreditSaleDetail.Delivered)
                //    uncreditSaleDetail.Status = SaleDetailStatus.DeliveredAndRollbacked;
                //else
                //    uncreditSaleDetail.Status = SaleDetailStatus.Rollbacked;
                // چک کردن مبلغ برگشتی که بیش از مبلغ فاکتور نباشد
                if (uncreditSaleDetail.UnitPrice > UncreditSaleDetails.First(w => w == uncreditSaleDetail).UnitPrice)
                {
                    _bussinessRules.Add(SaleBusinessRules.RollbackUncreditGreaterThanMain);
                    return;
                }

            }

            #endregion

            #region کالاها

            foreach (ProductSaleDetail productSaleDetail in RollbackSale.ProductSaleDetails)
            {
                //// تخفیف صفر شود
                productSaleDetail.Discount = productSaleDetail.Discount;
                //// مالیات صفر شود
                productSaleDetail.Imposition = productSaleDetail.Imposition;

                productSaleDetail.LineDiscount = productSaleDetail.LineDiscount;

                productSaleDetail.LineImposition = productSaleDetail.LineImposition;
                // تعداد 1 شود
                productSaleDetail.Units = productSaleDetail.Units;

                //productSaleDetail.Rollbacked = true;
                productSaleDetail.IsRollbackDetail = true;


                productSaleDetail.ModifiedDate = PersianDateTime.Now;

                
                //if (productSaleDetail.Delivered)
                //    productSaleDetail.Status = SaleDetailStatus.DeliveredAndRollbacked;
                //else
                //    productSaleDetail.Status = SaleDetailStatus.Rollbacked;


                // چک کردن مبلغ برگشتی به حساب مشتری که بیش از مبلغ فاکتور نباشد
                if (productSaleDetail.UnitPrice > ProductSaleDetails.First(w => w == productSaleDetail).UnitPrice)
                {
                    _bussinessRules.Add(SaleBusinessRules.RollbackProductGreaterThanMain);
                    return;
                }

                // اگر قبلا تحویل شده باشد موجودی کالاها بروز شود
                if (ProductSaleDetails.First(w => w == productSaleDetail).Status == SaleDetailStatus.Delivered)
                {
                    // بروزرسانی موجودی کالا در انبار
                    this.ProductSaleDetails.First(w => w == productSaleDetail).ProductPrice.Product.UnitsInStock +=
                        // به اندازه تعداد فروخته شده به انبار برمیگردد
                        ProductSaleDetails.First(w => w == productSaleDetail).Units;
                }

            }

            #endregion

            #endregion

            #region بروز رسانی حساب مشتری

            // توضیح اینکه مبلغ بازگشتی به حساب مشتری در مورد فروشهای
            // اعتباری با مبلغ بازگشتی به شبکه متفاوت است 
            // و به اندازه جمع یونیت پرایز می باشد

            // بروز رسانی حساب مشتری ربطی به تحویل یا عدم تحویل ندارد
            //Customer.Balance += SaleTotal;

            #endregion



            #region افزودن فاکتور برگشت از فروش

            // اگر کامپایلر به اینجا رسید یعنی هیچ مشکلی وجود ندارد

            // فاکتور برگشت از فروش بصورت پیش فرض تأیید شده است
            RollbackSale.Close(RollbackSale.CreateEmployee);

            RollbackedSales.Add(RollbackSale);

            #endregion
        }

        #endregion

        #region حذف آیتم از فاکتور

        #region حذف خدمات اعتباری

        public virtual void DeleteSaleDetail(CreditSaleDetail creditSaleDetail)
        {
            #region فاکتور بسته نشده باشد

            if (creditSaleDetail.Sale.Closed == true)
            {
                _bussinessRules.Add(SaleBusinessRules.SaleClosed);
                return;
            }

            #endregion

            #region فاکتور برگشت از فروش نباشد

            if (creditSaleDetail.Sale.IsRollbackSale == true)
            {
                _bussinessRules.Add(SaleBusinessRules.ThisIsRollbackAndCantDelete);
                return;
            }

            #endregion

            // حذف آیتم مورد نظر
            this.CreditSaleDetails.Remove(creditSaleDetail);
        }

        public virtual void DeleteSaleDetails(IEnumerable<CreditSaleDetail> creditSaleDetails)
        {
            if (creditSaleDetails != null && creditSaleDetails.Count() > 0)
                foreach (CreditSaleDetail creditSaleDetail in creditSaleDetails)
                {
                    DeleteSaleDetail(creditSaleDetail);
                }
        }

        #endregion

        #region حذف خدمات غیر اعتباری

        public virtual void DeleteSaleDetail(UncreditSaleDetail uncreditSaleDetail)
        {
            #region فاکتور بسته نشده باشد

            if (uncreditSaleDetail.Sale.Closed == true)
            {
                _bussinessRules.Add(SaleBusinessRules.SaleClosed);
                return;
            }

            #endregion

            #region فاکتور برگشت از فروش نباشد

            if (uncreditSaleDetail.Sale.IsRollbackSale == true)
            {
                _bussinessRules.Add(SaleBusinessRules.ThisIsRollbackAndCantDelete);
                return;
            }

            #endregion

            // حذف آیتم مورد نظر
            this.UncreditSaleDetails.Remove(uncreditSaleDetail);
        }

        public virtual void DeleteSaleDetails(IEnumerable<UncreditSaleDetail> uncreditSaleDetails)
        {
            if (uncreditSaleDetails != null && uncreditSaleDetails.Count() > 0)
                foreach (UncreditSaleDetail uncreditSaleDetail in uncreditSaleDetails)
                {
                    DeleteSaleDetail(uncreditSaleDetail);
                }
        }

        #endregion


        #region حذف کالا

        public virtual void DeleteSaleDetail(ProductSaleDetail productSaleDetail)
        {
            #region فاکتور بسته نشده باشد

            if (productSaleDetail.Sale.Closed == true)
            {
                _bussinessRules.Add(SaleBusinessRules.SaleClosed);
                return;
            }

            #endregion

            #region فاکتور برگشت از فروش نباشد

            if (productSaleDetail.Sale.IsRollbackSale == true)
            {
                _bussinessRules.Add(SaleBusinessRules.ThisIsRollbackAndCantDelete);
                return;
            }

            #endregion

            // حذف آیتم مورد نظر
            this.ProductSaleDetails.Remove(productSaleDetail);
        }

        public virtual void DeleteSaleDetails(IEnumerable<ProductSaleDetail> productSaleDetails)
        {
            if (productSaleDetails != null && productSaleDetails.Count() > 0)
                foreach (ProductSaleDetail productSaleDetail in productSaleDetails)
                {
                    DeleteSaleDetail(productSaleDetail);
                }
        }

        #endregion

        #endregion

    }
}
