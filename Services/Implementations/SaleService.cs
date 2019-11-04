#region Usings

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Sales.Interfaces;
using Model.Customers.Interfaces;
using Model.Employees.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.SaleCatalogService;
using Model.Sales;
using Services.ViewModels.Sales;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Store.Interfaces;
using Services.ViewModels.Store;
using Model.Employees;
using Infrastructure.Querying;
using Model.Customers;
using Infrastructure.Domain;
using Model.Fiscals;
using Model.Fiscals.Interfaces;
using Model.Interfaces;
using Model;
using Services.ViewModels.Reports;

#endregion

namespace Services.Implementations
{

    public class SaleService : ISaleService
    {
        #region Declares

        private readonly ISaleRepository _saleRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _uow;
        private readonly IProductPriceRepository _productPriceRepository;
        private readonly ICreditServiceRepository _creditServiceRepository;
        private readonly IUncreditServiceRepository _uncreditServiceRepository;
        private readonly IProductSaleDetailRepository _productSaleDetailRepository;
        private readonly ICreditSaleDetailRepository _creditSaleDetailRepository;
        private readonly IUncreditSaleDetailRepository _uncreditSaleDetailRepository;
        private readonly IFiscalRepository _fiscalRepository;
        private readonly ICustomerLevelRepository _customerLevelRepository;
        private readonly ILevelRepository _levelRepository;
        private readonly ISystemCountersRepository _systemCountersrepository;
        private readonly IBonusComissionRepository _bonusComissionRepository;
        private readonly ICourierRepository _courierRepository;
        private readonly INetworkCenterRepository _networkCenterRepository;

        #endregion

        #region Ctor

        public SaleService(ISaleRepository saleRepository, IUnitOfWork uow)
        {
            _saleRepository = saleRepository;
            _uow = uow;
        }

        public SaleService(ISaleRepository saleRepository, ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository, IUnitOfWork uow,
            IProductPriceRepository productPriceRepository, ICreditServiceRepository creditServiceRepository,
            IUncreditServiceRepository uncreditServiceRepository,
            IProductSaleDetailRepository productSaleDetailRepository,
            ICreditSaleDetailRepository creditSaleDetailRepository,
            IUncreditSaleDetailRepository uncreditSaleDetailRepository,
            IFiscalRepository fiscalRepository,
            ICustomerLevelRepository customerLevelRepository,
            ILevelRepository levelRepository, ISystemCountersRepository systemCountersRepository,
            IBonusComissionRepository bonusComissionRepository,
            ICourierRepository courierRepository, INetworkCenterRepository networkCenterRepository)
            : this(saleRepository, uow)
        {
            this._customerRepository = customerRepository;
            this._employeeRepository = employeeRepository;
            _productPriceRepository = productPriceRepository;
            _creditServiceRepository = creditServiceRepository;
            _uncreditServiceRepository = uncreditServiceRepository;
            _productSaleDetailRepository = productSaleDetailRepository;
            _creditSaleDetailRepository = creditSaleDetailRepository;
            _uncreditSaleDetailRepository = uncreditSaleDetailRepository;
            _fiscalRepository = fiscalRepository;
            _customerLevelRepository = customerLevelRepository;
            _levelRepository = levelRepository;
            _bonusComissionRepository = bonusComissionRepository;
            _systemCountersrepository = systemCountersRepository;
            _courierRepository = courierRepository;
            _networkCenterRepository = networkCenterRepository;
        }

        #endregion

        #region CreateSaleNumber

        private string CreateSaleNumber()
        {
            int serialnumber = 0;
            //GetSalesResponse allSales = GetSales();

            //int SaleNumber = Convert.ToInt32(allSales.SaleViews.Max(m => m.SaleNumber)) + 1;
            //string SaleNumberStr = SaleNumber.ToString();
            //if (SaleNumber == 1)
            //{
            //    SaleNumberStr = PersianDateTime.Now.Substring(2, 2) + "0000" + SaleNumberStr;
            //}

            //string saleNumberYear = SaleNumberStr.Substring(0, 2);
            //string nowYear = PersianDateTime.Now.Substring(2, 2);

            try
            {
                IEnumerable<SystemCounters> counter = _systemCountersrepository.FindAll();

                serialnumber = counter.First().LastFactorSerialNumber + 1;
                counter.First().LastFactorSerialNumber = serialnumber;

                _systemCountersrepository.Save(counter.First());
                _uow.Commit();
                return serialnumber.ToString();
            }
            catch (Exception ex)
            {
                return "Error";
            }
            //if (saleNumberYear == nowYear)
            //    return SaleNumberStr;
            //else
            //    return nowYear + "00001";
        }

        #endregion

        #region Add

        public GeneralResponse AddSale(AddSaleRequest request)
        {
            decimal Upercentage = 0;
            decimal Ppercentage = 0;
            decimal Cpercentage = 0;

            GeneralResponse response = new GeneralResponse();

            try
            {
                Customer customer = this._customerRepository.FindBy(request.CustomerID);
                //آیا در این مرحله امکان فاکتور برای مشتری در این شبکه وجود دارد

                if (customer.Level.IsFirstLevel)
                {
                    if (customer.Network == null && !customer.Level.HasRequireNetwork)
                    {
                        response.ErrorMessages.Add("افزودن فاکتور به دلیل مشخص نبودن شبکه مشتری امکان پدیر نیست.");
                        return response;
                    }
                    if (customer.Center == null)
                    {
                        response.ErrorMessages.Add("افزودن فاکتور به دلیل مشخص نبودن مرکز مخابراتی مشتری امکان پدیر نیست.");
                        return response;
                    }
                    if (customer.Network != null && customer.Level.HasRequireNetwork)
                    {
                        NetworkCenter networkCenter = new NetworkCenter();
                    
                        Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                        Criterion criteria1 = new Criterion("Network.ID", customer.Network.ID, CriteriaOperator.Equal);
                        Criterion criteria2 = new Criterion("Center.ID", customer.Center.ID, CriteriaOperator.Equal);
                        query.Add(criteria1);
                        query.Add(criteria2);

                        networkCenter = _networkCenterRepository.FindBy(query).FirstOrDefault();

                        if (!networkCenter.CanSale)
                        {
                            response.ErrorMessages.Add("اجازه فروش برای این مرکز/شبکه در این مرحله وجود ندارد");
                            return response;
                        }
                    }
                }

                // چک کردن اینکه آیا در این مرحله امکان عملیات فاکتور وجود دارد یا خیر


                #region در صورتی که اقلام فاکتور قبلا تحویل شده باشد به اعتبار قابل تحویل مشتری اضافه میشود

                Sale sale = new Sale();

                #endregion


                if (!request.IsRollback)
                {
                    Level level = _levelRepository.FindBy(customer.Level.ID);
                    if (level.Options == null || !level.Options.CanSale)
                    {
                        response.ErrorMessages.Add("SaleIsNotPermitedInThisLevel");
                        return response;
                    }
                }

                #region Sale Properties



                // For Rollback this is the current Sale and sale is the rollbacking sale
                Sale oldSale = new Sale();
                bool adding = true;

                // if this is new Sale
                if (request.SaleID == Guid.Empty)
                {

                    sale.ID = Guid.NewGuid();
                    response.ID = sale.ID;
                    sale.CreateDate = PersianDateTime.Now;
                    sale.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                    sale.Customer = this._customerRepository.FindBy(request.CustomerID);
                    //sale.SaleNumber = CreateSaleNumber();
                    sale.RowVersion = 1;
                }
                // if this is exist sale
                else
                {
                    adding = false;
                    // if this is rollback sale
                    if (request.IsRollback)
                    {
                        // creating new sale (this is actually rollback sale)
                        sale.ID = Guid.NewGuid();
                        response.ID = sale.ID;
                        sale.CreateDate = PersianDateTime.Now;
                        sale.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                        sale.Customer = this._customerRepository.FindBy(request.CustomerID);
                        string SerialNumber = CreateSaleNumber();
                        if (SerialNumber != "Error")
                            sale.SaleNumber = SerialNumber;
                        else
                        {
                            response.ErrorMessages.Add("اشکال در تخصیص شماره فاکتور");
                            return response;
                        }
                        //
                        sale.RowVersion = 1;
                        sale.MainSale = _saleRepository.FindBy(request.SaleID);
                        sale.HasCourier = sale.MainSale.HasCourier;

                        // getting current sale
                        oldSale = sale.MainSale;
                        
                        #region تنظیم برگشت فاکتور اصلی

                        foreach (var item in oldSale.ProductSaleDetails)
                        {
                            foreach (var _item in request.AddProductSaleDetailRequests)
                            {
                                if (item.ProductPrice.ID == _item.ProductPriceID)
                                {
                                    if (item.IsRollbackDetail)
                                    {
                                        response.ErrorMessages.Add("برگشت از فروش این محصول قبلا انجام شده است");
                                        return response;
                                    }
                                    if (item.Delivered == false &&
                                        item.LineTotalWithoutDiscountAndImposition > _item.RollbackPrice)
                                    {

                                        response.ErrorMessages.Add(
                                            "این آیتم تحویل نشده لذا مبلغ برگشتی نمیتواند کمتر از مبلغ فروش باشد");
                                        return response;
                                    }
                                    if (item.LineTotalWithoutDiscountAndImposition < _item.RollbackPrice)
                                    {
                                        response.ErrorMessages.Add("مبلغ برگشتی نمیتواند بیشتر از مبلغ فروش باشد");
                                        return response;
                                    }
                                    _item.RollbackDate = PersianDateTime.Now;
                                    item.Rollbacked = true;
                                    if (item.Delivered || _item.IsDeliverdBefor)
                                        _item.Status = SaleDetailStatus.DeliveredAndRollbacked;
                                }
                            }
                        }

                        foreach (var item in oldSale.CreditSaleDetails)
                        {
                            foreach (var _item in request.AddCreditSaleDetailRequests)
                            {
                                if (item.CreditService.ID == _item.CreditServiceID)
                                {
                                    if (item.IsRollbackDetail)
                                    {
                                        response.ErrorMessages.Add("برگشت از فروش این محصول قبلا انجام شده است");
                                        return response;
                                    }
                                    if (item.Delivered == false &&
                                        item.LineTotalWithoutDiscountAndImposition > _item.RollbackPrice)
                                    {

                                        response.ErrorMessages.Add(
                                            "این آیتم تحویل نشده لذا مبلغ برگشتی نمیتواند کمتر از مبلغ فروش باشد");
                                        return response;
                                    }
                                    if (item.LineTotalWithoutDiscountAndImposition < _item.RollbackPrice)
                                    {
                                        response.ErrorMessages.Add("مبلغ برگشتی نمیتواند بیشتر از مبلغ فروش باشد");
                                        return response;
                                    }
                                    _item.RollbackDate = PersianDateTime.Now;
                                    item.Rollbacked = true;
                                    if (item.Delivered || _item.IsDeliverdBefor)
                                        _item.Status = SaleDetailStatus.DeliveredAndRollbacked;
                                }
                            }
                        }
                        foreach (var item in oldSale.UncreditSaleDetails)
                        {
                            foreach (var _item in request.AddUncreditSaleDetailRequests)
                            {
                                if (item.UncreditService.ID == _item.UncreditServiceID)
                                {
                                    if (item.IsRollbackDetail)
                                    {
                                        response.ErrorMessages.Add("برگشت از فروش این محصول قبلا انجام شده است");
                                        return response;
                                    }
                                    if (item.Delivered == false &&
                                        item.LineTotalWithoutDiscountAndImposition > _item.RollbackPrice)
                                    {

                                        response.ErrorMessages.Add(
                                            "این آیتم تحویل نشده لذا مبلغ برگشتی نمیتواند کمتر از مبلغ فروش باشد");
                                        return response;
                                    }
                                    if (item.LineTotalWithoutDiscountAndImposition < _item.RollbackPrice)
                                    {
                                        response.ErrorMessages.Add("مبلغ برگشتی نمیتواند بیشتر از مبلغ فروش باشد");
                                        return response;
                                    }
                                    _item.RollbackDate = PersianDateTime.Now;
                                    _item.ID = Guid.NewGuid();

                                    item.Rollbacked = true;
                                    if (item.Delivered || _item.IsDeliverdBefor)
                                        _item.Status = SaleDetailStatus.DeliveredAndRollbacked;

                                }
                            }
                        }

                        #endregion

                        _saleRepository.Save(oldSale);

                        if (request.AddUncreditSaleDetailRequests.Any())
                        {
                            foreach (var item in request.AddUncreditSaleDetailRequests)
                            {
                                bool temp = sale.MainSale.UncreditSaleDetails.Where(x => x.ID == item.MainUncreditSaleDetailID).FirstOrDefault().BonusDate == null ? false : true;
                                if (temp)
                                item.HasCourier = sale.HasCourier;
                                decimal a =
                                    oldSale.UncreditSaleDetails.Where(x => x.Rollbacked).Where(x=>x.ID==item.MainUncreditSaleDetailID)
                                        .Sum(x => x.LineTotalWithoutDiscountAndImposition);
                                decimal b = item.RollbackPrice;
                                if (a == 0 && b == 0)
                                    Upercentage = 1;
                                else
                                    Upercentage = b/a;
                                sale.AddSaleDetail(PrepareUnuncreditSaleDetail(item,Upercentage));
                            }
                        }
                        if (request.AddProductSaleDetailRequests.Any())
                        {
                            foreach (var item in request.AddProductSaleDetailRequests)
                            {
                                bool temp = sale.MainSale.ProductSaleDetails.Where(x => x.ID == item.MainProductSaleDetailID).FirstOrDefault().BonusDate == null ? false : true;
                                if (temp)
                                item.HasCourier = sale.HasCourier;
                                decimal a =
                                    oldSale.ProductSaleDetails.Where(x => x.Rollbacked).Where(x => x.ID == item.MainProductSaleDetailID)
                                        .Sum(x => x.LineTotalWithoutDiscountAndImposition);
                                decimal b = item.RollbackPrice;
                                if (a == 0 && b == 0)
                                    Upercentage = 1;
                                else
                                    Upercentage = b / a;
                                sale.AddSaleDetail(PrepareProductSaleDetail(item, Upercentage));
                            }
                        }

                        if (request.AddCreditSaleDetailRequests.Any())
                        {
                            foreach (var item in request.AddCreditSaleDetailRequests)
                            {
                                bool temp = sale.MainSale.CreditSaleDetails.Where(x => x.ID == item.MainCreditSaleDetailID).FirstOrDefault().BonusDate == null ? false : true;
                                if(temp)
                                item.HasCourier = sale.HasCourier;
                                decimal a =
                                    oldSale.CreditSaleDetails.Where(x => x.Rollbacked).Where(x => x.ID == item.MainCreditSaleDetailID)
                                        .Sum(x => x.LineTotalWithoutDiscountAndImposition);
                                decimal b = item.RollbackPrice;
                                if (a == 0 && b == 0)
                                    Upercentage = 1;
                                else
                                    Upercentage = b / a;
                                sale.AddSaleDetail(PrepareCreditSaleDetail(item, Upercentage));
                            }
                        }

                    }
                    // then this is only adding itmes to old sale
                    else
                    {
                        sale = _saleRepository.FindBy(request.SaleID);
                    }

                }

                #endregion

                #region Add Sale Details

                if (adding)
                {
                    IEnumerable<ProductSaleDetail> products = PrepareProductSaleDetails(
                        request.AddProductSaleDetailRequests, Ppercentage);
                    sale.AddSaleDetails(products);
                    IEnumerable<CreditSaleDetail> credites =
                        PrepareCreditSaleDetails(request.AddCreditSaleDetailRequests,
                            Cpercentage);
                    sale.AddSaleDetails(credites);
                    IEnumerable<UncreditSaleDetail> uncredits =
                        PrepareUnuncreditSaleDetails(request.AddUncreditSaleDetailRequests, Upercentage);
                    sale.AddSaleDetails(uncredits);
                }



                #endregion

                // if we are rolling back
                if (request.IsRollback)
                {
                    // adding rollbackSale to this sale
                    oldSale.AddRollbackSale(sale);

                }

                response.ID = sale.ID;

                #region Validation

                if (sale.GetBrokenRules().Count() > 0)
                {


                    foreach (BusinessRule businessRule in sale.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                if (request.IsRollback && oldSale.GetBrokenRules().Count() > 0)
                {


                    foreach (BusinessRule businessRule in oldSale.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                #region validation

                if (sale.UncreditSaleDetails.Any())
                if (sale.UncreditSaleDetails.FirstOrDefault().GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in sale.UncreditSaleDetails.FirstOrDefault().GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }
                    
                    return response;
                }

                if (sale.ProductSaleDetails.Any())
                if (sale.ProductSaleDetails.FirstOrDefault().GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in sale.ProductSaleDetails.FirstOrDefault().GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                if (sale.CreditSaleDetails.Any())
                if (sale.CreditSaleDetails.FirstOrDefault().GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in sale.CreditSaleDetails.FirstOrDefault().GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                //if (sale.CreditSaleDetails.Any(item => item.LineTotal != (item.Units*item.Units) - item.LineDiscount + item.LineImposition &&
                //                                       item.IsRollbackDetail == false))
                //{
                //    response.ErrorMessages.Add("اختلاف محاسباتی .... لطفا با برنامه نویس تماس بگیرید.");
                //    return response;
                //}

                //if (sale.UncreditSaleDetails.Any(item => item.LineTotal != (item.Units * item.Units) - item.LineDiscount + item.LineImposition &&
                //                       item.IsRollbackDetail == false))
                //{
                //    response.ErrorMessages.Add("اختلاف محاسباتی .... لطفا با برنامه نویس تماس بگیرید.");
                //    return response;
                //}
                //if (sale.ProductSaleDetails.Any(item => item.LineTotal != (item.Units * item.Units) - item.LineDiscount + item.LineImposition &&
                //                       item.IsRollbackDetail == false))
                //{
                //    response.ErrorMessages.Add("اختلاف محاسباتی .... لطفا با برنامه نویس تماس بگیرید.");
                //    return response;
                //}
                _saleRepository.Add(sale);



                if (request.IsRollback)
                {
                    long rollbackSum = 0;
                    long balanceRollback = 0;
                    foreach (var item in request.AddProductSaleDetailRequests)
                    {
                        foreach (var _item in sale.ProductSaleDetails)
                        {
                            item.MainProductSaleDetailID = _item.ID;
                            if (_item.ProductPrice.ID == item.ProductPriceID)
                            {
                                balanceRollback += _item.RollbackPrice - _item.LineDiscount + _item.LineImposition;
                                if (item.IsDeliverdBefor)
                                    rollbackSum += _item.RollbackPrice - _item.LineDiscount + _item.LineImposition;
                            }
                        }

                    }

                    foreach (var item in request.AddCreditSaleDetailRequests)
                    {
                        foreach (var _item in sale.CreditSaleDetails)
                        {
                            item.MainCreditSaleDetailID = _item.ID;
                            if (_item.CreditService.ID == item.CreditServiceID)
                            {

                                balanceRollback += _item.RollbackPrice - _item.LineDiscount + _item.LineImposition;
                                if (item.IsDeliverdBefor)
                                    rollbackSum += _item.RollbackPrice - _item.LineDiscount + _item.LineImposition;
                            }
                        }

                    }
                    foreach (var item in request.AddUncreditSaleDetailRequests)
                    {
                        foreach (var _item in sale.UncreditSaleDetails)
                        {
                            item.MainUncreditSaleDetailID = _item.ID;
                            if (_item.UncreditService.ID == item.UncreditServiceID)
                            {
                                balanceRollback += _item.RollbackPrice - _item.LineDiscount + _item.LineImposition;
                                if (item.IsDeliverdBefor)
                                    rollbackSum += _item.RollbackPrice - _item.LineDiscount + _item.LineImposition;
                            }
                        }

                    }
                    customer.Balance += balanceRollback;
                    customer.CanDeliverCost += rollbackSum;
                    _customerRepository.Save(customer);
                    

                }
                response.ObjectAdded = new
                {
                    AdslPhone = customer.ADSLPhone,
                    Name = customer.Name,
                    ObjectAdded = new { Products = sale.ProductSaleDetails.ConvertToProductSaleDetailViews(), UnCredits = sale.UncreditSaleDetails.ConvertToUncreditSaleDetailViews(), Credit = sale.CreditSaleDetails.ConvertToCreditSaleDetailViews() }

                };

                _uow.Commit();

            }
            catch (Exception ex)
            {

                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        #region PrepareProductSaleDetails

        private ProductSaleDetail PrepareProductSaleDetail(AddProductSaleDetailRequest request, decimal ppercentage)
        {

            ProductSaleDetail productSaleDetail = new ProductSaleDetail();

            productSaleDetail.ID = Guid.NewGuid();
            productSaleDetail.CreateDate = PersianDateTime.Now;
            productSaleDetail.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
            productSaleDetail.ProductPrice = _productPriceRepository.FindBy(request.ProductPriceID);
            
            productSaleDetail.UnitPrice = request.UnitPrice;
            productSaleDetail.Units = request.Units;
            productSaleDetail.Imposition = request.Imposition;
            productSaleDetail.Discount = request.Discount;
            productSaleDetail.RowVersion = 1;

            _productSaleDetailRepository.FindBy(request.MainProductSaleDetailID);


            productSaleDetail.IsRollbackDetail = request.IsRollbackDetail;
            if (request.IsRollbackDetail)
            {

                if (request.MainProductSaleDetailID != Guid.Empty)
                {
                    ProductSaleDetail psd = _productSaleDetailRepository.FindBy(request.MainProductSaleDetailID);
                    productSaleDetail.MainSaleDetail = psd;
                    productSaleDetail.SaleEmployee =psd.SaleEmployee;
                }

                productSaleDetail.RollbackNote = request.RollbackNote;
                productSaleDetail.RollbackPrice = request.RollbackPrice;
                productSaleDetail.Discount = 0;
                // Convert.ToInt32(request.CanRollbackDiscountPrice / request.Units * ppercentage) - (Convert.ToInt32(request.CanRollbackDiscountPrice / request.Units * ppercentage) % 5000);
                productSaleDetail.Imposition = 0;
                // Convert.ToInt32(request.CanRollbackImpositionPrice / request.Units * ppercentage) - (Convert.ToInt32(request.CanRollbackImpositionPrice / request.Units * ppercentage) % 5000);
                productSaleDetail.Units = request.Units;
                productSaleDetail.LineDiscount = Convert.ToInt32(request.CanRollbackDiscountPrice * ppercentage) -
                                                 (Convert.ToInt32(request.CanRollbackDiscountPrice * ppercentage) % 1);
                productSaleDetail.LineImposition = Convert.ToInt32(request.CanRollbackImpositionPrice * ppercentage) -
                                                   (Convert.ToInt32(request.CanRollbackImpositionPrice * ppercentage) % 1);
                ;
                productSaleDetail.LineTotal = request.RollbackPrice - productSaleDetail.LineDiscount +
                                              productSaleDetail.LineImposition;
                
                if (productSaleDetail.MainSaleDetail.Delivered)
                {
                    productSaleDetail.Comission =-
                        Convert.ToInt32(productSaleDetail.MainSaleDetail.Comission*ppercentage) -
                        (Convert.ToInt32(productSaleDetail.MainSaleDetail.Comission*ppercentage)%1);
                    productSaleDetail.ComissionDate = PersianDateTime.Now;
                }
                if (productSaleDetail.MainSaleDetail.Sale.Closed)
                {
                    productSaleDetail.Bonus =- Convert.ToInt32(productSaleDetail.MainSaleDetail.Bonus * ppercentage) -
                                          (Convert.ToInt32(productSaleDetail.MainSaleDetail.Bonus * ppercentage) % 1);
                    if(request.HasCourier)
                    productSaleDetail.BonusDate = PersianDateTime.Now;
                }


                productSaleDetail.RollbackNote = request.RollbackNote;
                productSaleDetail.RollbackPrice = request.RollbackPrice;
                productSaleDetail.Status = request.Status;

            }
            else
            {
                productSaleDetail.SaleEmployee = productSaleDetail.CreateEmployee;
                productSaleDetail.Discount = request.Discount;
                productSaleDetail.Imposition = request.Imposition;
                productSaleDetail.UnitPrice = request.UnitPrice;
                productSaleDetail.Units = request.Units;
                productSaleDetail.LineDiscount = request.LineDiscount;
                productSaleDetail.LineImposition = request.LineImposition;
                productSaleDetail.LineTotalWithoutDiscountAndImposition = request.LineTotalWithoutDiscountAndImposition;
                productSaleDetail.LineTotal = request.Units * request.UnitPrice - productSaleDetail.LineDiscount +
                                              productSaleDetail.LineImposition;
                productSaleDetail.Status = SaleDetailStatus.Nothing;
            }
            productSaleDetail.Rollbacked = request.Rollbacked;

            return productSaleDetail;

        }

        private IEnumerable<ProductSaleDetail> PrepareProductSaleDetails(
            IEnumerable<AddProductSaleDetailRequest> requests, decimal ppercentage)
        {
            IList<ProductSaleDetail> response = new List<ProductSaleDetail>();

            if (requests != null && requests.Count() > 0)
                foreach (AddProductSaleDetailRequest request in requests)
                {
                    response.Add(PrepareProductSaleDetail(request, ppercentage));
                }

            return response;
        }

        #endregion

        #region PrepareCreditSaleDetails

        private CreditSaleDetail PrepareCreditSaleDetail(AddCreditSaleDetailRequest request, decimal cpercentage)
        {
            CreditSaleDetail creditSaleDetail = new CreditSaleDetail();

            creditSaleDetail.ID = Guid.NewGuid();
            creditSaleDetail.CreateDate = PersianDateTime.Now;
            creditSaleDetail.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
            creditSaleDetail.CreditService = _creditServiceRepository.FindBy(request.CreditServiceID);
            
            creditSaleDetail.Discount = request.Discount;
            creditSaleDetail.Imposition = request.Imposition;
            creditSaleDetail.UnitPrice = request.UnitPrice;
            creditSaleDetail.PurchaseUnitPrice =
                _creditServiceRepository.FindBy(request.CreditServiceID).PurchaseUnitPrice;
            creditSaleDetail.Units = request.Units;
            creditSaleDetail.RowVersion = 1;
            creditSaleDetail.Status = request.Status;

            creditSaleDetail.RollbackNetworkPrice = request.RollbackNetworkPrice == null
                ? 0
                : (long)request.RollbackNetworkPrice;
            creditSaleDetail.RollbackNote = request.RollbackNote;
            if (request.MainCreditSaleDetailID != Guid.Empty)
            {
                CreditSaleDetail psd = _creditSaleDetailRepository.FindBy(request.MainCreditSaleDetailID);
                creditSaleDetail.MainSaleDetail = psd;
                creditSaleDetail.SaleEmployee = psd.SaleEmployee;
            }

            creditSaleDetail.IsRollbackDetail = request.IsRollbackDetail;
            if (request.IsRollbackDetail)
            {
                creditSaleDetail.RollbackPrice = request.RollbackPrice;
                creditSaleDetail.Discount = 0;
                //Convert.ToInt32(request.CanRollbackDiscountPrice / request.Units * cpercentage) - (Convert.ToInt32(request.CanRollbackDiscountPrice / request.Units * cpercentage) % 5000);
                creditSaleDetail.Imposition = 0;
                //Convert.ToInt32(request.CanRollbackImpositionPrice / request.Units * cpercentage) - (Convert.ToInt32(request.CanRollbackImpositionPrice / request.Units * cpercentage) % 5000);
                creditSaleDetail.Units = request.Units;
                creditSaleDetail.LineDiscount = Convert.ToInt32(request.CanRollbackDiscountPrice * cpercentage) -
                                                (Convert.ToInt32(request.CanRollbackDiscountPrice * cpercentage) % 1);
                creditSaleDetail.LineImposition = Convert.ToInt32(request.CanRollbackImpositionPrice * cpercentage) -
                                                  (Convert.ToInt32(request.CanRollbackImpositionPrice * cpercentage) % 1);
 
                creditSaleDetail.LineTotal = request.RollbackPrice - creditSaleDetail.LineDiscount +
                                             creditSaleDetail.LineImposition;

                if (creditSaleDetail.MainSaleDetail.Delivered)
                {
                    creditSaleDetail.Comission =-
                        Convert.ToInt32(creditSaleDetail.MainSaleDetail.Comission * cpercentage) -
                        (Convert.ToInt32(creditSaleDetail.MainSaleDetail.Comission * cpercentage) % 1);
                    creditSaleDetail.ComissionDate = PersianDateTime.Now;
                }
                if (creditSaleDetail.MainSaleDetail.Sale.Closed)
                {
                    creditSaleDetail.Bonus = -Convert.ToInt32(creditSaleDetail.MainSaleDetail.Bonus * cpercentage) -
                                          (Convert.ToInt32(creditSaleDetail.MainSaleDetail.Bonus * cpercentage) % 1);
                    if (request.HasCourier)
                        creditSaleDetail.BonusDate = PersianDateTime.Now;
                    
                }

                creditSaleDetail.RollbackNote = request.RollbackNote;

            }
            else
            {
                creditSaleDetail.SaleEmployee = creditSaleDetail.CreateEmployee;
                creditSaleDetail.Discount = request.Discount;
                creditSaleDetail.Imposition = request.Imposition;
                creditSaleDetail.UnitPrice = request.UnitPrice;
                creditSaleDetail.Units = request.Units;
                creditSaleDetail.LineDiscount = request.LineDiscount;
                creditSaleDetail.LineImposition = request.LineImposition;
                creditSaleDetail.LineTotalWithoutDiscountAndImposition = request.LineTotalWithoutDiscountAndImposition;
                creditSaleDetail.LineTotal = request.Units * request.UnitPrice - creditSaleDetail.LineDiscount +
                                             creditSaleDetail.LineImposition;
                creditSaleDetail.Status = SaleDetailStatus.Nothing;
            }

            creditSaleDetail.Rollbacked = request.Rollbacked;


            return creditSaleDetail;
        }

        private IEnumerable<CreditSaleDetail> PrepareCreditSaleDetails(IEnumerable<AddCreditSaleDetailRequest> requests,
            decimal cpercentage)
        {
            IList<CreditSaleDetail> response = new List<CreditSaleDetail>();

            if (requests != null && requests.Count() > 0)
                foreach (AddCreditSaleDetailRequest request in requests)
                {
                    response.Add(PrepareCreditSaleDetail(request, cpercentage));
                }

            return response;
        }

        #endregion

        #region PrepareUncreditSaleDetails

        private UncreditSaleDetail PrepareUnuncreditSaleDetail(AddUncreditSaleDetailRequest request, decimal upercentage)
        {
            UncreditSaleDetail uncreditSaleDetail = new UncreditSaleDetail();

            uncreditSaleDetail.ID = Guid.NewGuid();
            uncreditSaleDetail.CreateDate = PersianDateTime.Now;
            uncreditSaleDetail.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
            uncreditSaleDetail.UncreditService = _uncreditServiceRepository.FindBy(request.UncreditServiceID);
            
            uncreditSaleDetail.RowVersion = 1;
            if (request.MainUncreditSaleDetailID != Guid.Empty)
            {
                UncreditSaleDetail psd = _uncreditSaleDetailRepository.FindBy(request.MainUncreditSaleDetailID);

                uncreditSaleDetail.MainSaleDetail = psd;
                uncreditSaleDetail.SaleEmployee = psd.SaleEmployee;
                
            }
            uncreditSaleDetail.IsRollbackDetail = request.IsRollbackDetail;
            if (request.IsRollbackDetail)
            {

                uncreditSaleDetail.Discount = 0;
                // Convert.ToInt32(request.CanRollbackDiscountPrice / request.Units * upercentage) - (Convert.ToInt32(request.CanRollbackDiscountPrice / request.Units * upercentage) % 5000);
                uncreditSaleDetail.Imposition = 0;
                //Convert.ToInt32(request.CanRollbackImpositionPrice / request.Units * upercentage) - (Convert.ToInt32(request.CanRollbackImpositionPrice / request.Units * upercentage) % 5000);
                uncreditSaleDetail.Units = request.Units;
                uncreditSaleDetail.LineDiscount = Convert.ToInt32(request.CanRollbackDiscountPrice * upercentage) -
                                                  (Convert.ToInt32(request.CanRollbackDiscountPrice * upercentage) % 1);
                uncreditSaleDetail.LineImposition = Convert.ToInt32(request.CanRollbackImpositionPrice * upercentage) -
                                                    (Convert.ToInt32(request.CanRollbackImpositionPrice * upercentage) % 1);
                uncreditSaleDetail.LineTotal = request.RollbackPrice - uncreditSaleDetail.LineDiscount +
                                               uncreditSaleDetail.LineImposition;

                if (uncreditSaleDetail.MainSaleDetail.Delivered)
                {
                    uncreditSaleDetail.Comission =-
                        Convert.ToInt32(uncreditSaleDetail.MainSaleDetail.Comission * upercentage) -
                        (Convert.ToInt32(uncreditSaleDetail.MainSaleDetail.Comission * upercentage) % 1);

                }
                if (uncreditSaleDetail.MainSaleDetail.Sale.Closed)
                {
                    uncreditSaleDetail.Bonus =- Convert.ToInt32(uncreditSaleDetail.MainSaleDetail.Bonus * upercentage) -
                                          (Convert.ToInt32(uncreditSaleDetail.MainSaleDetail.Bonus * upercentage) % 1);
                    if (request.HasCourier)
                        uncreditSaleDetail.BonusDate = PersianDateTime.Now;
                }

                uncreditSaleDetail.RollbackNote = request.RollbackNote;
                uncreditSaleDetail.RollbackPrice = request.RollbackPrice;
                uncreditSaleDetail.Status = request.Status;

            }
            else
            {
                uncreditSaleDetail.Discount = request.Discount;
                uncreditSaleDetail.Imposition = request.Imposition;
                uncreditSaleDetail.UnitPrice = request.UnitPrice;
                uncreditSaleDetail.Units = request.Units;
                uncreditSaleDetail.LineDiscount = request.LineDiscount;
                uncreditSaleDetail.LineImposition = request.LineImposition;
                uncreditSaleDetail.LineTotalWithoutDiscountAndImposition = request.LineTotalWithoutDiscountAndImposition;
                uncreditSaleDetail.LineTotal = request.Units * request.UnitPrice - uncreditSaleDetail.LineDiscount +
                                               uncreditSaleDetail.LineImposition;
                uncreditSaleDetail.Status = SaleDetailStatus.Nothing;
                uncreditSaleDetail.SaleEmployee = uncreditSaleDetail.CreateEmployee;
            }

            uncreditSaleDetail.Rollbacked = request.Rollbacked;


            return uncreditSaleDetail;
        }

        private IEnumerable<UncreditSaleDetail> PrepareUnuncreditSaleDetails(
            IEnumerable<AddUncreditSaleDetailRequest> requests, decimal upercentage)
        {
            IList<UncreditSaleDetail> response = new List<UncreditSaleDetail>();

            if (requests != null && requests.Count() > 0)
                foreach (AddUncreditSaleDetailRequest request in requests)
                {
                    response.Add(PrepareUnuncreditSaleDetail(request, upercentage));
                }

            return response;
        }

        #endregion

        #endregion

        #region Edit

        public GeneralResponse EditSale(EditSaleRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            // چک کردن اینکه آیا در این مرحله امکان عملیات فاکتور وجود دارد یا خیر
            Customer customer = this._customerRepository.FindBy(request.CustomerID);
            Level level = _levelRepository.FindBy(customer.Level.ID);
            if (level.Options == null || !level.Options.CanSale)
            {
                response.ErrorMessages.Add("SaleIsNotPermitedInThisLevel");
                return response;
            }

            Sale sale = new Sale();
            sale = _saleRepository.FindBy(request.ID);

            if (sale != null)
            {
                try
                {
                    sale.ModifiedDate = PersianDateTime.Now;
                    sale.Customer = this._customerRepository.FindBy(request.CustomerID);
                    sale.SaleNumber = request.SaleNumber;

                    if (sale.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        sale.RowVersion += 1;
                    }

                    if (sale.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in sale.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _saleRepository.Save(sale);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                }
            }
            else
            {

                response.ErrorMessages.Add("NoItemToEditKey");
            }
            return response;
        }

        #endregion

        #region CloseSale

        public GeneralResponse CloseSale(CloseSaleRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            Sale sale = new Sale();
            sale = _saleRepository.FindBy(request.SaleID);

            // چک کردن اینکه آیا در این مرحله امکان عملیات فاکتور وجود دارد یا خیر
            Customer customer = sale.Customer;
            Level level = _levelRepository.FindBy(customer.Level.ID);
            if (level.Options == null || !level.Options.CanSale)
            {
                response.ErrorMessages.Add("SaleIsNotPermitedInThisLevel");
                return response;
            }
            sale.SaleNumber = CreateSaleNumber();
            Employee closeEmployee = new Employee();
            closeEmployee = _employeeRepository.FindBy(request.CloseEmployeeID);

            if (sale != null && closeEmployee != null)
            {
                try
                {

                    sale.Close(closeEmployee);

                    if (sale.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add(
                            "کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        sale.RowVersion += 1;
                    }

                    if (sale.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in sale.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }



                    //IList<BonusComission> bonusComissions = AddBonusAndComission(sale);


                    #region افزودن پورسانت و امتیاز

                    foreach (var item in sale.CreditSaleDetails)
                    {
                       
                        long comission = (item.Units*item.CreditService.Comission) - (item.LineDiscount / 10);
                        item.UnDeliveredComission = comission < 0 ? 0 : comission;
                        long bonus = (item.Units*item.CreditService.Bonus) - (item.LineDiscount / 10000);
                        if (item.CreditService.Bonus < 0)
                            item.Bonus =item.Units* bonus;
                        else
                        item.Bonus = bonus < 0 ? 0 : bonus;
                        item.SaleEmployee = item.CreateEmployee;
                    }
                    foreach (var item in sale.UncreditSaleDetails)
                    {
                        long comission = (item.Units*item.UncreditService.Comission) - (item.LineDiscount / 10);
                        item.UnDeliveredComission = comission < 0 ? 0 : comission;
                        
                        long bonus = (item.Units*item.UncreditService.Bonus) - (item.LineDiscount / 10000);
                        if (item.UncreditService.Bonus < 0)
                            item.Bonus = item.Units * bonus;
                        else
                        item.Bonus = bonus<0?0:bonus;
                        item.SaleEmployee = item.CreateEmployee;
                    }
                    foreach (var item in sale.ProductSaleDetails)
                    {
                        long comission = (item.Units*item.ProductPrice.Comission) - (item.LineDiscount/10);
                        item.UnDeliveredComission = comission < 0 ? 0 : comission;
                        long bonus = (item.Units*item.ProductPrice.Bonus) - (item.LineDiscount / 10000);
                        if (item.ProductPrice.Bonus < 0)
                            item.Bonus =item.Units* bonus;
                        else
                        item.Bonus = bonus < 0 ? 0 : bonus;
                        item.SaleEmployee = item.CreateEmployee;
                    }

                    #endregion

                    if (response.ErrorMessages.Count() > 0)
                    {
                        return response;
                    }
                    _saleRepository.Save(sale);
                    _customerRepository.Save(customer);

                    //foreach (var bonusComission in bonusComissions)
                    //{
                    //    _bonusComissionRepository.Add(bonusComission);
                    //}
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                }
            }
            else
            {

                response.ErrorMessages.Add("هیچ موردی جهت ویرایش وجود ندارد.");
            }
            return response;

        }

        #endregion

        #region Deliver

        public GeneralResponse SaleDetail_Deliver(DeliverRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            Customer customer = new Customer();
            Employee deliverEmployee = new Employee();
            deliverEmployee = _employeeRepository.FindBy(request.DeliverEmployeeID);

            // If Product
            if (request.DetailType == 'P')
            {
                ProductSaleDetail productSaleDetail = _productSaleDetailRepository.FindBy(request.SaleDetailID);

                
                customer = _customerRepository.FindBy(productSaleDetail.Sale.Customer.ID);

                if (customer.CanDeliverCost >= productSaleDetail.LineTotal)
                {
                    productSaleDetail.Deliver(deliverEmployee, request.DeliverNote);
                    customer.CanDeliverCost -= productSaleDetail.LineTotal;
                }
                else
                {
                    response.ErrorMessages.Add("BalanceNotEnoughAndCanNotDeliver");
                    return response;
                }

                #region Validate

                if (productSaleDetail.GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in productSaleDetail.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                productSaleDetail.Comission = productSaleDetail.UnDeliveredComission;
                productSaleDetail.ComissionDate = PersianDateTime.Now;
                _productSaleDetailRepository.Save(productSaleDetail);
                
            }

                // If Uncredit Service
            else if (request.DetailType == 'U')
            {
                UncreditSaleDetail uncreditSaleDetail = _uncreditSaleDetailRepository.FindBy(request.SaleDetailID);
                customer = _customerRepository.FindBy(uncreditSaleDetail.Sale.Customer.ID);
                // آیا مشتری مورد نظر به اندازه تحویل این آیتم بستانکار است؟
                if (customer.CanDeliverCost >= uncreditSaleDetail.LineTotal)
                {
                    uncreditSaleDetail.Deliver(deliverEmployee, request.DeliverNote);
                    
                    customer.CanDeliverCost -= uncreditSaleDetail.LineTotal;
                }
                else
                {
                    response.ErrorMessages.Add("BalanceNotEnoughAndCanNotDeliver");
                    return response;
                }

                #region Validate

                if (uncreditSaleDetail.GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in uncreditSaleDetail.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                uncreditSaleDetail.Comission = uncreditSaleDetail.UnDeliveredComission;
                uncreditSaleDetail.ComissionDate = PersianDateTime.Now;

                _uncreditSaleDetailRepository.Save(uncreditSaleDetail);

            }

                // If Credit Service
            else if (request.DetailType == 'C')
            {
                CreditSaleDetail creditSaleDetail = _creditSaleDetailRepository.FindBy(request.SaleDetailID);
                customer = _customerRepository.FindBy(creditSaleDetail.Sale.Customer.ID);
                // آیا مشتری مورد نظر به اندازه تحویل این آیتم بستانکار است؟
                if (customer.CanDeliverCost >= creditSaleDetail.LineTotal)
                {
                    creditSaleDetail.Deliver(deliverEmployee, request.DeliverNote);
                    customer.CanDeliverCost -= creditSaleDetail.LineTotal;
                }
                else
                {
                    response.ErrorMessages.Add("BalanceNotEnoughAndCanNotDeliver");
                    return response;
                }

                #region Validate

                if (creditSaleDetail.GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in creditSaleDetail.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                creditSaleDetail.Comission = creditSaleDetail.UnDeliveredComission;
                creditSaleDetail.ComissionDate = PersianDateTime.Now;

                _creditSaleDetailRepository.Save(creditSaleDetail);

            }

            if (customer.CanDeliverCost < 0)
            {
                response.ErrorMessages.Add(
                    " هشدار ! با انجام این عملیات معین تحویل مشتری منفی میشود. لطفا با برنامه نویس تماس بگیرید");
                return response;
            }

            if (response.success == true)
                _uow.Commit();

            return response;
        }

        /// <summary>
        /// مبلغ قابل تحویل
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public long DeliverableCost(Guid customerID)
        {
            // calculate sum of fiscals:
            Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
            Criterion criteria = new Criterion("Customer.ID", customerID, CriteriaOperator.Equal);
            query.Add(criteria);

            Response<Fiscal> fiscalsResponse = _fiscalRepository.FindBy(query, -1, -1);
            long sumFiscalCost = fiscalsResponse.data.Sum(s => s.ConfirmedCost.HasValue ? (long)s.ConfirmedCost : 0);

            // calculate sum of delivered sales:
            Response<Sale> salesResponse = _saleRepository.FindBy(query, -1, -1);
            long sumDeliveredCost = salesResponse.data.Sum(s => (long)s.SumCostOfDeliveredItems);

            return sumFiscalCost - sumDeliveredCost;
        }

        #endregion

        #region Delete

        #region DeleteSale

        public GeneralResponse DeleteSale(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            Sale sale = new Sale();
            sale = _saleRepository.FindBy(request.ID);

            if (sale != null)
            {
                try
                {
                    if (sale.Closed)
                    {
                        response.ErrorMessages.Add("فاکتور تایید شده قابل حذف نمی باشد");
                        return response;
                    }
                    _saleRepository.Remove(sale);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                }
            }

            return response;
        }

        #endregion

        public GeneralResponse DeleteSaleDetail(DeleteSaleDetailRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (DeleteRequest deleteRequest in request.deleteProductSaleDetailRequests)
                {
                    GeneralResponse subResponse = DeleteProductSaleDetail(deleteRequest);
                    foreach (string err in subResponse.ErrorMessages)
                        response.ErrorMessages.Add(err);
                }

                foreach (DeleteRequest deleteRequest in request.deleteCreditSaleDetailRequests)
                {
                    GeneralResponse subResponse = DeleteCreditSaleDetail(deleteRequest);
                    foreach (string err in subResponse.ErrorMessages)
                        response.ErrorMessages.Add(err);
                }

                foreach (DeleteRequest deleteRequest in request.deleteUncreditSaleDetailRequests)
                {
                    GeneralResponse subResponse = DeleteUncreditSaleDetail(deleteRequest);
                    foreach (string err in subResponse.ErrorMessages)
                        response.ErrorMessages.Add(err);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return response;
            }

            _uow.Commit();

            return response;
        }

        #region Delete ProductSaleDetail

        public GeneralResponse DeleteProductSaleDetail(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            ProductSaleDetail productSaleDetail = new ProductSaleDetail();
            productSaleDetail = _productSaleDetailRepository.FindBy(request.ID);

            Sale sale = productSaleDetail.Sale;

            if (productSaleDetail != null)
            {
                try
                {
                    sale.DeleteSaleDetail(productSaleDetail);

                    #region Validation

                    if (sale.GetBrokenRules().Count() > 0)
                    {


                        foreach (BusinessRule businessRule in sale.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _productSaleDetailRepository.Remove(productSaleDetail);

                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                }
            }

            return response;
        }

        #endregion

        #region Delete CreditSaleDetail

        public GeneralResponse DeleteCreditSaleDetail(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            CreditSaleDetail creditSaleDetail = new CreditSaleDetail();
            creditSaleDetail = _creditSaleDetailRepository.FindBy(request.ID);

            Sale sale = creditSaleDetail.Sale;

            if (creditSaleDetail != null)
            {
                try
                {
                    sale.DeleteSaleDetail(creditSaleDetail);

                    #region Validation

                    if (sale.GetBrokenRules().Count() > 0)
                    {


                        foreach (BusinessRule businessRule in sale.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _creditSaleDetailRepository.Remove(creditSaleDetail);

                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                }
            }

            return response;
        }

        #endregion

        #region Delete UncreditSaleDetail

        public GeneralResponse DeleteUncreditSaleDetail(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            UncreditSaleDetail uncreditSaleDetail = new UncreditSaleDetail();
            uncreditSaleDetail = _uncreditSaleDetailRepository.FindBy(request.ID);

            Sale sale = uncreditSaleDetail.Sale;

            if (uncreditSaleDetail != null)
            {
                try
                {
                    sale.DeleteSaleDetail(uncreditSaleDetail);

                    #region Validation

                    if (sale.GetBrokenRules().Count() > 0)
                    {


                        foreach (BusinessRule businessRule in sale.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _uncreditSaleDetailRepository.Remove(uncreditSaleDetail);

                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                }
            }

            return response;
        }

        #endregion

        #endregion

        #region Get One

        public GetSaleResponse GetSale(GetRequest request)
        {
            GetSaleResponse response = new GetSaleResponse();

            try
            {
                Sale sale = new Sale();
                SaleView saleView = sale.ConvertToSaleView();

                sale = _saleRepository.FindBy(request.ID);
                if (sale != null)
                    saleView = sale.ConvertToSaleView();

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("Sale.ID", sale.ID, CriteriaOperator.Equal);
                query.Add(criteria);
                IEnumerable<UncreditSaleDetailView> uncreditSaleDetailViews =
                    _uncreditSaleDetailRepository.FindByQuery(query).data.ConvertToUncreditSaleDetailViews();
                IEnumerable<CreditSaleDetailView> creditSaleDetailViews =
                    _creditSaleDetailRepository.FindByQuery(query).data.ConvertToCreditSaleDetailViews();
                IEnumerable<ProductSaleDetailView> productSaleDetailViews =
                    _productSaleDetailRepository.FindByQuery(query).data.ConvertToProductSaleDetailViews();


                foreach (var item in uncreditSaleDetailViews)
                {
                    Infrastructure.Querying.Query querying = new Infrastructure.Querying.Query();
                    Criterion crt = new Criterion("MainSaleDetail.ID", item.ID, CriteriaOperator.Equal);
                    querying.Add(crt);

                    UncreditSaleDetailView Temp = _uncreditSaleDetailRepository.FindByQuery(querying)
                        .data.FirstOrDefault()
                        .ConvertToUncreditSaleDetailView();
                    if (Temp != null)
                    {
                        item.RollbackEmployeeName = Temp.CreateEmployeeName;
                        item.RollbackDate = Temp.RollbackDate;
                    }

                }

                foreach (var item in creditSaleDetailViews)
                {
                    Infrastructure.Querying.Query querying = new Infrastructure.Querying.Query();
                    Criterion crt = new Criterion("MainSaleDetail.ID", item.ID, CriteriaOperator.Equal);
                    querying.Add(crt);

                    CreditSaleDetailView Temp = _creditSaleDetailRepository.FindByQuery(querying)
                        .data.FirstOrDefault()
                        .ConvertToCreditSaleDetailView();
                    if (Temp != null)
                    {
                        item.RollbackEmployeeName = Temp.CreateEmployeeName;
                        item.RollbackDate = Temp.CreateDate;
                    }

                }

                foreach (var item in productSaleDetailViews)
                {
                    Infrastructure.Querying.Query querying = new Infrastructure.Querying.Query();
                    Criterion crt = new Criterion("MainSaleDetail.ID", item.ID, CriteriaOperator.Equal);
                    querying.Add(crt);

                    ProductSaleDetailView Temp =
                        _productSaleDetailRepository.FindByQuery(querying)
                            .data.FirstOrDefault()
                            .ConvertToProductSaleDetailView();
                    if (Temp != null)
                    {
                        item.RollbackEmployeeName = Temp.CreateEmployeeName;
                        item.RollbackDate = Temp.CreateDate;

                    }
                }


                saleView.UncreditSaleDetails = uncreditSaleDetailViews;
                saleView.CreditSaleDetails = creditSaleDetailViews;
                saleView.ProductSaleDetails = productSaleDetailViews;

                response.SaleView = saleView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetGeneralResponse<SaleView> GetSale(Guid saleID)
        {
            GetGeneralResponse<SaleView> response = new GetGeneralResponse<SaleView>();

            try
            {
                Sale sale = new Sale();
                SaleView saleView = sale.ConvertToSaleView();

                sale = _saleRepository.FindBy(saleID);
                if (sale != null)
                    saleView = sale.ConvertToSaleView();

                response.data = saleView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        #endregion

        #region Get All

        public GetSalesResponse GetSales()
        {
            GetSalesResponse response = new GetSalesResponse();

            try
            {
                IEnumerable<SaleView> sales = _saleRepository.FindAll()
                    .ConvertToSaleViews();

                response.SaleViews = sales;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        #region Get Simple Sale

        public GetSalesResponse SimpleGetSales(AjaxGetRequest request, IList<Sort> sort, IList<FilterData> filter,
            bool ForReport)
        {
            GetSalesResponse response = new GetSalesResponse();
            Response<Sale> sale = new Response<Sale>();
            int index = (request.PageNumber - 1) * request.PageSize;
            int count = request.PageSize;

            if (request.ID != Guid.Empty || filter.Count > 0)
            {

                string _query = FilterUtilityService.GenerateFilterHQLQuery(filter, "Sale", sort);

                sale = _saleRepository.FindAll(_query, index, count);
            }

            response.SimpleSaleViews = sale.data.ConvertToSimpleSaleViews();


            #region Preparing Courier
            Courier courier = new Courier();
            foreach (var item in response.SimpleSaleViews)
            {
                IList<ClientSaleDetailView> details = new List<ClientSaleDetailView>();
                Infrastructure.Querying.Query query1 = new Infrastructure.Querying.Query();
                Criterion tempq = new Criterion("Sale.ID", item.ID, CriteriaOperator.Equal);
                query1.Add(tempq);

                courier = _courierRepository.FindBy(query1).FirstOrDefault();
                item.Courier = courier.ConvertToCourierView();

                #region Temp

                //foreach (var _i_tem in sale.data)
                //{

                //    if (item.ID == _i_tem.ID)
                //    {

                //        #region Products

                //        if (_i_tem.ProductSaleDetails != null)
                //            foreach (ProductSaleDetail productSaleDetail in _i_tem.ProductSaleDetails)
                //            {
                //                ClientSaleDetailView temp = new ClientSaleDetailView();
                //                temp.SaleDetailID = productSaleDetail.ID;
                //                temp.SaleDetailName = productSaleDetail.ProductPrice.ProductPriceTitle;
                //                temp.Imposition = productSaleDetail.Imposition;
                //                temp.MaxDiscount = productSaleDetail.ProductPrice.MaxDiscount;
                //                temp.UnitPrice = productSaleDetail.UnitPrice;
                //                temp.Units = productSaleDetail.Units;
                //                temp.RowID = productSaleDetail.ID;
                //                temp.RowVersion = productSaleDetail.RowVersion;
                //                temp.Discount = productSaleDetail.Discount;
                //                temp.CanDeliver = productSaleDetail.CanDeliver;
                //                temp.RollbackPrice = productSaleDetail.RollbackPrice;
                //                if (productSaleDetail.Rollbacked)
                //                {
                //                    temp.RollbackEmployeeName = productSaleDetail.RollbackedProductSaleDetail.CreateEmployee.Name;
                //                    temp.RollbackDate = productSaleDetail.RollbackedProductSaleDetail.CreateDate;
                //                }
                //                if (productSaleDetail.Delivered)
                //                {
                //                    temp.DeliverDate = productSaleDetail.DeliverDate;
                //                    temp.DeliverEmployeeName = productSaleDetail.DeliverEmployee.Name;
                //                }
                //                details.Add(temp);
                //            }

                //        #endregion

                //        #region Credits

                //        if (_i_tem.CreditSaleDetails != null)
                //            foreach (CreditSaleDetail creditSaleDetail in _i_tem.CreditSaleDetails)
                //            {
                //                ClientSaleDetailView temp = new ClientSaleDetailView();
                //                temp.SaleDetailID = creditSaleDetail.ID;
                //                temp.SaleDetailName = creditSaleDetail.CreditService.ServiceName;
                //                temp.Imposition = creditSaleDetail.Imposition;
                //                temp.MaxDiscount = creditSaleDetail.CreditService.MaxDiscount;
                //                temp.UnitPrice = creditSaleDetail.UnitPrice;
                //                temp.Units = creditSaleDetail.Units;
                //                temp.RowID = creditSaleDetail.ID;
                //                temp.RowVersion = creditSaleDetail.RowVersion;
                //                temp.Discount = creditSaleDetail.Discount;
                //                temp.CanDeliver = creditSaleDetail.CanDeliver;
                //                temp.RollbackPrice = creditSaleDetail.RollbackPrice;
                //                if (creditSaleDetail.Rollbacked)
                //                {
                //                    temp.RollbackEmployeeName = creditSaleDetail.RollbackedCreditSaleDetail.CreateEmployee.Name;
                //                    temp.RollbackDate = creditSaleDetail.RollbackedCreditSaleDetail.CreateDate;
                //                }
                //                if (creditSaleDetail.Delivered)
                //                {
                //                    temp.DeliverDate = creditSaleDetail.DeliverDate;
                //                    temp.DeliverEmployeeName = creditSaleDetail.DeliverEmployee.Name;
                //                }
                //                details.Add(temp);
                //            }

                //        #endregion

                //#region Uncredits

                //if (getSaleResponse.SaleView.UncreditSaleDetails != null)
                //    foreach (UncreditSaleDetailView UncreditSaleDetail in getSaleResponse.SaleView.UncreditSaleDetails)
                //    {
                //        ClientSaleDetailViewModel item = new ClientSaleDetailViewModel();
                //        item.SaleDetailID = UncreditSaleDetail.UncreditServiceID;
                //        item.SaleDetailName = UncreditSaleDetail.UncreditServiceName;
                //        item.Imposition = UncreditSaleDetail.Imposition;
                //        item.MaxDiscount = GetProduction(UncreditSaleDetail.UncreditServiceID).MaxDiscount;
                //        item.UnitPrice = UncreditSaleDetail.UnitPrice;
                //        item.Units = UncreditSaleDetail.Units;
                //        item.RowID = UncreditSaleDetail.ID;
                //        item.RowVersion = UncreditSaleDetail.RowVersion;
                //        item.Discount = UncreditSaleDetail.Discount;
                //        item.CanDeliver = UncreditSaleDetail.CanDeliver;
                //        item.RollbackPrice = UncreditSaleDetail.RollbackPrice;
                //        if (UncreditSaleDetail.Rollbacked)
                //        {
                //            item.RollbackEmployeeName = UncreditSaleDetail.RollbackEmployeeName;
                //            item.RollbackDate = UncreditSaleDetail.RollbackDate;
                //        }
                //        if (UncreditSaleDetail.Delivered)
                //        {
                //            item.DeliverDate = UncreditSaleDetail.DeliverDate;
                //            item.DeliverEmployeeName = UncreditSaleDetail.DeliverEmployeeName;
                //        }
                //        details.Add(item);

                //        //details.Add(new ClientSaleDetailViewModel
                //        //{
                //        //    SaleDetailID = UncreditSaleDetail.UncreditServiceID,
                //        //    SaleDetailName = UncreditSaleDetail.UncreditServiceName,
                //        //    Imposition = UncreditSaleDetail.Imposition,
                //        //    MaxDiscount = GetProduction(UncreditSaleDetail.UncreditServiceID).MaxDiscount,
                //        //    UnitPrice = UncreditSaleDetail.UnitPrice,
                //        //    Units = UncreditSaleDetail.Units,
                //        //    RowID = UncreditSaleDetail.ID,
                //        //    RowVersion = UncreditSaleDetail.RowVersion,
                //        //    Discount = UncreditSaleDetail.Discount,
                //        //    CanDeliver = UncreditSaleDetail.CanDeliver                               
                //        //});
                //    }

                //#endregion

                //    }

                //}
                //item.SaleDetails = details;

                #endregion
            }

            #endregion

            return response;

        }
        #endregion

        public GetSalesResponse GetSales(AjaxGetRequest request, IList<Sort> sort, IList<FilterData> filter, bool ForReport)
        {
            GetSalesResponse response = new GetSalesResponse();

            try
            {
                Response<Sale> sale = new Response<Sale>();
                int index = (request.PageNumber - 1) * request.PageSize;
                int count = request.PageSize;
                if (ForReport)
                {
                    string UncreditQuery = FilterUtilityService.GenerateFilterHQLQuery(filter, "UncreditSaleDetail",
                        null);
                    Response<UncreditSaleDetail> unCreditSaleDetails =
                        _uncreditSaleDetailRepository.FindAll(UncreditQuery);

                    string CreditQuery = FilterUtilityService.GenerateFilterHQLQuery(filter, "CreditSaleDetail",
                        null);
                    Response<CreditSaleDetail> creditSaleDetails =
                        _creditSaleDetailRepository.FindAll(CreditQuery);

                    string ProductQuery = FilterUtilityService.GenerateFilterHQLQuery(filter, "ProductSaleDetail",
                        null);
                    Response<ProductSaleDetail> productSaleDetail =
                        _productSaleDetailRepository.FindAll(ProductQuery);

                    IList<FilterData> SaleFilter = new List<FilterData>();
                    IList<Guid> SaleIDs = new List<Guid>();

                    if (unCreditSaleDetails.totalCount > 0)
                        foreach (var item in unCreditSaleDetails.data)
                            SaleIDs.Add(item.Sale.ID);

                    if (creditSaleDetails.totalCount > 0)
                        foreach (var item in creditSaleDetails.data)
                            SaleIDs.Add(item.Sale.ID);

                    if (productSaleDetail.totalCount > 0)
                        foreach (var item in productSaleDetail.data)
                            SaleIDs.Add(item.Sale.ID);

                    var FinalSaleIDs = SaleIDs.Distinct();


                    IList<string> Ids = new List<string>();
                    foreach (var finalSaleID in FinalSaleIDs)
                    {
                        Ids.Add(finalSaleID.ToString());
                    }
                    SaleFilter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            type = "list",
                            value = Ids.ToArray()
                        },
                        field = "ID"
                    });



                    string _query = FilterUtilityService.GenerateFilterHQLQuery(SaleFilter, "Sale", sort);

                    sale = _saleRepository.FindAll(_query, index, count);

                }
                else
                {
                    if (request.ID != Guid.Empty || filter.Count > 0)
                    {

                        string _query = FilterUtilityService.GenerateFilterHQLQuery(filter, "Sale", sort);

                        sale = _saleRepository.FindAll(_query);
                    }
                    else
                    {
                        string _query = FilterUtilityService.GenerateFilterHQLQuery(filter, "Sale", sort);
                        sale = _saleRepository.FindAll(index, count);
                    }
                }

                #region Preparing Courier

                Courier courier = new Courier();
                foreach (var item in sale.data)
                {

                    //Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                    //Criterion temp = new Criterion("Sale.ID", item.ID, CriteriaOperator.Equal);
                    //query.Add(temp);

                    //courier = _courierRepository.FindBy(query).FirstOrDefault();

                    //item.Couriers = courier;
                }

                #endregion

                IEnumerable<SaleView> sales = sale.data.ConvertToSaleViews();
                foreach (var item in sales)
                {
                    Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                    Criterion criteria = new Criterion("Sale.ID", item.ID, CriteriaOperator.Equal);
                    query.Add(criteria);
                    IEnumerable<UncreditSaleDetailView> uncreditSaleDetailViews =
                        _uncreditSaleDetailRepository.FindByQuery(query).data.ConvertToUncreditSaleDetailViews();
                    IEnumerable<CreditSaleDetailView> creditSaleDetailViews =
                        _creditSaleDetailRepository.FindByQuery(query).data.ConvertToCreditSaleDetailViews();
                    IEnumerable<ProductSaleDetailView> productSaleDetailViews =
                        _productSaleDetailRepository.FindByQuery(query).data.ConvertToProductSaleDetailViews();

                    #region Preparing Courier

                    Infrastructure.Querying.Query query1 = new Infrastructure.Querying.Query();
                    Criterion temp = new Criterion("Sale.ID", item.ID, CriteriaOperator.Equal);
                    query1.Add(temp);

                    courier = _courierRepository.FindBy(query1).FirstOrDefault();

                    item.Courier = courier.ConvertToCourierView();

                    #endregion

                    item.UncreditSaleDetails = uncreditSaleDetailViews;
                    item.CreditSaleDetails = creditSaleDetailViews;
                    item.ProductSaleDetails = productSaleDetailViews;
                }

                response.SaleViews = sales;
                response.TotalCount = sale.totalCount;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<SaleView>> GetUnClosedSales(int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<SaleView>> response = new GetGeneralResponse<IEnumerable<SaleView>>();
            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion CloseCriteria = new Criterion("Closed", true, CriteriaOperator.Equal);
                query.Add(CloseCriteria);

                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<Sale> sales = _saleRepository.FindBy(query, index, count);

                response.data = sales.data.ConvertToSaleViews();
                response.totalCount = sales.totalCount;

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Get Un delivered Products for Customer

        public GetGeneralResponse<IEnumerable<ProductSaleDetailView>> GetUnDeliveredProducts(Guid CustomerID)
        {
            GetGeneralResponse<IEnumerable<ProductSaleDetailView>> response =
                new GetGeneralResponse<IEnumerable<ProductSaleDetailView>>();

            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion CustomerIdCriteria = new Criterion("Customer.ID", CustomerID, CriteriaOperator.Equal);
                query.Add(CustomerIdCriteria);

                IEnumerable<Sale> sales = _saleRepository.FindBy(query);

                IList<ProductSaleDetailView> productSaleDetialViews = new List<ProductSaleDetailView>();

                foreach (var item in sales)
                {
                    foreach (var _item in item.ProductSaleDetails)
                    {
                        if (_item.Delivered != true && _item.IsRollbackDetail != true)
                            productSaleDetialViews.Add(_item.ConvertToProductSaleDetailView());
                    }
                }
                response.data = productSaleDetialViews;
                response.totalCount = productSaleDetialViews.Count();
            }

            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }


            return response;
        }

        #endregion

        #region Add Bonus and Comission

        public IList<BonusComission> AddBonusAndComission(Sale sale)
        {

            #region Getting Sale Details

            //Product Sale Detail
            Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

            Criterion SaleIDCriteria = new Criterion("Sale.ID", sale.ID, CriteriaOperator.Equal);
            query.Add(SaleIDCriteria);

            ProductSaleDetail productSaleDetail = _productSaleDetailRepository.FindBy(query).FirstOrDefault();
            CreditSaleDetail creditSaleDetail = _creditSaleDetailRepository.FindBy(query).FirstOrDefault();
            UncreditSaleDetail uncreditSaleDetail = _uncreditSaleDetailRepository.FindBy(query).FirstOrDefault();

            #endregion

            IList<BonusComission> bonusComissions = new List<BonusComission>();

            if (creditSaleDetail != null)
            {
                BonusComission bonusComission = new BonusComission();
                if (creditSaleDetail.LineDiscount > 0 && creditSaleDetail.CreditService.Bonus > 0)
                    bonusComission.Bonus = (creditSaleDetail.CreditService.Bonus * creditSaleDetail.Units) - (creditSaleDetail.LineDiscount / 10000);
                else
                    bonusComission.Bonus = creditSaleDetail.CreditService.Bonus * creditSaleDetail.Units;
                bonusComission.UnDeliveredComission = creditSaleDetail.CreditService.Comission;
                bonusComission.Comission = 0;
                bonusComission.Courier = null;
                bonusComission.CreateDate = PersianDateTime.Now;
                bonusComission.CreateEmployee = sale.CreateEmployee;
                bonusComission.CreditSaleDetail = creditSaleDetail;
                bonusComission.ProductSaleDetail = null;
                bonusComission.Customer = sale.Customer;
                bonusComission.ID = Guid.NewGuid();
                bonusComission.RowVersion = 1;
                bonusComission.UnCreditSaleDetail = null;

                bonusComissions.Add(bonusComission);
                creditSaleDetail.BonusComission = bonusComission;
                _creditSaleDetailRepository.Save(creditSaleDetail);



            }

            if (uncreditSaleDetail != null)
            {
                BonusComission bonusComission = new BonusComission();
                if (uncreditSaleDetail.LineDiscount > 0 && uncreditSaleDetail.UncreditService.Bonus > 0)
                    bonusComission.Bonus = (uncreditSaleDetail.UncreditService.Bonus * uncreditSaleDetail.Units) -
                                           (uncreditSaleDetail.LineDiscount / 10000);
                else
                    bonusComission.Bonus = uncreditSaleDetail.UncreditService.Bonus * uncreditSaleDetail.Units;

                bonusComission.UnDeliveredComission = uncreditSaleDetail.UncreditService.Comission;
                bonusComission.Comission = 0;
                bonusComission.Courier = null;
                bonusComission.CreateDate = PersianDateTime.Now;
                bonusComission.CreateEmployee = sale.CreateEmployee;
                bonusComission.CreditSaleDetail = null;
                bonusComission.ProductSaleDetail = null;
                bonusComission.Customer = sale.Customer;
                bonusComission.ID = Guid.NewGuid();
                bonusComission.RowVersion = 1;
                bonusComission.UnCreditSaleDetail = uncreditSaleDetail;

                bonusComissions.Add(bonusComission);
                uncreditSaleDetail.BonusComission = bonusComission;
                _uncreditSaleDetailRepository.Save(uncreditSaleDetail);
            }
            if (productSaleDetail != null)
            {
                BonusComission bonusComission = new BonusComission();
                if (productSaleDetail.LineDiscount > 0 && productSaleDetail.ProductPrice.Bonus > 0)
                    bonusComission.Bonus = (productSaleDetail.ProductPrice.Bonus * productSaleDetail.Units) - (productSaleDetail.LineDiscount / 10000);
                else
                    bonusComission.Bonus = productSaleDetail.ProductPrice.Bonus * productSaleDetail.Units;
                bonusComission.UnDeliveredComission = productSaleDetail.ProductPrice.Comission;
                bonusComission.Comission = 0;
                bonusComission.Courier = null;
                bonusComission.CreateDate = PersianDateTime.Now;
                bonusComission.CreateEmployee = sale.CreateEmployee;
                bonusComission.CreditSaleDetail = null;
                bonusComission.ProductSaleDetail = productSaleDetail;
                bonusComission.Customer = sale.Customer;
                bonusComission.ID = Guid.NewGuid();
                bonusComission.RowVersion = 1;
                bonusComission.UnCreditSaleDetail = null;

                bonusComissions.Add(bonusComission);

                productSaleDetail.BonusComission = bonusComission;
                _productSaleDetailRepository.Save(productSaleDetail);

            }

            return bonusComissions;
        }

        #endregion

        
        #region Get CandDeliverCost

        public GetGeneralResponse<IEnumerable<CanDeliverCostView>> GetSaleCanDeliverCost(string StartDate,string EndDate)
        {
            var response=new GetGeneralResponse<IEnumerable<CanDeliverCostView>>();
            IList<CanDeliverCostView> list=new List<CanDeliverCostView>();


            IList<string> lastQueries=new List<string>();
            lastQueries.Add("Select -Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=False And M.Delivered=True And M.Delivered=True and M.DeliverDate <'" + StartDate + " 00:00:00'");
            lastQueries.Add("Select -Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=False And M.Delivered=True And M.Delivered=True and M.DeliverDate <'" + StartDate + " 00:00:00'");
            lastQueries.Add("Select -Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=False And M.Delivered=True And M.Delivered=True and M.DeliverDate <'" + StartDate + " 00:00:00'");

            lastQueries.Add("Select Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=True And Status=3 And M.Sale.Closed=True and Sale.CloseDate <'" + StartDate + " 00:00:00'");
            lastQueries.Add("Select Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=True And Status=3 And M.Sale.Closed=True and Sale.CloseDate <'" + StartDate + " 00:00:00'");
            lastQueries.Add("Select Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=True And Status=3 And M.Sale.Closed=True and Sale.CloseDate <'" + StartDate + " 00:00:00'");

            lastQueries.Add("Select Sum(M.ConfirmedCost) From Fiscal M where M.Confirm=2 And M.InvestDate <'" + StartDate + "'");

            var LastDomain = _creditSaleDetailRepository.FindSumOfLineTotal(lastQueries);

            list.Add(new CanDeliverCostView()
            {
                Domain = "مانده قبل",
                Bes = LastDomain < 0 ? 0 : LastDomain,
                Bed = LastDomain > 0 ? 0 : LastDomain,
                Type = "تراکنش"
            });

            int start = Convert.ToInt32(StartDate.Substring(5, 2));
            int end = Convert.ToInt32(EndDate.Substring(5, 2));

            IList<string> CurrentQueries = new List<string>();
            bool isStart = new bool();
            for (int i = start; i <= end; i++)
            {
                
                string EndS = i > 9 ? i.ToString() : "0" + i.ToString();
                string EndT = i > 9 ? (i).ToString() : "0" + i.ToString();
                if (i != end)
                {

                    if (!isStart)
                    {
                        #region تحویل شده ها

                        CurrentQueries.Add(
                            "Select -Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=False And M.Delivered=True And M.Sale.Closed=True and M.DeliverDate >'" +
                            StartDate + " 00:00:00' And M. DeliverDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurSaleCre = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات عتباری",
                            SaleType = "تحویل شده ها",
                            Bed = CurSaleCre,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select -Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=False And M.Delivered=True And M.Sale.Closed=True and M.DeliverDate >'" +
                            StartDate + " 00:00:00' And  M.DeliverDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurSaleUnc = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات غیر اعتباری",
                            SaleType = "تحویل شده ها",
                            Bed = CurSaleUnc,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select -Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=False And M.Delivered=True And M.Sale.Closed=True and M.DeliverDate >'" +
                            StartDate + " 00:00:00' And  M.DeliverDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurSalePro = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "محصولات",
                            SaleType = "تحویل شده ها",
                            Bed = CurSalePro,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();

                        #endregion

                        #region برگشت شده ها

                        CurrentQueries.Add(
                            "Select Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=True And M.Status=3 And M.Sale.Closed=True and Sale.CloseDate >'" +
                            StartDate + " 00:00:00' And  Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurRolCre = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات اعتباری",
                            SaleType = "برگشت شده ها",
                            Bes = CurRolCre,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=True And M.Status=3 And M.Sale.Closed=True and Sale.CloseDate >'" +
                            StartDate + " 00:00:00' And  Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurRolUnc = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات غیر اعتباری",
                            SaleType = "برگشت شده ها",
                            Bes = CurRolUnc,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=True And M.Status=3 And M.Sale.Closed=True and Sale.CloseDate >'" +
                            StartDate + " 00:00:00' And  Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurRolPro = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "محصولات",
                            SaleType = "برگشت شده ها",
                            Bes = CurRolPro,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();

                        #endregion

                        #region مالی ها

                        CurrentQueries.Add(
                            "Select Sum(M.ConfirmedCost) From Fiscal M where M.Confirm=2 and ConfirmedCost>0 And M.InvestDate >='" +
                            StartDate + "' And  M.InvestDate<='" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31'");
                        var CurFisPay = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "دریافت",
                            SaleType = "مالی",
                            Bes = CurFisPay,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select Sum(M.ConfirmedCost) From Fiscal M where M.Confirm=2 and ConfirmedCost<0 And M.InvestDate >='" +
                            StartDate + "' And  M.InvestDate<='" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31'");
                        var CurFisRec = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "پرداخت",
                            SaleType = "مالی",
                            Bed = CurFisRec,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();

                        #endregion

                        isStart = true;

                    }
                    else
                    {

                        #region تحویل شده ها

                        CurrentQueries.Add(
                            "Select -Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=False And M.Delivered=True And M.Sale.Closed=True and M.DeliverDate >'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/01 00:00:00' And  M.DeliverDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurSaleCre = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات عتباری",
                            SaleType = "تحویل شده ها",
                            Bed = CurSaleCre,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();
                        CurrentQueries.Add(
                            "Select -Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=False And M.Delivered=True And M.Sale.Closed=True and M.DeliverDate >'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/01 00:00:00' And  M.DeliverDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurSaleUnc = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات غیر اعتباری",
                            SaleType = "تحویل شده ها",
                            Bed = CurSaleUnc,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select -Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=False And M.Delivered=True And M.Sale.Closed=True and M.DeliverDate >'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/01 00:00:00' And  M.DeliverDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurSalePro = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "محصولات",
                            SaleType = "تحویل شده ها",
                            Bed = CurSalePro,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();

                        #endregion

                        #region برگشت شده ها

                        CurrentQueries.Add(
                            "Select Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=True And M.Status=3 And M.Sale.Closed=True and Sale.CloseDate >'" +
                            StartDate.Substring(0, 4) + "/" + EndS + " 00:00:00' And  Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurRolCre = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات اعتباری",
                            SaleType = "برگشت شده ها",
                            Bes = CurRolCre,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=True And M.Status=3 And M.Sale.Closed=True and Sale.CloseDate >'" +
                            StartDate.Substring(0, 4) + "/" + EndS + " 00:00:00' And  Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurRolUnc = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات غیر اعتباری",
                            SaleType = "برگشت شده ها",
                            Bes = CurRolUnc,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=True And M.Status=3 And M.Sale.Closed=True and Sale.CloseDate >'" +
                            StartDate.Substring(0, 4) + "/" + EndS + " 00:00:00' And  Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurRolPro = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "محصولات",
                            SaleType = "برگشت شده ها",
                            Bes = CurRolPro,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();

                        #endregion

                        #region مالی ها

                        CurrentQueries.Add(
                            "Select Sum(M.ConfirmedCost) From Fiscal M where M.Confirm=2 and ConfirmedCost>0 And M.InvestDate >='" +
                             StartDate.Substring(0, 4) + "/" + EndS + "' And  M.InvestDate<='" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31'");
                        var CurFisPay = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "دریافت",
                            SaleType = "مالی",
                            Bes = CurFisPay,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select Sum(M.ConfirmedCost) From Fiscal M where M.Confirm=2 and ConfirmedCost<0 And M.InvestDate >='" +
                             StartDate.Substring(0, 4) + "/" + EndS + "' And  M.InvestDate<='" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31'");
                        var CurFisRec = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "پرداخت",
                            SaleType = "مالی",
                            Bed = CurFisRec,
                            Type = "تراکنش"
                        });
                        CurrentQueries.Clear();

                        #endregion
                    }
                }
                else
                {
                    
                    #region تحویل شده ها

                    CurrentQueries.Add(
                        "Select -Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=False And M.Delivered=True And M.Sale.Closed=True and M.DeliverDate >'" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + " 00:00:00' And  M.DeliverDate<'" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + " 23:59:59'");
                    var CurSaleCre = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "خدمات عتباری",
                        SaleType = "تحویل شده ها",
                        Bed = CurSaleCre,
                        Type = "تراکنش"
                    });
                    CurrentQueries.Clear();
                    CurrentQueries.Add(
                        "Select -Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=False And M.Delivered=True And M.Sale.Closed=True and M.DeliverDate >'" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + " 00:00:00' And  M.DeliverDate<'" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + " 23:59:59'");
                    var CurSaleUnc = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "خدمات غیر اعتباری",
                        SaleType = "تحویل شده ها",
                        Bed = CurSaleUnc,
                        Type = "تراکنش"
                    });
                    CurrentQueries.Clear();

                    CurrentQueries.Add(
                        "Select -Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=False And M.Delivered=True And M.Sale.Closed=True and M.DeliverDate >'" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + " 00:00:00' And  M.DeliverDate<'" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + " 23:59:59'");
                    var CurSalePro = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "محصولات",
                        SaleType = "تحویل شده ها",
                        Bed = CurSalePro,
                        Type = "تراکنش"
                    });
                    CurrentQueries.Clear();

                    #endregion

                    #region برگشت شده ها

                    CurrentQueries.Add(
                        "Select Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=True And M.Status=3 And M.Sale.Closed=True and Sale.CloseDate >'" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + " 00:00:00' And  Sale.CloseDate<'" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + " 23:59:59'");
                    var CurRolCre = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "خدمات اعتباری",
                        SaleType = "برگشت شده ها",
                        Bes = CurRolCre,
                        Type = "تراکنش"
                    });
                    CurrentQueries.Clear();

                    CurrentQueries.Add(
                        "Select Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=True And M.Status=3 And M.Sale.Closed=True and Sale.CloseDate >'" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + " 00:00:00' And  Sale.CloseDate<'" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + " 23:59:59'");
                    var CurRolUnc = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "خدمات غیر اعتباری",
                        SaleType = "برگشت شده ها",
                        Bes = CurRolUnc,
                        Type = "تراکنش"
                    });
                    CurrentQueries.Clear();

                    CurrentQueries.Add(
                        "Select Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=True And M.Status=3 And M.Sale.Closed=True and Sale.CloseDate >'" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + " 00:00:00' And  Sale.CloseDate<'" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + " 23:59:59'");
                    var CurRolPro = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "محصولات",
                        SaleType = "برگشت شده ها",
                        Bes = CurRolPro,
                        Type = "تراکنش"
                    });
                    CurrentQueries.Clear();

                    #endregion

                    #region مالی ها

                    CurrentQueries.Add(
                        "Select Sum(M.ConfirmedCost) From Fiscal M where M.Confirm=2 and ConfirmedCost>0 And M.InvestDate >='" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + " ' And  M.InvestDate<='" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + "'");
                    var CurFisPay = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "دریافت",
                        SaleType = "مالی",
                        Bes = CurFisPay,
                        Type = "تراکنش"
                    });
                    CurrentQueries.Clear();

                    CurrentQueries.Add(
                        "Select Sum(M.ConfirmedCost) From Fiscal M where M.Confirm=2 and ConfirmedCost<0 And M.InvestDate >='" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + "' And  M.InvestDate<='" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + "'");
                    var CurFisRec = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "پرداخت",
                        SaleType = "مالی",
                        Bed = CurFisRec,
                        Type = "تراکنش"
                    });
                    CurrentQueries.Clear();

                    #endregion
                }
            }

            #region محاسبات نهایی

            var temp = list.Sum(x => x.Bed) + list.Sum(x => x.Bes);
            list.Add(new CanDeliverCostView()
            {
                Domain = "آخر دوره",
                Month = "",
                SaleDetailType = "",
                SaleType = "",
                Bes = temp < 0 ? 0 : temp,
                Bed = temp > 0 ? 0 : temp,
                Type = "تراکنش"
            });
            long akharin = list.Last().Bed!=0?list.Last().Bed:list.Last().Bes ;

            CurrentQueries.Add(
                "Select Sum(CanDeliverCost) from Customer M");
            var Today = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
            list.Add(new CanDeliverCostView()
            {
                Domain = "در لحظه",
                Month = "",
                SaleDetailType = "",
                SaleType = "",
                Bes = Today < 0 ? 0 : Today,
                Bed = Today > 0 ? 0 : Today,
                Type = "تراکنش"
            });
            CurrentQueries.Clear();
            long Lahze = list.Last().Bed != 0 ? list.Last().Bed : list.Last().Bes;
            
            list.Add(new CanDeliverCostView()
            {
                Domain = "اختلاف",
                Month = "",
                SaleDetailType = "",
                SaleType = "",
                Bed = akharin - Lahze,
                Type = "تراکنش"
            });

            #endregion

            response.data = list;

            return response;

        }

        #endregion

        #region Customers Balance

        public GetGeneralResponse<IEnumerable<CanDeliverCostView>> GetSaleBalance(string StartDate, string EndDate)
        {
            var response = new GetGeneralResponse<IEnumerable<CanDeliverCostView>>();
            IList<CanDeliverCostView> list = new List<CanDeliverCostView>();


            IList<string> lastQueries = new List<string>();
            lastQueries.Add("Select -Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=False And M.Sale.Closed=True And M.Sale.CloseDate <'" + StartDate + " 00:00:00'");
            lastQueries.Add("Select -Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=False And  M.Sale.Closed=True And  M.Sale.CloseDate <'" + StartDate + " 00:00:00'");
            lastQueries.Add("Select -Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=False And M.Sale.Closed=True And   M.Sale.CloseDate <'" + StartDate + " 00:00:00'");

            lastQueries.Add("Select Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=True  And M.Sale.Closed=True And   M.Sale.CloseDate <'" + StartDate + " 00:00:00'");
            lastQueries.Add("Select Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=True And M.Sale.Closed=True And   M.Sale.CloseDate <'" + StartDate + " 00:00:00'");
            lastQueries.Add("Select Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=True And M.Sale.Closed=True And   M.Sale.CloseDate <'" + StartDate + " 00:00:00'");

            lastQueries.Add("Select Sum(M.ConfirmedCost) From Fiscal M where M.Confirm=2 And M.InvestDate <'" + StartDate + "'");

            var LastDomain = _creditSaleDetailRepository.FindSumOfLineTotal(lastQueries);

            list.Add(new CanDeliverCostView()
            {
                Domain = "مانده قبل",
                Bes = LastDomain < 0 ? 0 : LastDomain,
                Bed = LastDomain > 0 ? 0 : LastDomain
            });

            int start = Convert.ToInt32(StartDate.Substring(5, 2));
            int end = Convert.ToInt32(EndDate.Substring(5, 2));

            IList<string> CurrentQueries = new List<string>();
            bool isStart = new bool();
            for (int i = start; i <= end; i++)
            {

                string EndS = i > 9 ? i.ToString() : "0" + i.ToString();
                string EndT = i > 9 ? (i - 1).ToString() : "0" + i.ToString();

                if (i != end)
                {

                    if (!isStart)
                    {
                        #region فروش ها

                        CurrentQueries.Add(
                            "Select -Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=False  And M.Sale.Closed=True And  M.Sale.CloseDate >'" +
                            StartDate + " 00:00:00' And  M.Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurSaleCre = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات عتباری",
                            SaleType = "تحویل شده ها",
                            Bed = CurSaleCre,
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select -Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=False And M.Sale.Closed=True And  M.Sale.CloseDate >'" +
                            StartDate + " 00:00:00' And  M.Sale.CloseDate <'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurSaleUnc = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات غیر اعتباری",
                            SaleType = "تحویل شده ها",
                            Bed = CurSaleUnc,
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select -Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=False  And M.Sale.Closed=True and   M.Sale.CloseDate >'" +
                            StartDate + " 00:00:00' And    M.Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurSalePro = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "محصولات",
                            SaleType = "تحویل شده ها",
                            Bed = CurSalePro,
                        });
                        CurrentQueries.Clear();

                        #endregion

                        #region برگشت شده ها

                        CurrentQueries.Add(
                            "Select Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=True  And M.Sale.Closed=True and Sale.CloseDate >'" +
                            StartDate + " 00:00:00' And  Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurRolCre = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات اعتباری",
                            SaleType = "برگشت شده ها",
                            Bes = CurRolCre,
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=True And M.Sale.Closed=True and Sale.CloseDate >'" +
                            StartDate + " 00:00:00' And  Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurRolUnc = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات غیر اعتباری",
                            SaleType = "برگشت شده ها",
                            Bes = CurRolUnc,
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=True And M.Sale.Closed=True and Sale.CloseDate >'" +
                            StartDate + " 00:00:00' And  Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurRolPro = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "محصولات",
                            SaleType = "برگشت شده ها",
                            Bes = CurRolPro,
                        });
                        CurrentQueries.Clear();

                        #endregion

                        #region مالی ها

                        CurrentQueries.Add(
                            "Select Sum(M.ConfirmedCost) From Fiscal M where M.Confirm=2 and ConfirmedCost>0 And M.InvestDate >='" +
                            StartDate + "' And  M.InvestDate<='" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31'");
                        var CurFisPay = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "دریافت",
                            SaleType = "مالی",
                            Bes = CurFisPay,
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select Sum(M.ConfirmedCost) From Fiscal M where M.Confirm=2 and ConfirmedCost<0 And M.InvestDate >='" +
                            StartDate + "' And  M.InvestDate<='" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31'");
                        var CurFisRec = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "پرداخت",
                            SaleType = "مالی",
                            Bed = CurFisRec,
                        });
                        CurrentQueries.Clear();

                        #endregion

                        isStart = true;

                    }
                    else
                    {

                        #region تحویل شده ها

                        CurrentQueries.Add(
                            "Select -Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=False And M.Sale.Closed=True and  M.Sale.CloseDate >'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/01 00:00:00' And   M.Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurSaleCre = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات عتباری",
                            SaleType = "تحویل شده ها",
                            Bed = CurSaleCre,
                        });
                        CurrentQueries.Clear();
                        CurrentQueries.Add(
                            "Select -Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=False  And M.Sale.Closed=True and  M.Sale.CloseDate >'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/01 00:00:00' And    M.Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurSaleUnc = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات غیر اعتباری",
                            SaleType = "تحویل شده ها",
                            Bed = CurSaleUnc,
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select -Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=False And M.Sale.Closed=True and   M.Sale.CloseDate >'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/01 00:00:00' And    M.Sale.CloseDate <'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurSalePro = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "محصولات",
                            SaleType = "تحویل شده ها",
                            Bed = CurSalePro,
                        });
                        CurrentQueries.Clear();

                        #endregion

                        #region برگشت شده ها

                        CurrentQueries.Add(
                            "Select Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=True And M.Sale.Closed=True and Sale.CloseDate >'" +
                            StartDate.Substring(0, 4) + "/" + EndS + " 00:00:00' And  Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurRolCre = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات اعتباری",
                            SaleType = "برگشت شده ها",
                            Bes = CurRolCre,
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=True And M.Sale.Closed=True and Sale.CloseDate >'" +
                            StartDate.Substring(0, 4) + "/" + EndS + " 00:00:00' And  Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurRolUnc = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "خدمات غیر اعتباری",
                            SaleType = "برگشت شده ها",
                            Bes = CurRolUnc,
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=True And M.Sale.Closed=True and Sale.CloseDate >'" +
                            StartDate.Substring(0, 4) + "/" + EndS + " 00:00:00' And  Sale.CloseDate<'" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31 23:59:59'");
                        var CurRolPro = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "محصولات",
                            SaleType = "برگشت شده ها",
                            Bes = CurRolPro,
                        });
                        CurrentQueries.Clear();

                        #endregion

                        #region مالی ها

                        CurrentQueries.Add(
                            "Select Sum(M.ConfirmedCost) From Fiscal M where M.Confirm=2 and ConfirmedCost>0 And M.InvestDate >='" +
                             StartDate.Substring(0, 4) + "/" + EndS + "' And  M.InvestDate<='" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31'");
                        var CurFisPay = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "دریافت",
                            SaleType = "مالی",
                            Bes = CurFisPay,
                        });
                        CurrentQueries.Clear();

                        CurrentQueries.Add(
                            "Select Sum(M.ConfirmedCost) From Fiscal M where M.Confirm=2 and ConfirmedCost<0 And M.InvestDate >='" +
                             StartDate.Substring(0, 4) + "/" + EndS + "' And  M.InvestDate<='" +
                            StartDate.Substring(0, 4) + "/" + EndS + "/31'");
                        var CurFisRec = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                        list.Add(new CanDeliverCostView()
                        {
                            Domain = "طی دوره",
                            Month = ((Month)i).ToString(),
                            SaleDetailType = "پرداخت",
                            SaleType = "مالی",
                            Bed = CurFisRec,
                        });
                        CurrentQueries.Clear();

                        #endregion
                    }
                }
                else
                {

                    #region تحویل شده ها

                    CurrentQueries.Add(
                        "Select -Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=False And M.Sale.Closed=True and  M.Sale.CloseDate >'" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + " 00:00:00' And    M.Sale.CloseDate<'" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + " 23:59:59'");
                    var CurSaleCre = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "خدمات عتباری",
                        SaleType = "تحویل شده ها",
                        Bed = CurSaleCre,
                    });
                    CurrentQueries.Clear();
                    CurrentQueries.Add(
                        "Select -Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=False And M.Sale.Closed=True and   M.Sale.CloseDate >'" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + " 00:00:00' And    M.Sale.CloseDate<'" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + " 23:59:59'");
                    var CurSaleUnc = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "خدمات غیر اعتباری",
                        SaleType = "تحویل شده ها",
                        Bed = CurSaleUnc,
                    });
                    CurrentQueries.Clear();

                    CurrentQueries.Add(
                        "Select -Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=False And M.Sale.Closed=True and   M.Sale.CloseDate >'" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + " 00:00:00' And    M.Sale.CloseDate<'" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + " 23:59:59'");
                    var CurSalePro = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "محصولات",
                        SaleType = "تحویل شده ها",
                        Bed = CurSalePro,
                    });
                    CurrentQueries.Clear();

                    #endregion

                    #region برگشت شده ها

                    CurrentQueries.Add(
                        "Select Sum(M.LineTotal) From CreditSaleDetail M where M.IsRollbackDetail=True  And M.Sale.Closed=True and Sale.CloseDate >'" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + " 00:00:00' And  Sale.CloseDate<'" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + " 23:59:59'");
                    var CurRolCre = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "خدمات اعتباری",
                        SaleType = "برگشت شده ها",
                        Bes = CurRolCre,
                    });
                    CurrentQueries.Clear();

                    CurrentQueries.Add(
                        "Select Sum(M.LineTotal) From UncreditSaleDetail M where M.IsRollbackDetail=True And M.Sale.Closed=True and Sale.CloseDate >'" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + " 00:00:00' And  Sale.CloseDate<'" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + " 23:59:59'");
                    var CurRolUnc = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "خدمات غیر اعتباری",
                        SaleType = "برگشت شده ها",
                        Bes = CurRolUnc,
                    });
                    CurrentQueries.Clear();

                    CurrentQueries.Add(
                        "Select Sum(M.LineTotal) From ProductSaleDetail M where M.IsRollbackDetail=True And M.Sale.Closed=True and Sale.CloseDate >'" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + " 00:00:00' And  Sale.CloseDate<'" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + " 23:59:59'");
                    var CurRolPro = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "محصولات",
                        SaleType = "برگشت شده ها",
                        Bes = CurRolPro,
                    });
                    CurrentQueries.Clear();

                    #endregion

                    #region مالی ها

                    CurrentQueries.Add(
                        "Select Sum(M.ConfirmedCost) From Fiscal M where M.Confirm=2 and ConfirmedCost>0 And M.InvestDate >='" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + "' And  M.InvestDate<='" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + "'");
                    var CurFisPay = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "دریافت",
                        SaleType = "مالی",
                        Bes = CurFisPay,
                    });
                    CurrentQueries.Clear();

                    CurrentQueries.Add(
                        "Select Sum(M.ConfirmedCost) From Fiscal M where M.Confirm=2 and ConfirmedCost<0 And M.InvestDate >='" +
                        StartDate.Substring(0, 4) + "/" + EndS + "/01" + "' And  M.InvestDate<='" +
                        EndDate.Substring(0, 4) + "/" + EndT + "/" + EndDate.Substring(8, 2) + "'");
                    var CurFisRec = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
                    list.Add(new CanDeliverCostView()
                    {
                        Domain = "طی دوره",
                        Month = ((Month)i).ToString(),
                        SaleDetailType = "پرداخت",
                        SaleType = "مالی",
                        Bed = CurFisRec,
                    });
                    CurrentQueries.Clear();

                    #endregion
                }
            }

            #region محاسبات نهایی

            var temp = list.Sum(x => x.Bed) + list.Sum(x => x.Bes);
            list.Add(new CanDeliverCostView()
            {
                Domain = "آخر دوره",
                Month = "",
                SaleDetailType = "",
                SaleType = "",
                Bes = temp < 0 ? 0 : temp,
                Bed = temp > 0 ? 0 : temp
            });
            long akharin = list.Last().Bed!=0?list.Last().Bed:list.Last().Bes;

            CurrentQueries.Add(
                "Select Sum(Balance) from Customer M");
            var Today = _creditSaleDetailRepository.FindSumOfLineTotal(CurrentQueries);
            list.Add(new CanDeliverCostView()
            {
                Domain = "در لحظه",
                Month = "",
                SaleDetailType = "",
                SaleType = "",
                Bes = Today < 0 ? 0 : Today,
                Bed = Today > 0 ? 0 : Today
            });
            CurrentQueries.Clear();
            long Lahze = list.Last().Bed != 0 ? list.Last().Bed : list.Last().Bes;

            list.Add(new CanDeliverCostView()
            {
                Domain = "اختلاف",
                Month = "",
                SaleDetailType = "",
                SaleType = "",
                Bed = akharin - Lahze,
            });

            #endregion

            response.data = list;

            return response;

        }

        #endregion

        public GeneralResponse Bonus()
        {
            var response = new GeneralResponse();
            try
            {
                #region Rollback uncrdit Service Comissions

                //IEnumerable<UncreditSaleDetail> uncreditSaleDetails = _uncreditSaleDetailRepository.FindAll();
                //var l = new List<BonusComission>();
                //foreach (var item in uncreditSaleDetails)
                //{
                //    if (item.IsRollbackDetail == true)
                //    {
                //        BonusComission bc = new BonusComission();

                //        var query = new Infrastructure.Querying.Query();
                //        Criterion cr = new Criterion("UnCreditSaleDetail.ID", item.MainSaleDetail.ID, CriteriaOperator.Equal);
                //        query.Add(cr);
                //        Response<BonusComission> b = _bonusComissionRepository.FindByQuery(query);
                //        var p = b.data.FirstOrDefault();
                //        if (p != null)
                //        {
                //            bc.ID = Guid.NewGuid();
                //            bc.Bonus = -p.Bonus;
                //            bc.Comission = -p.Comission;
                //            bc.IsRollback = true;
                //            bc.UnCreditSaleDetail = item.MainSaleDetail;
                //            bc.CreditSaleDetail = null;
                //            bc.ProductSaleDetail = null;
                //            bc.CreateDate = item.CreateDate;
                //            bc.CreateEmployee = item.MainSaleDetail.CreateEmployee;
                //            bc.RowVersion = 1;
                //            bc.Customer = item.MainSaleDetail.Sale.Customer;
                //            l.Add(bc);
                //            _bonusComissionRepository.Add(bc);
                //        }
                //    }
                //}

                #endregion

                #region Rollback product Comissions

                IEnumerable<ProductSaleDetail> productSaleDetails = _productSaleDetailRepository.FindAll();
                var l = new List<BonusComission>();
                foreach (var item in productSaleDetails)
                {
                    if (item.IsRollbackDetail == true)
                    {
                        BonusComission bc = new BonusComission();

                        var query = new Infrastructure.Querying.Query();
                        Criterion cr = new Criterion("ProductSaleDetail.ID", item.MainSaleDetail.ID, CriteriaOperator.Equal);
                        query.Add(cr);
                        Response<BonusComission> b = _bonusComissionRepository.FindByQuery(query);
                        var p = b.data.FirstOrDefault();
                        if (p != null)
                        {
                            bc.ID = Guid.NewGuid();
                            bc.Bonus = -p.Bonus;
                            bc.Comission = -p.Comission;
                            bc.IsRollback = true;
                            bc.UnCreditSaleDetail = null;
                            bc.CreditSaleDetail = null;
                            bc.ProductSaleDetail = item.MainSaleDetail;
                            bc.CreateDate = item.CreateDate;
                            bc.CreateEmployee = item.MainSaleDetail.CreateEmployee;
                            bc.RowVersion = 1;
                            bc.Customer = item.MainSaleDetail.Sale.Customer;
                            l.Add(bc);
                            _bonusComissionRepository.Add(bc);
                        }
                    }
                }

                #endregion


                #region Add Sale Uncredit Service To bonus Comission

                //IEnumerable<UncreditSaleDetail> uncreditSaleDetails = _uncreditSaleDetailRepository.FindAll();

                //foreach (var item in uncreditSaleDetails)
                //{
                //    BonusComission bc = new BonusComission();
                //    bc.ID = Guid.NewGuid();
                //    if (!item.IsRollbackDetail)
                //    {
                //        if (item.Sale.Closed)
                //        {
                //            if (item.LineDiscount > 0 && item.UncreditService.Bonus > 0)
                //            {
                //                bc.Bonus = item.UncreditService.Bonus - (item.LineDiscount / 10000);
                //            }
                //            else
                //            {
                //                bc.Bonus = item.UncreditService.Bonus;
                //            }
                //            if (item.Delivered)
                //            {
                //                if (item.LineDiscount > 0 && item.UncreditService.Comission > 0)
                //                {
                //                    bc.Comission = item.UncreditService.Comission - (item.LineDiscount / 10);
                //                }
                //                else
                //                {
                //                    bc.Comission = item.UncreditService.Comission;
                //                }
                //            }
                //        }
                //    }
                //    if (bc.Bonus == 0 && bc.Comission == 0)
                //        continue;
                //    if (item.Sale.Couriers != null)
                //        bc.Courier = item.Sale.Couriers;
                //    bc.Courier = null;
                //    bc.CreditSaleDetail = null;
                //    bc.UnCreditSaleDetail = item;
                //    bc.ProductSaleDetail = null;
                //    bc.Customer = item.Sale.Customer;
                //    bc.IsRollback = false;
                //    bc.RowVersion = 1;
                //    bc.CreateEmployee = item.Sale.CreateEmployee;
                //    bc.CreateDate = item.Sale.CreateDate;
                //    bc.ModifiedDate = item.DeliverDate;

                //    item.BonusComission = bc;

                //    _uncreditSaleDetailRepository.Save(item);
                //    _bonusComissionRepository.Add(bc);
                //}

                #endregion

                #region Add Sale Credit Service To bonus Comission

                //IEnumerable<CreditSaleDetail> creditSaleDetails = _creditSaleDetailRepository.FindAll();

                //foreach (var item in creditSaleDetails)
                //{
                //    BonusComission bc = new BonusComission();
                //    bc.ID = Guid.NewGuid();
                //    if (!item.IsRollbackDetail)
                //    {
                //        if (item.Sale.Closed)
                //        {
                //            if (item.LineDiscount > 0 && item.CreditService.Bonus > 0)
                //            {
                //                bc.Bonus = item.CreditService.Bonus - (item.LineDiscount/10000);
                //            }
                //            else
                //            {
                //                bc.Bonus = item.CreditService.Bonus;
                //            }
                //            if (item.Delivered)
                //            {
                //                if (item.LineDiscount > 0 && item.CreditService.Comission > 0)
                //                {
                //                    bc.Comission = item.CreditService.Comission - (item.LineDiscount/10);
                //                }
                //                else
                //                {
                //                    bc.Comission = item.CreditService.Comission;
                //                }
                //            }
                //        }
                //    }
                //    if (bc.Bonus == 0 && bc.Comission == 0)
                //        continue;
                //    bc.Courier = null;
                //    bc.CreditSaleDetail = item;
                //    bc.UnCreditSaleDetail = null;
                //    bc.ProductSaleDetail = null;
                //    bc.Customer = item.Sale.Customer;
                //    bc.IsRollback = false;
                //    bc.RowVersion = 1;
                //    bc.CreateEmployee = item.Sale.CreateEmployee;
                //    bc.CreateDate = item.Sale.CreateDate;
                //    bc.ModifiedDate = item.DeliverDate;

                //    item.BonusComission = bc;

                //    _creditSaleDetailRepository.Save(item);
                //    _bonusComissionRepository.Add(bc);
                //}

                #endregion

                #region Add Sale product Service To bonus Comission

                //IEnumerable<ProductSaleDetail> productSaleDetails = _productSaleDetailRepository.FindAll();

                //foreach (var item in productSaleDetails)
                //{
                //    BonusComission bc = new BonusComission();
                //    bc.ID = Guid.NewGuid();
                //    if (!item.IsRollbackDetail)
                //    {
                //        if (item.Sale.Closed)
                //        {
                //            if (item.LineDiscount > 0 && item.ProductPrice.Bonus > 0)
                //            {
                //                bc.Bonus = item.ProductPrice.Bonus - (item.LineDiscount / 10000);
                //            }
                //            else
                //            {
                //                bc.Bonus = item.ProductPrice.Bonus;
                //            }
                //            if (item.Delivered)
                //            {
                //                if (item.LineDiscount > 0 && item.ProductPrice.Comission > 0)
                //                {
                //                    bc.Comission = item.ProductPrice.Comission - (item.LineDiscount / 10);
                //                }
                //                else
                //                {
                //                    bc.Comission = item.ProductPrice.Comission;
                //                }
                //            }
                //        }
                //    }
                //    if (bc.Bonus == 0 && bc.Comission == 0)
                //        continue;
                //    bc.Courier = null;
                //    bc.CreditSaleDetail = null;
                //    bc.UnCreditSaleDetail = null;
                //    bc.ProductSaleDetail = item;
                //    bc.Customer = item.Sale.Customer;
                //    if (item.Sale.Couriers != null)
                //        bc.Courier = item.Sale.Couriers;
                //    bc.IsRollback = false;
                //    bc.RowVersion = 1;
                //    bc.CreateEmployee = item.Sale.CreateEmployee;
                //    bc.CreateDate = item.Sale.CreateDate;
                //    bc.ModifiedDate = item.DeliverDate;

                //    item.BonusComission = bc;

                //    _productSaleDetailRepository.Save(item);
                //    _bonusComissionRepository.Add(bc);
                //}

                #endregion


                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }
            return response;
        }
    }
}
