#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Controllers.ViewModels;
using Controllers.ViewModels.SaleCatalog;
using Infrastructure.Persian;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.SaleCatalogService;
using Services.ViewModels.Employees;
using Services.ViewModels.Sales;
using Services.ViewModels.Store;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using Controllers.ViewModels.Reports;
using Services.ViewModels.Customers;
using Model.Customers;
using Services.Messaging.CustomerCatalogService;
using Model.Sales;
using Infrastructure.Querying;
#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class SaleController : BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly ISaleService _saleService;

        private readonly ICustomerService _customerService;

        private readonly IProductPriceService _productPriceService;

        private readonly ICreditServiceService _creditServiceService;

        private readonly IUncreditServiceService _uncreditServiceService;

        private readonly IProductSaleDetailService _productSaleDetailService;

        private readonly ICreditSaleDetailService _creditSaleDetailService;

        private readonly IUncreditSaleDetailService _uncreditSaleDetailService;

        private readonly IStoreProductService _storeProductService;

        private readonly IProductService _productService;
        #endregion

        #region Ctor

        public SaleController(IEmployeeService employeeService
            , ISaleService saleService
            , ICustomerService customerService
            , IProductPriceService productPriceService
            , ICreditServiceService creditServiceService
            , IUncreditServiceService uncreditServiceService
            , IProductSaleDetailService productSaleDetailService
            , ICreditSaleDetailService creditSaleDetailService
            , IUncreditSaleDetailService uncreditSaleDetailService
            , IStoreProductService storeProductService,
            IProductService productService
            )
            : base(employeeService, customerService)
        {
            this._saleService = saleService;
            this._employeeService = employeeService;
            _customerService = customerService;
            _productPriceService = productPriceService;
            _creditServiceService = creditServiceService;
            _uncreditServiceService = uncreditServiceService;
            _productSaleDetailService = productSaleDetailService;
            _creditSaleDetailService = creditSaleDetailService;
            _uncreditSaleDetailService = uncreditSaleDetailService;
            _storeProductService = storeProductService;
            _productService = productService;
        }

        #endregion

        #region Index

        public ActionResult Index()
        {

            SaleHomePageView saleHomePageView = new SaleHomePageView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Sales_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(saleHomePageView);
            }
            #endregion

            saleHomePageView.EmployeeView = GetEmployee();
            saleHomePageView.SaleViews = this._saleService.GetSales().SaleViews;

            return View(saleHomePageView);
        }

        #endregion

        #region Read

        #region Sales_Read

        public JsonResult Sales_Read(Guid? customerID, int pageSize, int pageNumber, string sort, IList<FilterData> filter, bool? Closed)
        {
            GetSalesResponse getSalesResponse = new GetSalesResponse();

            //#region Access Check
            //bool hasPermission = GetEmployee().IsGuaranteed("Sales_Read");
            //if (!hasPermission)
            //{
            //    ModelState.AddModelError("", "AccessDenied");
            //    return Json(getSalesResponse, JsonRequestBehavior.AllowGet);
            //}
            //#endregion
            IList<FilterData> Filters = new List<FilterData>();
            if (filter != null)
            {
                foreach (var item in filter)
                {
                    Filters.Add(item);
                }
            }
            AjaxGetRequest request = new AjaxGetRequest();
            request.ID = customerID == null ? Guid.Empty : (Guid)customerID;
            request.PageSize = pageSize;
            request.PageNumber = pageNumber;
            if (customerID != null)
            {
                FilterData _filter = new FilterData()
                {
                    data = new data()
                    {
                        type = "list",
                        value = new[] { request.ID.ToString() },
                    },
                    field = "CustomerName"
                };
                Filters.Add(_filter);
            }
            bool closed = Closed == null ? true : false;
            if (!closed)
            {
                FilterData _filter = new FilterData()
                {
                    data = new data()
                    {
                        type = "boolean",
                        value = new[] { false.ToString() },
                    },
                    field = "Closed"
                };
                Filters.Add(_filter);
            }

            getSalesResponse = _saleService.GetSales(request, ConvertJsonToObject(sort), Filters, false);

            foreach (var saleview in getSalesResponse.SaleViews)
            {
                GetRequest getRequest = new GetRequest();
                getRequest.ID = saleview.ID;
                GetSaleResponse getSaleResponse = new GetSaleResponse();
                getSaleResponse = _saleService.GetSale(getRequest);

                IList<ClientSaleDetailView> details = new List<ClientSaleDetailView>();

                #region Products

                if (getSaleResponse.SaleView.ProductSaleDetails != null)
                    foreach (ProductSaleDetailView productSaleDetail in getSaleResponse.SaleView.ProductSaleDetails)
                    {
                        ClientSaleDetailView item = new ClientSaleDetailView();
                        item.SaleDetailID = productSaleDetail.ProductPriceID;
                        item.SaleDetailName = productSaleDetail.ProductPriceTitle;
                        item.Imposition = productSaleDetail.Imposition;
                        item.MaxDiscount = GetProduction(productSaleDetail.ProductPriceID).MaxDiscount;
                        item.UnitPrice = productSaleDetail.UnitPrice;
                        item.Units = productSaleDetail.Units;
                        item.RowID = productSaleDetail.ID;
                        item.RowVersion = productSaleDetail.RowVersion;
                        item.Discount = productSaleDetail.Discount;
                        item.CanDeliver = productSaleDetail.CanDeliver;
                        item.RollbackPrice = productSaleDetail.RollbackPrice;
                        if (productSaleDetail.Rollbacked)
                        {
                            item.RollbackEmployeeName = productSaleDetail.RollbackEmployeeName;
                            item.RollbackDate = productSaleDetail.RollbackDate;
                        }
                        if (productSaleDetail.Delivered)
                        {
                            item.DeliverDate = productSaleDetail.DeliverDate;
                            item.DeliverEmployeeName = productSaleDetail.DeliverEmployeeName;
                        }
                        details.Add(item);
                    }

                #endregion

                #region Credits

                if (getSaleResponse.SaleView.CreditSaleDetails != null)
                    foreach (CreditSaleDetailView creditSaleDetail in getSaleResponse.SaleView.CreditSaleDetails)
                    {
                        ClientSaleDetailView item = new ClientSaleDetailView();
                        item.SaleDetailID = creditSaleDetail.CreditServiceID;
                        item.SaleDetailName = creditSaleDetail.CreditServiceName;
                        item.Imposition = creditSaleDetail.Imposition;
                        item.MaxDiscount = GetProduction(creditSaleDetail.CreditServiceID).MaxDiscount;
                        item.UnitPrice = creditSaleDetail.UnitPrice;
                        item.Units = creditSaleDetail.Units;
                        item.RowID = creditSaleDetail.ID;
                        item.RowVersion = creditSaleDetail.RowVersion;
                        item.Discount = creditSaleDetail.Discount;
                        item.CanDeliver = creditSaleDetail.CanDeliver;
                        item.RollbackPrice = creditSaleDetail.RollbackPrice;
                        if (creditSaleDetail.Rollbacked)
                        {
                            item.RollbackEmployeeName = creditSaleDetail.RollbackEmployeeName;
                            item.RollbackDate = creditSaleDetail.RollbackDate;
                        }
                        if (creditSaleDetail.Delivered)
                        {
                            item.DeliverDate = creditSaleDetail.DeliverDate;
                            item.DeliverEmployeeName = creditSaleDetail.DeliverEmployeeName;
                        }
                        details.Add(item);
                    }

                #endregion

                #region Uncredits

                if (getSaleResponse.SaleView.UncreditSaleDetails != null)
                    foreach (UncreditSaleDetailView UncreditSaleDetail in getSaleResponse.SaleView.UncreditSaleDetails)
                    {
                        ClientSaleDetailView item = new ClientSaleDetailView();
                        item.SaleDetailID = UncreditSaleDetail.UncreditServiceID;
                        item.SaleDetailName = UncreditSaleDetail.UncreditServiceName;
                        item.Imposition = UncreditSaleDetail.Imposition;
                        item.MaxDiscount = GetProduction(UncreditSaleDetail.UncreditServiceID).MaxDiscount;
                        item.UnitPrice = UncreditSaleDetail.UnitPrice;
                        item.Units = UncreditSaleDetail.Units;
                        item.RowID = UncreditSaleDetail.ID;
                        item.RowVersion = UncreditSaleDetail.RowVersion;
                        item.Discount = UncreditSaleDetail.Discount;
                        item.CanDeliver = UncreditSaleDetail.CanDeliver;
                        item.RollbackPrice = UncreditSaleDetail.RollbackPrice;
                        if (UncreditSaleDetail.Rollbacked)
                        {
                            item.RollbackEmployeeName = UncreditSaleDetail.RollbackEmployeeName;
                            item.RollbackDate = UncreditSaleDetail.RollbackDate;
                        }
                        if (UncreditSaleDetail.Delivered)
                        {
                            item.DeliverDate = UncreditSaleDetail.DeliverDate;
                            item.DeliverEmployeeName = UncreditSaleDetail.DeliverEmployeeName;
                        }
                        details.Add(item);
                    }

                #endregion

                saleview.SaleDetails = details;
            }


            return Json(getSalesResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Salse that has undelivered and un rolbacked sale detail

        public JsonResult Sales_Read_Report(int? pageSize, int? pageNumber, string sort, IList<FilterData> filter, bool? IsCustomer)
        {
            GetSalesResponse getSalesResponse = new GetSalesResponse();

            //#region Access Check
            //bool hasPermission = GetEmployee().IsGuaranteed("Sales_Read_Report");
            //if (!hasPermission)
            //{
            //    ModelState.AddModelError("", "AccessDenied");
            //    return Json(getSalesResponse, JsonRequestBehavior.AllowGet);
            //}
            //#endregion

            IList<FilterData> Filters = new List<FilterData>();
            if (filter != null)
            {
                foreach (var item in filter)
                {
                    Filters.Add(item);
                }
            }
            AjaxGetRequest request = new AjaxGetRequest();
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            request.PageSize = PageSize;
            request.PageNumber = PageNumber;


            Filters.Add(new FilterData()
            {
                data = new data()
                {
                    type = "boolean",
                    value = new[] { false.ToString() },
                },
                field = "Delivered"

            });
            Filters.Add(new FilterData()
            {
                data = new data()
                {
                    type = "boolean",
                    value = new[] { false.ToString() },
                },
                field = "Rollbacked"
            });
            Filters.Add(new FilterData()
            {
                data = new data()
                {
                    type = "boolean",
                    value = new[] { false.ToString() },
                },
                field = "IsRollbackDetail"
            });
            Filters.Add(new FilterData()
            {
                data = new data()
                {
                    type = "boolean",
                    value = new[] { true.ToString() },
                },
                field = "Sale.Closed"
            });


            getSalesResponse = _saleService.GetSales(request, ConvertJsonToObject(sort), Filters, true);

            if (IsCustomer == true)
            {
                GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();

                IList<CustomerView> customers = new List<CustomerView>();
                if (getSalesResponse.SaleViews != null)
                    foreach (var item in getSalesResponse.SaleViews)
                    {
                        customers.Add(item.Customer);
                    }
                response.data = customers;
                response.totalCount = getSalesResponse.TotalCount;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            return Json(getSalesResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SaleDetails_Read

        public ActionResult SaleDetails_Read(Guid saleID)
        {
            IList<ClientSaleDetailViewModel> details = new List<ClientSaleDetailViewModel>();

            //#region Access Check
            //bool hasPermission = GetEmployee().IsGuaranteed("SaleDetails_Read");
            //if (!hasPermission)
            //{
            //    ModelState.AddModelError("", "AccessDenied");
            //    return Json(details, JsonRequestBehavior.AllowGet);
            //}
            //#endregion

            GetRequest getRequest = new GetRequest();
            getRequest.ID = saleID;
            GetSaleResponse getSaleResponse = new GetSaleResponse();
            getSaleResponse = _saleService.GetSale(getRequest);



            #region Products

            if (getSaleResponse.SaleView.ProductSaleDetails != null)
                foreach (ProductSaleDetailView productSaleDetail in getSaleResponse.SaleView.ProductSaleDetails)
                {
                    ClientSaleDetailViewModel item = new ClientSaleDetailViewModel();
                    item.SaleDetailID = productSaleDetail.ProductPriceID;
                    item.SaleDetailName = productSaleDetail.ProductPriceTitle;
                    item.Imposition = productSaleDetail.Imposition;
                    item.MaxDiscount = GetProduction(productSaleDetail.ProductPriceID).MaxDiscount;
                    item.UnitPrice = productSaleDetail.UnitPrice;
                    item.Units = productSaleDetail.Units;
                    item.RowID = productSaleDetail.ID;
                    item.RowVersion = productSaleDetail.RowVersion;
                    item.Discount = productSaleDetail.Discount;
                    item.CanDeliver = productSaleDetail.CanDeliver;
                    item.RollbackPrice = productSaleDetail.RollbackPrice;
                    if (productSaleDetail.Rollbacked)
                    {
                        item.RollbackEmployeeName = productSaleDetail.RollbackEmployeeName;
                        item.RollbackDate = productSaleDetail.RollbackDate;
                    }
                    if (productSaleDetail.Delivered)
                    {
                        item.DeliverDate = productSaleDetail.DeliverDate;
                        item.DeliverEmployeeName = productSaleDetail.DeliverEmployeeName;
                    }
                    details.Add(item);
                }

            #endregion

            #region Credits

            if (getSaleResponse.SaleView.CreditSaleDetails != null)
                foreach (CreditSaleDetailView creditSaleDetail in getSaleResponse.SaleView.CreditSaleDetails)
                {
                    ClientSaleDetailViewModel item = new ClientSaleDetailViewModel();
                    item.SaleDetailID = creditSaleDetail.CreditServiceID;
                    item.SaleDetailName = creditSaleDetail.CreditServiceName;
                    item.Imposition = creditSaleDetail.Imposition;
                    item.MaxDiscount = GetProduction(creditSaleDetail.CreditServiceID).MaxDiscount;
                    item.UnitPrice = creditSaleDetail.UnitPrice;
                    item.Units = creditSaleDetail.Units;
                    item.RowID = creditSaleDetail.ID;
                    item.RowVersion = creditSaleDetail.RowVersion;
                    item.Discount = creditSaleDetail.Discount;
                    item.CanDeliver = creditSaleDetail.CanDeliver;
                    item.RollbackPrice = creditSaleDetail.RollbackPrice;
                    if (creditSaleDetail.Rollbacked)
                    {
                        item.RollbackEmployeeName = creditSaleDetail.RollbackEmployeeName;
                        item.RollbackDate = creditSaleDetail.RollbackDate;
                    }
                    if (creditSaleDetail.Delivered)
                    {
                        item.DeliverDate = creditSaleDetail.DeliverDate;
                        item.DeliverEmployeeName = creditSaleDetail.DeliverEmployeeName;
                    }
                    details.Add(item);

                    //details.Add(new ClientSaleDetailViewModel
                    //{
                    //    SaleDetailID = creditSaleDetail.CreditServiceID,
                    //    SaleDetailName = creditSaleDetail.CreditServiceName,
                    //    Imposition = creditSaleDetail.Imposition,
                    //    MaxDiscount = GetProduction(creditSaleDetail.CreditServiceID).MaxDiscount,
                    //    UnitPrice = creditSaleDetail.UnitPrice,
                    //    Units = creditSaleDetail.Units,
                    //    RowID = creditSaleDetail.ID,
                    //    RowVersion = creditSaleDetail.RowVersion,
                    //    Discount = creditSaleDetail.Discount,
                    //    CanDeliver = creditSaleDetail.CanDeliver                                  
                    //});
                }

            #endregion

            #region Uncredits

            if (getSaleResponse.SaleView.UncreditSaleDetails != null)
                foreach (UncreditSaleDetailView UncreditSaleDetail in getSaleResponse.SaleView.UncreditSaleDetails)
                {
                    ClientSaleDetailViewModel item = new ClientSaleDetailViewModel();
                    item.SaleDetailID = UncreditSaleDetail.UncreditServiceID;
                    item.SaleDetailName = UncreditSaleDetail.UncreditServiceName;
                    item.Imposition = UncreditSaleDetail.Imposition;
                    item.MaxDiscount = GetProduction(UncreditSaleDetail.UncreditServiceID).MaxDiscount;
                    item.UnitPrice = UncreditSaleDetail.UnitPrice;
                    item.Units = UncreditSaleDetail.Units;
                    item.RowID = UncreditSaleDetail.ID;
                    item.RowVersion = UncreditSaleDetail.RowVersion;
                    item.Discount = UncreditSaleDetail.Discount;
                    item.CanDeliver = UncreditSaleDetail.CanDeliver;
                    item.RollbackPrice = UncreditSaleDetail.RollbackPrice;
                    if (UncreditSaleDetail.Rollbacked)
                    {
                        item.RollbackEmployeeName = UncreditSaleDetail.RollbackEmployeeName;
                        item.RollbackDate = UncreditSaleDetail.RollbackDate;
                    }
                    if (UncreditSaleDetail.Delivered)
                    {
                        item.DeliverDate = UncreditSaleDetail.DeliverDate;
                        item.DeliverEmployeeName = UncreditSaleDetail.DeliverEmployeeName;
                    }
                    details.Add(item);

                    //details.Add(new ClientSaleDetailViewModel
                    //{
                    //    SaleDetailID = UncreditSaleDetail.UncreditServiceID,
                    //    SaleDetailName = UncreditSaleDetail.UncreditServiceName,
                    //    Imposition = UncreditSaleDetail.Imposition,
                    //    MaxDiscount = GetProduction(UncreditSaleDetail.UncreditServiceID).MaxDiscount,
                    //    UnitPrice = UncreditSaleDetail.UnitPrice,
                    //    Units = UncreditSaleDetail.Units,
                    //    RowID = UncreditSaleDetail.ID,
                    //    RowVersion = UncreditSaleDetail.RowVersion,
                    //    Discount = UncreditSaleDetail.Discount,
                    //    CanDeliver = UncreditSaleDetail.CanDeliver                               
                    //});
                }

            #endregion

            return Json(details, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Rollback

        #region Rollback Read

        public JsonResult Rollback_Read(Guid saleID)
        {
            IList<ClientRollbackViewModel> details = new List<ClientRollbackViewModel>();

            //#region Access Check
            //bool hasPermission = GetEmployee().IsGuaranteed("Rollback_Read");
            //if (!hasPermission)
            //{
            //    ModelState.AddModelError("", "AccessDenied");
            //    return Json(details, JsonRequestBehavior.AllowGet);
            //}
            //#endregion

            #region Getting main sale

            GetRequest getRequest = new GetRequest();
            getRequest.ID = saleID;
            GetSaleResponse getSaleResponse = new GetSaleResponse();
            getSaleResponse = _saleService.GetSale(getRequest);
            SaleView saleView = getSaleResponse.SaleView;

            #endregion

            details = new List<ClientRollbackViewModel>();

            #region For Products

            foreach (ProductSaleDetailView productSaleDetail in saleView.ProductSaleDetails)
            {
                ClientRollbackViewModel item = new ClientRollbackViewModel();
                item.RollbackPrice = productSaleDetail.LineTotalWithoutDiscountAndImposition;
                item.RowID = productSaleDetail.ID;
                item.SaleDetailID = productSaleDetail.ProductPriceID;
                item.SaleDetailName = productSaleDetail.ProductPriceTitle;
                item.Units = productSaleDetail.Units;
                item.CanRollbackDiscountPrice = productSaleDetail.LineDiscount;
                item.CanRollbackImpositionPrice = productSaleDetail.LineImposition;
                //آیا این محصول قبلا تحویل خورده است ؟
                if (productSaleDetail.Delivered)
                {
                    item.IsDeliverdBefor = true;
                    item.DeliverDate = productSaleDetail.DeliverDate;
                }

                if (productSaleDetail.IsRollbackDetail || productSaleDetail.Rollbacked)
                {
                    item.RollbackNote = productSaleDetail.RollbackNote;
                    item.RollbackNetworkPrice = null;
                    item.RollBackEmployeeName = saleView.CreateEmployeeName;
                    item.RollbackDate = productSaleDetail.CreateDate;
                    item.RoleBacked = true;
                    item.RollbackPrice = productSaleDetail.LineTotal;
                }

                details.Add(item);
            }

            #endregion

            #region For Credits

            foreach (CreditSaleDetailView creditSaleDetail in saleView.CreditSaleDetails)
            {

                long? rollbackNetworkPrice = null;
                if (creditSaleDetail.DeliverDate != null)
                    rollbackNetworkPrice = creditSaleDetail.PurchaseUnitPrice * creditSaleDetail.Units;

                ClientRollbackViewModel item = new ClientRollbackViewModel();
                item.RollbackNote = creditSaleDetail.RollbackNote;
                item.RollbackPrice = creditSaleDetail.LineTotalWithoutDiscountAndImposition;
                item.RowID = creditSaleDetail.ID;
                item.SaleDetailID = creditSaleDetail.CreditServiceID;
                item.SaleDetailName = creditSaleDetail.CreditServiceName;
                item.Units = creditSaleDetail.Units;
                item.CanRollbackDiscountPrice = creditSaleDetail.LineDiscount;
                item.CanRollbackImpositionPrice = creditSaleDetail.LineImposition;
                //آیا این محصول قبلا تحویل شده است ؟
                if (creditSaleDetail.Delivered)
                {
                    item.IsDeliverdBefor = true;
                    item.DeliverDate = creditSaleDetail.DeliverDate;
                }
                item.Units = creditSaleDetail.Units;
                if (creditSaleDetail.IsRollbackDetail || creditSaleDetail.Rollbacked)
                {
                    item.RollbackNetworkPrice = rollbackNetworkPrice;
                    item.RollBackEmployeeName = creditSaleDetail.CreateEmployeeName;
                    item.RollbackDate = creditSaleDetail.CreateDate;
                    item.RoleBacked = true;
                    //item.RollbackPrice = creditSaleDetail.LineTotalWithoutDiscountAndImposition;
                    item.RollbackPrice = creditSaleDetail.LineTotal;
                }
                details.Add(item);
            }

            #endregion

            #region For Uncredits
            //تغییر داده شده
            foreach (UncreditSaleDetailView uncreditSaleDetail in saleView.UncreditSaleDetails)//.Where(x=>x.Rollbacked=true))
            {
                ClientRollbackViewModel item = new ClientRollbackViewModel();
                item.RollbackNote = uncreditSaleDetail.RollbackNote;
                item.RollbackPrice = uncreditSaleDetail.LineTotalWithoutDiscountAndImposition;
                item.RowID = uncreditSaleDetail.ID;
                item.SaleDetailID = uncreditSaleDetail.UncreditServiceID;
                item.SaleDetailName = uncreditSaleDetail.UncreditServiceName;
                item.Units = uncreditSaleDetail.Units;
                item.CanRollbackDiscountPrice = uncreditSaleDetail.LineDiscount;
                item.CanRollbackImpositionPrice = uncreditSaleDetail.LineImposition;
                //آیا این محصول قبلا تحویل خورده است ؟
                if (uncreditSaleDetail.Delivered)
                {
                    item.IsDeliverdBefor = true;
                    item.DeliverDate = uncreditSaleDetail.CreateDate;
                }
                if (uncreditSaleDetail.IsRollbackDetail || uncreditSaleDetail.Rollbacked)
                {
                    item.RollbackNetworkPrice = null;
                    item.RollBackEmployeeName = uncreditSaleDetail.CreateEmployeeName;
                    item.RollbackDate = uncreditSaleDetail.CreateDate;
                    item.RoleBacked = true;
                    item.RollbackPrice = uncreditSaleDetail.LineTotal;
                }
                details.Add(item);

            }

            #endregion

            return Json(details, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Rollback Insert
        /// <summary>
        /// برای ایجاد برگشت از فروش جدید
        /// </summary>
        /// <param name="customerID">شناسه مشتری</param>
        /// <param name="saleID">شناسه فروشی که قرار است برگشت بخورد</param>
        /// <param name="saleDetails">اقلام فاکتور فروش یا برگشت از فروش</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Rollback_Insert(string customerID, string saleID,
            [Bind(Prefix = "saleDetails")]
            IEnumerable<ClientRollbackViewModel> saleDetails)
        {
            GeneralResponse addresponse = new GeneralResponse();
            bool hasPermission = false;
            //#region Access Check
            //hasPermission = GetEmployee().IsGuaranteed("Rollback_Insert");
            //if (!hasPermission)
            //{
            //    addresponse.ErrorMessages.Add("AccessDenied");
            //    return Json(addresponse, JsonRequestBehavior.AllowGet);
            //}
            //#endregion

            if (saleDetails != null)
            {
                #region Declares

                IList<AddProductSaleDetailRequest> addProductSaleDetailRequests = new List<AddProductSaleDetailRequest>();
                IList<AddCreditSaleDetailRequest> addCreditSaleDetailRequests = new List<AddCreditSaleDetailRequest>();
                IList<AddUncreditSaleDetailRequest> addUncreditSaleDetailRequests = new List<AddUncreditSaleDetailRequest>();

                #endregion

                foreach (var saleDetail in saleDetails)
                {



                    char DetailType = GetDetailType(saleDetail.SaleDetailID);
                    if (saleDetail.RollbackDate != null)
                    {
                        addresponse.ErrorMessages.Add("برگشت از فروش این آیتم قبلا انجام شده است");
                        return Json(addresponse, JsonRequestBehavior.AllowGet);
                    }

                    #region If Product

                    if (DetailType == 'P')
                    {
                        #region Access Check

                        if (!saleDetail.IsDeliverdBefor)
                        {
                            hasPermission = GetEmployee().IsGuaranteed("Product_RollBack");
                            if (!hasPermission)
                            {
                                addresponse.ErrorMessages.Add("شما مجاز به برگشت کالای تحویل نشده نیستید");
                                return Json(addresponse, JsonRequestBehavior.AllowGet);
                            }
                        }

                        if (saleDetail.IsDeliverdBefor)
                        {
                            hasPermission = GetEmployee().IsGuaranteed("Product_RollBack_Delivered");
                            if (!hasPermission)
                            {
                                addresponse.ErrorMessages.Add("شما مجاز به برگشت کالای تحویل شده نیستید");
                                return Json(addresponse, JsonRequestBehavior.AllowGet);
                            }
                        }

                        #endregion

                        AddProductSaleDetailRequest addProductSaleDetailRequest = new AddProductSaleDetailRequest();
                        addProductSaleDetailRequest.ProductPriceID = saleDetail.SaleDetailID;
                        addProductSaleDetailRequest.Units = saleDetail.Units;
                        addProductSaleDetailRequest.UnitPrice = 0;
                        addProductSaleDetailRequest.Discount = 0;
                        addProductSaleDetailRequest.Imposition = 0;
                        addProductSaleDetailRequest.CreateEmployeeID = GetEmployee().ID;
                        addProductSaleDetailRequest.RollbackNote = saleDetail.RollbackNote;
                        addProductSaleDetailRequest.RollbackPrice = saleDetail.RollbackPrice;
                        addProductSaleDetailRequest.MainProductSaleDetailID = saleDetail.RowID;
                        addProductSaleDetailRequest.IsDeliverdBefor = saleDetail.IsDeliverdBefor;
                        addProductSaleDetailRequest.IsRollbackDetail = true;
                        addProductSaleDetailRequest.CanRollbackDiscountPrice = saleDetail.CanRollbackDiscountPrice;
                        addProductSaleDetailRequest.CanRollbackImpositionPrice = saleDetail.CanRollbackImpositionPrice;
                        addProductSaleDetailRequests.Add(addProductSaleDetailRequest);
                    }
                    #endregion

                    #region If Credit

                    else if (DetailType == 'C')
                    {
                        #region Access Check

                        if (!saleDetail.IsDeliverdBefor)
                        {
                            hasPermission = GetEmployee().IsGuaranteed("CreditSaledetail_RollBack");
                            if (!hasPermission)
                            {
                                addresponse.ErrorMessages.Add("شما مجاز به برگشت خدمات اعتباری تحویل نشده نیستید");
                                return Json(addresponse, JsonRequestBehavior.AllowGet);
                            }
                        }

                        if (saleDetail.IsDeliverdBefor)
                        {
                            hasPermission = GetEmployee().IsGuaranteed("CreditSaledetail_RollBack_Delivered");
                            if (!hasPermission)
                            {
                                addresponse.ErrorMessages.Add("شما مجاز به برگشت خدمات اعتباری تحویل شده نیستید");
                                return Json(addresponse, JsonRequestBehavior.AllowGet);
                            }
                        }
                        #endregion


                        AddCreditSaleDetailRequest addCreditSaleDetailRequest = new AddCreditSaleDetailRequest();
                        addCreditSaleDetailRequest.CreditServiceID = saleDetail.SaleDetailID;
                        addCreditSaleDetailRequest.Units = saleDetail.Units;
                        addCreditSaleDetailRequest.UnitPrice = 0;
                        addCreditSaleDetailRequest.Discount = 0;
                        addCreditSaleDetailRequest.Imposition = 0;
                        addCreditSaleDetailRequest.CreateEmployeeID = GetEmployee().ID;
                        addCreditSaleDetailRequest.RollbackNote = saleDetail.RollbackNote;
                        addCreditSaleDetailRequest.RollbackPrice = saleDetail.RollbackPrice;
                        addCreditSaleDetailRequest.RollbackNetworkPrice = saleDetail.RollbackNetworkPrice;
                        addCreditSaleDetailRequest.MainCreditSaleDetailID = saleDetail.RowID;
                        addCreditSaleDetailRequest.IsDeliverdBefor = saleDetail.IsDeliverdBefor;
                        addCreditSaleDetailRequest.IsRollbackDetail = true;
                        addCreditSaleDetailRequest.CanRollbackDiscountPrice = saleDetail.CanRollbackDiscountPrice;
                        addCreditSaleDetailRequest.CanRollbackImpositionPrice = saleDetail.CanRollbackImpositionPrice;
                        addCreditSaleDetailRequests.Add(addCreditSaleDetailRequest);
                    }
                    #endregion

                    #region If Uncredit

                    else if (DetailType == 'U')
                    {

                        #region Access Check

                        if (!saleDetail.IsDeliverdBefor)
                        {
                            hasPermission = GetEmployee().IsGuaranteed("Uncreditsaledetail_RollBack");
                            if (!hasPermission)
                            {
                                addresponse.ErrorMessages.Add("شما مجاز به برگشت خدمات غیر اعتباری تحویل نشده نیستید");
                                return Json(addresponse, JsonRequestBehavior.AllowGet);
                            }
                        }
                        if (saleDetail.IsDeliverdBefor)
                        {
                            hasPermission = GetEmployee().IsGuaranteed("Uncreditsaledetail_RollBack_Delivered");
                            if (!hasPermission)
                            {
                                addresponse.ErrorMessages.Add("شما مجاز به برگشت خدمات غیر اعتباری تحویل شده نیستید");
                                return Json(addresponse, JsonRequestBehavior.AllowGet);
                            }
                        }

                        #endregion

                        AddUncreditSaleDetailRequest addUncreditSaleDetailRequest = new AddUncreditSaleDetailRequest();

                        addUncreditSaleDetailRequest.UncreditServiceID = saleDetail.SaleDetailID;
                        addUncreditSaleDetailRequest.Units = saleDetail.Units;
                        addUncreditSaleDetailRequest.UnitPrice = 0;
                        addUncreditSaleDetailRequest.Discount = 0;
                        addUncreditSaleDetailRequest.Imposition = 0;
                        addUncreditSaleDetailRequest.CreateEmployeeID = GetEmployee().ID;
                        addUncreditSaleDetailRequest.RollbackNote = saleDetail.RollbackNote;
                        addUncreditSaleDetailRequest.RollbackPrice = saleDetail.RollbackPrice;
                        addUncreditSaleDetailRequest.MainUncreditSaleDetailID = saleDetail.RowID;
                        addUncreditSaleDetailRequest.IsDeliverdBefor = saleDetail.IsDeliverdBefor;
                        addUncreditSaleDetailRequest.IsRollbackDetail = true;

                        addUncreditSaleDetailRequest.CanRollbackDiscountPrice = saleDetail.CanRollbackDiscountPrice;
                        addUncreditSaleDetailRequest.CanRollbackImpositionPrice = saleDetail.CanRollbackImpositionPrice;

                        addUncreditSaleDetailRequests.Add(addUncreditSaleDetailRequest);
                    }
                    #endregion
                }

                #region Insert Rollback Sale

                AddSaleRequest addRequest = new AddSaleRequest();

                addRequest.CustomerID = Guid.Parse(customerID);
                addRequest.CreateEmployeeID = GetEmployee().ID;
                addRequest.AddProductSaleDetailRequests = addProductSaleDetailRequests;
                addRequest.AddCreditSaleDetailRequests = addCreditSaleDetailRequests;
                addRequest.AddUncreditSaleDetailRequests = addUncreditSaleDetailRequests;
                addRequest.SaleID = saleID == "" ? Guid.Empty : Guid.Parse(saleID);
                addRequest.IsRollback = true;
                addresponse = _saleService.AddSale(addRequest);

                #endregion

            }

            return Json(addresponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Delivery

        #region Read

        public ActionResult SaleDetails_Deliver_Read(Guid saleID)
        {
            IList<ClientDeliverViewModel> details = new List<ClientDeliverViewModel>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SaleDetails_Deliver_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(details, JsonRequestBehavior.AllowGet);
            }
            #endregion

            GetRequest getRequest = new GetRequest();
            getRequest.ID = saleID;
            GetSaleResponse getSaleResponse = new GetSaleResponse();
            getSaleResponse = _saleService.GetSale(getRequest);

            details = new List<ClientDeliverViewModel>();

            #region Products

            if (getSaleResponse.SaleView.ProductSaleDetails != null)
                foreach (ProductSaleDetailView productSaleDetail in getSaleResponse.SaleView.ProductSaleDetails)
                {
                    details.Add(new ClientDeliverViewModel
                    {
                        SaleDetailID = productSaleDetail.ProductPriceID,
                        SaleDetailName = productSaleDetail.ProductPriceTitle,
                        Units = productSaleDetail.Units,
                        RowID = productSaleDetail.ID,

                        CanDeliver = productSaleDetail.CanDeliver,
                        DeliverDate = productSaleDetail.DeliverDate,
                        DeliverNote = productSaleDetail.DeliverNote,
                        DeliverEmployeeName = productSaleDetail.DeliverEmployeeName,

                        LineTotalWithoutDiscountAndImposition = productSaleDetail.LineTotalWithoutDiscountAndImposition,
                        LineDiscount = productSaleDetail.LineDiscount,
                        LineImposition = productSaleDetail.LineImposition,
                        LineTotal = productSaleDetail.LineTotal
                    });
                }

            #endregion

            #region Credits

            if (getSaleResponse.SaleView.CreditSaleDetails != null)
                foreach (CreditSaleDetailView creditSaleDetail in getSaleResponse.SaleView.CreditSaleDetails)
                {
                    details.Add(new ClientDeliverViewModel
                    {
                        SaleDetailID = creditSaleDetail.CreditServiceID,
                        SaleDetailName = creditSaleDetail.CreditServiceName,
                        Units = creditSaleDetail.Units,
                        RowID = creditSaleDetail.ID,

                        CanDeliver = creditSaleDetail.CanDeliver,
                        DeliverDate = creditSaleDetail.DeliverDate,
                        DeliverNote = creditSaleDetail.DeliverNote,
                        DeliverEmployeeName = creditSaleDetail.DeliverEmployeeName,

                        LineTotalWithoutDiscountAndImposition = creditSaleDetail.LineTotalWithoutDiscountAndImposition,
                        LineDiscount = creditSaleDetail.LineDiscount,
                        LineImposition = creditSaleDetail.LineImposition,
                        LineTotal = creditSaleDetail.LineTotal
                    });
                }

            #endregion

            #region Uncredits

            if (getSaleResponse.SaleView.UncreditSaleDetails != null)
                foreach (UncreditSaleDetailView uncreditSaleDetail in getSaleResponse.SaleView.UncreditSaleDetails)
                {
                    details.Add(new ClientDeliverViewModel
                    {
                        SaleDetailID = uncreditSaleDetail.UncreditServiceID,
                        SaleDetailName = uncreditSaleDetail.UncreditServiceName,
                        Units = uncreditSaleDetail.Units,
                        RowID = uncreditSaleDetail.ID,

                        CanDeliver = uncreditSaleDetail.CanDeliver,
                        DeliverDate = uncreditSaleDetail.DeliverDate,
                        DeliverNote = uncreditSaleDetail.DeliverNote,
                        DeliverEmployeeName = uncreditSaleDetail.DeliverEmployeeName,

                        LineTotalWithoutDiscountAndImposition = uncreditSaleDetail.LineTotalWithoutDiscountAndImposition,
                        LineDiscount = uncreditSaleDetail.LineDiscount,
                        LineImposition = uncreditSaleDetail.LineImposition,
                        LineTotal = uncreditSaleDetail.LineTotal
                    });
                }

            #endregion

            return Json(details, JsonRequestBehavior.AllowGet);
        }

        #region Sale Detail Deliver Read

        public ActionResult SaleDetail_Deliver_Read(Guid SaleID, Guid ID, string Type)
        {
            IList<ClientDeliverViewModel> details = new List<ClientDeliverViewModel>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SaleDetails_Deliver_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(details, JsonRequestBehavior.AllowGet);
            }
            #endregion

            GetRequest getRequest = new GetRequest();
            getRequest.ID = SaleID;
            GetSaleResponse getSaleResponse = new GetSaleResponse();
            getSaleResponse = _saleService.GetSale(getRequest);

            details = new List<ClientDeliverViewModel>();

            #region Products

            if (Type == "P")
                foreach (ProductSaleDetailView productSaleDetail in getSaleResponse.SaleView.ProductSaleDetails)
                {
                    if (productSaleDetail.ID == ID)
                        details.Add(new ClientDeliverViewModel
                        {
                            SaleDetailID = productSaleDetail.ProductPriceID,
                            SaleDetailName = productSaleDetail.ProductPriceTitle,
                            Units = productSaleDetail.Units,
                            RowID = productSaleDetail.ID,

                            CanDeliver = productSaleDetail.CanDeliver,
                            DeliverDate = productSaleDetail.DeliverDate,
                            DeliverNote = productSaleDetail.DeliverNote,
                            DeliverEmployeeName = productSaleDetail.DeliverEmployeeName,

                            LineTotalWithoutDiscountAndImposition = productSaleDetail.LineTotalWithoutDiscountAndImposition,
                            LineDiscount = productSaleDetail.LineDiscount,
                            LineImposition = productSaleDetail.LineImposition,
                            LineTotal = productSaleDetail.LineTotal
                        });
                }

            #endregion

            #region Credits

            if (Type == "C")
                foreach (CreditSaleDetailView creditSaleDetail in getSaleResponse.SaleView.CreditSaleDetails)
                {
                    if (creditSaleDetail.ID == ID)
                        details.Add(new ClientDeliverViewModel
                        {
                            SaleDetailID = creditSaleDetail.CreditServiceID,
                            SaleDetailName = creditSaleDetail.CreditServiceName,
                            Units = creditSaleDetail.Units,
                            RowID = creditSaleDetail.ID,

                            CanDeliver = creditSaleDetail.CanDeliver,
                            DeliverDate = creditSaleDetail.DeliverDate,
                            DeliverNote = creditSaleDetail.DeliverNote,
                            DeliverEmployeeName = creditSaleDetail.DeliverEmployeeName,

                            LineTotalWithoutDiscountAndImposition = creditSaleDetail.LineTotalWithoutDiscountAndImposition,
                            LineDiscount = creditSaleDetail.LineDiscount,
                            LineImposition = creditSaleDetail.LineImposition,
                            LineTotal = creditSaleDetail.LineTotal
                        });
                }

            #endregion

            #region Uncredits

            if (Type == "U")
                foreach (UncreditSaleDetailView uncreditSaleDetail in getSaleResponse.SaleView.UncreditSaleDetails)
                {
                    if (uncreditSaleDetail.ID == ID)
                        details.Add(new ClientDeliverViewModel
                        {
                            SaleDetailID = uncreditSaleDetail.UncreditServiceID,
                            SaleDetailName = uncreditSaleDetail.UncreditServiceName,
                            Units = uncreditSaleDetail.Units,
                            RowID = uncreditSaleDetail.ID,

                            CanDeliver = uncreditSaleDetail.CanDeliver,
                            DeliverDate = uncreditSaleDetail.DeliverDate,
                            DeliverNote = uncreditSaleDetail.DeliverNote,
                            DeliverEmployeeName = uncreditSaleDetail.DeliverEmployeeName,

                            LineTotalWithoutDiscountAndImposition = uncreditSaleDetail.LineTotalWithoutDiscountAndImposition,
                            LineDiscount = uncreditSaleDetail.LineDiscount,
                            LineImposition = uncreditSaleDetail.LineImposition,
                            LineTotal = uncreditSaleDetail.LineTotal
                        });
                }

            #endregion

            return Json(details, JsonRequestBehavior.AllowGet);
        }



        #endregion


        public JsonResult Sale_UnClosed_Read(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<SaleView>> response = new GetGeneralResponse<IEnumerable<SaleView>>();

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _saleService.GetUnClosedSales(PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Action

        public JsonResult SaleDetail_Deliver(Guid rowID, string deliverNote)
        {
            GeneralResponse deliverResponse = new GeneralResponse();

            EmployeeView employee = GetEmployee();

            //#region Access Check
            //bool hasPermission = GetEmployee().IsGuaranteed("SaleDetail_Deliver");
            //if (!hasPermission)
            //{
            //    deliverResponse.ErrorMessages.Add("AccessDenied");
            //    return Json(deliverResponse, JsonRequestBehavior.AllowGet);
            //}
            //#endregion

            DeliverRequest request = new DeliverRequest();
            request.DeliverEmployeeID = employee.ID;
            request.DeliverNote = deliverNote;
            request.DetailType = GetDetailTypeByRowID(rowID);
            request.SaleDetailID = rowID;

            //اگر کالا بود دسترسی تحویل کالا چک شود
            if (request.DetailType == 'P')
            {

                #region Access Check
                bool hasPermission = employee.IsGuaranteed("Product_Deliver");
                if (!hasPermission)
                {
                    deliverResponse.ErrorMessages.Add("AccessDenied");
                    return Json(deliverResponse, JsonRequestBehavior.AllowGet);
                }
                #endregion
            }

            //اگر خدمات غیر اعتباری بود بود دسترسی تحویل خدمات غیر اعتبرای چک شود چک شود
            if (request.DetailType == 'U')
            {

                #region Access Check
                bool hasPermission = employee.IsGuaranteed("UnCreditService_Deliver");
                if (!hasPermission)
                {
                    deliverResponse.ErrorMessages.Add("AccessDenied");
                    return Json(deliverResponse, JsonRequestBehavior.AllowGet);
                }
                #endregion
            }

            //اگر خدمات  اعتباری بود بود دسترسی تحویل خدمات  اعتبرای چک شود چک شود
            if (request.DetailType == 'C')
            {

                #region Access Check
                bool hasPermission = employee.IsGuaranteed("CreditService_Deliver");
                if (!hasPermission)
                {
                    deliverResponse.ErrorMessages.Add("AccessDenied");
                    return Json(deliverResponse, JsonRequestBehavior.AllowGet);
                }
                #endregion
            }

            deliverResponse = _saleService.SaleDetail_Deliver(request);

            return Json(deliverResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Delete

        #region Delete Sale

        public JsonResult DeleteSale(Guid saleID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Sale_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            DeleteRequest request = new DeleteRequest();
            request.ID = saleID;

            response = _saleService.DeleteSale(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete SaleDetail

        public JsonResult DeleteSaleDetail([Bind(Prefix = "saleDetails")]
            IEnumerable<ClientSaleDetailViewModel> clientSaleDetailViewModels)
        {
            IList<DeleteRequest> deleteRequests = new List<DeleteRequest>();
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SaleDetail_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            foreach (ClientSaleDetailViewModel clientSaleDetailViewModel in clientSaleDetailViewModels)
            {
                deleteRequests.Add(new DeleteRequest() { ID = clientSaleDetailViewModel.RowID });
            }


            DeleteSaleDetailRequest request = new DeleteSaleDetailRequest();

            IList<DeleteRequest> deleteProductSaleDetailRequests = new List<DeleteRequest>();
            IList<DeleteRequest> deleteCreditSaleDetailRequests = new List<DeleteRequest>();
            IList<DeleteRequest> deleteUncreditSaleDetailRequests = new List<DeleteRequest>();

            foreach (DeleteRequest deleteRequest in deleteRequests)
            {
                char DetailType = GetDetailTypeByRowID(deleteRequest.ID);

                if (DetailType == 'P')
                {
                    deleteProductSaleDetailRequests.Add(deleteRequest);
                }
                else if (DetailType == 'C')
                {
                    deleteCreditSaleDetailRequests.Add(deleteRequest);
                }
                else if (DetailType == 'U')
                {
                    deleteUncreditSaleDetailRequests.Add(deleteRequest);
                }

                request.deleteProductSaleDetailRequests = deleteProductSaleDetailRequests;
                request.deleteCreditSaleDetailRequests = deleteCreditSaleDetailRequests;
                request.deleteUncreditSaleDetailRequests = deleteUncreditSaleDetailRequests;

                response = _saleService.DeleteSaleDetail(request);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }




        #endregion

        #endregion

        #region Insert and Update Sale with details
        /// <summary>
        /// برای ایجاد فروش جدید و افزودن آیتم به فروش قدیم
        /// </summary>
        /// <param name="customerID">شناسه کارمند</param>
        /// <param name="saleID">شناسه فروش که در ایجاد اولیه نال است</param>
        /// <param name="IsRollback">در فروش صفر و برگشت از فروش یک است</param>
        /// <param name="saleDetails">اقلام فاکتور فروش یا برگشت از فروش</param>  
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaleDetails_Insert(string customerID, string saleID,
            [Bind(Prefix = "saleDetails")]
            IEnumerable<ClientSaleDetailViewModelIU> saleDetails)
        {
            GeneralResponse addresponse = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SaleDetails_Insert");
            if (!hasPermission)
            {
                addresponse.ErrorMessages.Add("AccessDenied");
                return Json(addresponse, JsonRequestBehavior.AllowGet);
            }
            #endregion

            if (saleDetails != null)
            {
                IList<AddProductSaleDetailRequest> addProductSaleDetailRequests = new List<AddProductSaleDetailRequest>();
                IList<AddCreditSaleDetailRequest> addCreditSaleDetailRequests = new List<AddCreditSaleDetailRequest>();
                IList<AddUncreditSaleDetailRequest> addUncreditSaleDetailRequests = new List<AddUncreditSaleDetailRequest>();

                foreach (var saleDetail in saleDetails)
                {
                    char DetailType = GetDetailType(saleDetail.SaleDetailID);
                    if (DetailType == 'P')
                    {
                        addProductSaleDetailRequests.Add(
                            new AddProductSaleDetailRequest()
                            {
                                ProductPriceID = saleDetail.SaleDetailID,
                                Units = saleDetail.Units,
                                UnitPrice = saleDetail.UnitPrice,
                                Discount = saleDetail.Discount,
                                Imposition = saleDetail.Imposition,
                                CreateEmployeeID = GetEmployee().ID,
                                LineDiscount = saleDetail.Units * saleDetail.Discount,
                                LineImposition = saleDetail.Imposition * saleDetail.Units,
                                LineTotalWithoutDiscountAndImposition = saleDetail.Units * saleDetail.UnitPrice,
                            });
                    }

                    else if (DetailType == 'C')
                    {
                        addCreditSaleDetailRequests.Add(
                            new AddCreditSaleDetailRequest()
                            {
                                CreditServiceID = saleDetail.SaleDetailID,
                                Units = saleDetail.Units,
                                UnitPrice = saleDetail.UnitPrice,
                                Discount = saleDetail.Discount,
                                Imposition = saleDetail.Imposition,
                                CreateEmployeeID = GetEmployee().ID,
                                LineDiscount = saleDetail.Units * saleDetail.Discount,
                                LineImposition = saleDetail.Imposition * saleDetail.Units,
                                LineTotalWithoutDiscountAndImposition = saleDetail.Units * saleDetail.UnitPrice,
                            });
                    }

                    else if (DetailType == 'U')
                    {
                        addUncreditSaleDetailRequests.Add(
                            new AddUncreditSaleDetailRequest()
                            {
                                UncreditServiceID = saleDetail.SaleDetailID,
                                Units = saleDetail.Units,
                                UnitPrice = saleDetail.UnitPrice,
                                Discount = saleDetail.Discount,
                                Imposition = saleDetail.Imposition,
                                CreateEmployeeID = GetEmployee().ID,
                                LineDiscount = saleDetail.Units * saleDetail.Discount,
                                LineImposition = saleDetail.Imposition * saleDetail.Units,
                                LineTotalWithoutDiscountAndImposition = saleDetail.Units * saleDetail.UnitPrice,
                            });
                    }
                }

                AddSaleRequest addRequest = new AddSaleRequest();

                addRequest.CustomerID = Guid.Parse(customerID);
                addRequest.CreateEmployeeID = GetEmployee().ID;
                addRequest.AddProductSaleDetailRequests = addProductSaleDetailRequests;
                addRequest.AddCreditSaleDetailRequests = addCreditSaleDetailRequests;
                addRequest.AddUncreditSaleDetailRequests = addUncreditSaleDetailRequests;
                addRequest.SaleID = saleID == "" ? Guid.Empty : Guid.Parse(saleID);
                addRequest.IsRollback = false;
                addresponse = _saleService.AddSale(addRequest);
            }

            return Json(addresponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaleDetails_Update([Bind(Prefix = "saleDetails")]
            IEnumerable<ClientSaleDetailViewModelIU> saleDetails)
        {
            GeneralResponse editResponse = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SaleDetail_Update");
            if (!hasPermission)
            {
                editResponse.ErrorMessages.Add("AccessDenied");
                return Json(editResponse, JsonRequestBehavior.AllowGet);
            }
            #endregion

            EditSaleRequest editRequest = new EditSaleRequest();

            #region Getting SaleView

            SaleView saleView = new SaleView();
            GetRequest getRequest = new GetRequest();
            //getRequest.ID = saleID;
            GetSaleResponse getSaleResponse = new GetSaleResponse();
            getSaleResponse = _saleService.GetSale(getRequest);
            saleView = getSaleResponse.SaleView;

            #endregion

            // if SaleView exists
            if (saleView != null && saleDetails != null)
            {
                foreach (var saleDetail in saleDetails)
                {
                    // if type of saleDetail id Product
                    if (GetDetailType(saleDetail.SaleDetailID) == 'P')
                    {
                        EditProductSaleDetailRequest request = new EditProductSaleDetailRequest();
                        //saleView.ProductSaleDetails.ID = saleDetail.RowID;
                        request.ModifiedEmployeeID = GetEmployee().ID;
                        request.Discount = saleDetail.Discount;
                        request.Imposition = saleDetail.Imposition;
                        request.Units = saleDetail.Units;
                        request.UnitPrice = saleDetail.UnitPrice;
                        request.ProductPriceID = saleDetail.SaleDetailID;

                        //request.RollbackNote = saleDetail.RollbackNote;
                        //request.MainProductSaleDetailID = productSaleDetailDetailView.ProductSaleDetailView.MainProductSaleDetailID;
                        //editProductSaleDetailRequest.RowVersion = 
                    }
                    else if (GetDetailType(saleDetail.SaleDetailID) == 'C')
                    {
                        EditCreditSaleDetailRequest request = new EditCreditSaleDetailRequest();
                        request.ID = saleDetail.RowID;
                        request.ModifiedEmployeeID = GetEmployee().ID;
                        request.Discount = saleDetail.Discount;
                        request.Imposition = saleDetail.Imposition;
                        request.Units = saleDetail.Units;
                        request.UnitPrice = saleDetail.UnitPrice;
                        request.CreditServiceID = saleDetail.SaleDetailID;
                    }

                    else if (GetDetailType(saleDetail.SaleDetailID) == 'U')
                    {
                        EditUncreditSaleDetailRequest request = new EditUncreditSaleDetailRequest();
                        request.ID = saleDetail.RowID;
                        request.ModifiedEmployeeID = GetEmployee().ID;
                        request.Discount = saleDetail.Discount;
                        request.Imposition = saleDetail.Imposition;
                        request.Units = saleDetail.Units;
                        request.UnitPrice = saleDetail.UnitPrice;
                        request.UncreditServiceID = saleDetail.SaleDetailID;
                    }
                }


                //editRequest.AddProductSaleDetailRequests = addProductSaleDetailRequests;
                //editRequest.AddCreditSaleDetailRequests = addCreditSaleDetailRequests;
                //editRequest.AddUncreditSaleDetailRequests = addUncreditSaleDetailRequests;

                editResponse = _saleService.EditSale(editRequest);
            }

            return Json(editRequest, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get Production

        #region Get All Productions

        //Added By Zafari
        //Get Production for report
        public JsonResult GetAllProductionForReport()
        {
            IList<Production> productions = new List<Production>();
            IEnumerable<ProductView> productViews = _productService.GetProducts().ProductViews;
            if (productViews != null)
            {
                foreach (ProductView productView in productViews)
                {
                    productions.Add(
                        new Production()
                        {
                            ProductID = productView.ID,
                            SaleDetailID = Guid.Empty,
                            SaleDetailName = productView.ProductName,
                            Imposition = 0,
                            MaxDiscount = 0,
                            UnitPrice = 0,
                        });
                }
            }
            return Json(productions, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// لیست همه محصولات و کالاهای اعتباری وابسته به شبکه مشتری
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public JsonResult GetAllProductions(Guid? CustomerID)
        {
            IList<Production> productions = new List<Production>();

            #region Products


            IEnumerable<ProductPriceView> productPriceViews = _productPriceService.GetProductPrices().ProductPriceViews.Where(x => !x.Discontinued);

            if (productPriceViews != null)
                foreach (ProductPriceView productPriceView in productPriceViews)
                {
                    productions.Add(new Production
                    {
                        //Added By Zafari
                        ProductID = productPriceView.ProductID,
                        SaleDetailID = productPriceView.ID,
                        SaleDetailName = productPriceView.ProductPriceTitle + " - " + string.Format("{0:#,###,###.###}", productPriceView.UnitPrice),
                        Imposition = productPriceView.Imposition,
                        MaxDiscount = productPriceView.MaxDiscount,
                        UnitPrice = productPriceView.UnitPrice,
                        ProductCode = productPriceView.ProductPriceCode,
                        ProductType = "Product",
                        Note = productPriceView.Note

                    });
                }

            #endregion

            #region Credits

            IEnumerable<CreditServiceView> creditServiceViews;

            Guid customerID = CustomerID == null ? Guid.Empty : (Guid)CustomerID;

            Guid networkID = GetCustomer(customerID.ToString()).NetworkID;
            //Edited By Zafari
            //Orginal  IEnumerable<CreditServiceView> creditServiceViews = _creditServiceService.GetCreditServices().CreditServiceViews;
            if (customerID == Guid.Empty)
            {
                creditServiceViews = _creditServiceService.GetCreditServices().CreditServiceViews.Where(x => !x.Discontinued);
            }
            else
            {

                creditServiceViews = _creditServiceService.GetCreditServices().CreditServiceViews.Where(x => !x.Discontinued).Where(x => x.NetworkID == networkID);


            }
            if (creditServiceViews != null)
                foreach (CreditServiceView creditServiceView in creditServiceViews)
                {
                    productions.Add(new Production
                    {
                        SaleDetailID = creditServiceView.ID,
                        SaleDetailName = creditServiceView.ServiceName + " - " + string.Format("{0:#,###,###.###}", creditServiceView.UnitPrice),
                        Imposition = creditServiceView.Imposition,
                        MaxDiscount = creditServiceView.MaxDiscount,
                        UnitPrice = creditServiceView.UnitPrice,
                        ProductCode = creditServiceView.CreditServiceCode,
                        ProductType = "CreditService",
                        Note = creditServiceView.Note

                    });
                }

            #endregion

            #region Uncredits
            //Edited By Zafari
            //Orginal IEnumerable<UncreditServiceView> uncreditServiceViews = _uncreditServiceService.GetUncreditServices().UncreditServiceViews;
            IEnumerable<UncreditServiceView> uncreditServiceViews = _uncreditServiceService.GetUncreditServices().UncreditServiceViews.Where(x => !x.Discontinued);
            if (uncreditServiceViews != null)
                foreach (UncreditServiceView uncreditServiceView in uncreditServiceViews)
                {
                    productions.Add(new Production
                    {
                        SaleDetailID = uncreditServiceView.ID,
                        SaleDetailName = uncreditServiceView.UncreditServiceName + " - " + string.Format("{0:#,###,###.###}", uncreditServiceView.UnitPrice),
                        Imposition = uncreditServiceView.Imposition,
                        MaxDiscount = uncreditServiceView.MaxDiscount,
                        UnitPrice = uncreditServiceView.UnitPrice,
                        ProductCode = uncreditServiceView.UnCreditServiceCode,
                        ProductType = "UnCreditService",
                        Note = uncreditServiceView.Note,
                    });
                }

            #endregion

            return Json(productions, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Get One Production Json

        public JsonResult GetOneProduction(Guid detailID)
        {
            Production production = new Production();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Product_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(production, JsonRequestBehavior.AllowGet);
            }
            #endregion


            GetRequest getRequest = new GetRequest() { ID = detailID };

            ProductPriceView productPriceView = _productPriceService.GetProductPrice(getRequest).ProductPriceView;

            if (productPriceView.ID != Guid.Empty)
            {
                production.SaleDetailName = productPriceView.ProductPriceTitle;
                production.MaxDiscount = productPriceView.MaxDiscount;
                production.Imposition = productPriceView.Imposition;
                production.UnitPrice = productPriceView.UnitPrice;
            }
            else
            {
                CreditServiceView creditServiceView = _creditServiceService.GetCreditService(getRequest).CreditServiceView;

                if (creditServiceView.ID != Guid.Empty)
                {
                    production.SaleDetailName = creditServiceView.ServiceName;
                    production.MaxDiscount = creditServiceView.MaxDiscount;
                    production.Imposition = creditServiceView.Imposition;
                    production.UnitPrice = creditServiceView.UnitPrice;
                }
                else
                {
                    UncreditServiceView uncreditServiceView = _uncreditServiceService.GetUncreditService(getRequest).UncreditServiceView;

                    production.SaleDetailName = uncreditServiceView.UncreditServiceName;
                    production.MaxDiscount = uncreditServiceView.MaxDiscount;
                    production.Imposition = uncreditServiceView.Imposition;
                    production.UnitPrice = uncreditServiceView.UnitPrice;
                }
            }

            return Json(production, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Old

        #region Create

        public ActionResult Create(string id)
        {
            SalePageView salePageView = new SalePageView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Sale_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(salePageView);
            }
            #endregion

            PopulateDetailList();


            salePageView.EmployeeView = GetEmployee();

            salePageView.ClientSaleDetailViewModels = new List<ClientSaleDetailViewModel>();

            // Filling Customer properties:
            salePageView.CustomerView = GetCustomer(id);

            return View(salePageView);
        }
        #endregion

        #region Create2

        public ActionResult Create2(string id)
        {
            SalePageView salePageView = new SalePageView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Sale_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(salePageView);
            }
            #endregion

            PopulateDetailList();

            salePageView.EmployeeView = GetEmployee();

            salePageView.ClientSaleDetailViewModels = new List<ClientSaleDetailViewModel>();

            // Filling Customer properties:
            salePageView.CustomerView = GetCustomer(id);

            return View(salePageView);
        }

        #endregion

        #region Create

        [HttpPost]
        public ActionResult Create(SaleDetailView saleDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SaleDetail_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(saleDetailView);
            }
            #endregion

            #region Customer Info

            saleDetailView.CustomerView = GetCustomer(saleDetailView.SaleView.CustomerID);

            #endregion

            saleDetailView.EmployeeView = GetEmployee();

            if (ModelState.IsValid)
                try
                {
                    AddSaleRequest request = new AddSaleRequest();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.CustomerID = saleDetailView.SaleView.CustomerID;
                    request.SaleNumber = saleDetailView.SaleView.SaleNumber;
                    //request.MainSaleID = saleDetailView.SaleView.MainSaleID;

                    #region Using Sessions

                    if (Session["addProductSaleDetailRequests"] != null)
                    {
                        IEnumerable<AddProductSaleDetailRequest> addProductSaleDetailRequests = (IEnumerable<AddProductSaleDetailRequest>)Session["addProductSaleDetailRequests"];
                        request.AddProductSaleDetailRequests = addProductSaleDetailRequests;
                    }
                    if (Session["addCreditSaleDetailRequests"] != null)
                    {
                        IEnumerable<AddCreditSaleDetailRequest> addCreditSaleDetailRequests = (IEnumerable<AddCreditSaleDetailRequest>)Session["addCreditSaleDetailRequests"];
                        request.AddCreditSaleDetailRequests = addCreditSaleDetailRequests;
                    }
                    if (Session["addUncreditSaleDetailRequests"] != null)
                    {
                        IEnumerable<AddUncreditSaleDetailRequest> addUncreditSaleDetailRequests = (IEnumerable<AddUncreditSaleDetailRequest>)Session["addUncreditSaleDetailRequests"];
                        request.AddUncreditSaleDetailRequests = addUncreditSaleDetailRequests;
                    }

                    #endregion

                    GeneralResponse response = this._saleService.AddSale(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(saleDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(saleDetailView);
                }

            return View(saleDetailView);
        }

        #endregion

        #region Edit

        public ActionResult Edit(string id)
        {
            SaleDetailView saleDetailView = new SaleDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SaleDetail_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(saleDetailView);
            }
            #endregion

            saleDetailView.SaleView = this.GetSaleView(id);
            saleDetailView.EmployeeView = GetEmployee();

            return View(saleDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, SaleDetailView saleDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SaleDetail_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(saleDetailView);
            }
            #endregion


            saleDetailView.EmployeeView = GetEmployee();

            if (ModelState.IsValid)
                try
                {
                    EditSaleRequest request = new EditSaleRequest();

                    request.ID = Guid.Parse(id);
                    request.CustomerID = saleDetailView.SaleView.CustomerID;
                    request.SaleNumber = saleDetailView.SaleView.SaleNumber;
                    request.MainSaleID = saleDetailView.SaleView.MainSaleID;
                    request.RowVersion = saleDetailView.SaleView.RowVersion;

                    GeneralResponse response = this._saleService.EditSale(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(saleDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(saleDetailView);
                }

            return View(saleDetailView);
        }

        #endregion

        #region Details

        public ActionResult Details(string id)
        {
            SaleDetailView saleDetailView = new SaleDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SaleDetail_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(saleDetailView);
            }
            #endregion


            SaleView saleView = this.GetSaleView(id);

            saleDetailView.SaleView = saleView;
            saleDetailView.EmployeeView = GetEmployee();

            return View(saleDetailView);
        }

        #endregion

        #endregion

        #region CloseSale

        public JsonResult CloseSale(string saleID, int RowVersion)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("CloseSale");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            CloseSaleRequest request = new CloseSaleRequest();
            request.SaleID = Guid.Parse(saleID);
            request.CloseEmployeeID = GetEmployee().ID;
            request.RowVersion = RowVersion;

            response = _saleService.CloseSale(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get Un delivered Products fo Customer

        public JsonResult UnDeliveredProducts_Read(Guid CustomerID)
        {
            GetGeneralResponse<IEnumerable<ProductSaleDetailView>> response = new GetGeneralResponse<IEnumerable<ProductSaleDetailView>>();

            response = _saleService.GetUnDeliveredProducts(CustomerID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Members

        #region GetSaleView

        private SaleView GetSaleView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetSaleResponse response = this._saleService.GetSale(request);

            return response.SaleView;
        }

        #endregion

        #region PopulateDetailList

        private void PopulateDetailList()
        {
            IList<ClientSaleDetailViewModel> clientSaleDetailViewModels = new List<ClientSaleDetailViewModel>();

            #region Products

            IEnumerable<ProductPriceView> productPriceViews = _productPriceService.GetProductPrices().ProductPriceViews;

            if (productPriceViews != null)
                foreach (ProductPriceView productPriceView in productPriceViews)
                {
                    clientSaleDetailViewModels.Add(new ClientSaleDetailViewModel
                    {
                        SaleDetailID = productPriceView.ID,
                        SaleDetailName = productPriceView.ProductPriceTitle,
                        Imposition = productPriceView.Imposition,
                        MaxDiscount = productPriceView.MaxDiscount,
                        UnitPrice = productPriceView.UnitPrice
                    });
                }

            #endregion

            #region Credits

            IEnumerable<CreditServiceView> creditServiceViews = _creditServiceService.GetCreditServices().CreditServiceViews;

            if (creditServiceViews != null)
                foreach (CreditServiceView creditServiceView in creditServiceViews)
                {
                    clientSaleDetailViewModels.Add(new ClientSaleDetailViewModel
                    {
                        SaleDetailID = creditServiceView.ID,
                        SaleDetailName = creditServiceView.ServiceName,
                        Imposition = creditServiceView.Imposition,
                        MaxDiscount = creditServiceView.MaxDiscount,
                        UnitPrice = creditServiceView.UnitPrice
                    });
                }

            #endregion

            #region Uncredits

            IEnumerable<UncreditServiceView> uncreditServiceViews = _uncreditServiceService.GetUncreditServices().UncreditServiceViews;

            if (uncreditServiceViews != null)
                foreach (UncreditServiceView uncreditServiceView in uncreditServiceViews)
                {
                    clientSaleDetailViewModels.Add(new ClientSaleDetailViewModel
                    {
                        SaleDetailID = uncreditServiceView.ID,
                        SaleDetailName = uncreditServiceView.UncreditServiceName,
                        Imposition = uncreditServiceView.Imposition,
                        MaxDiscount = uncreditServiceView.MaxDiscount,
                        UnitPrice = uncreditServiceView.UnitPrice
                    });
                }

            #endregion

            #region DropDownList For Details

            List<DropDownItem> list = new List<DropDownItem>();

            foreach (ClientSaleDetailViewModel details in clientSaleDetailViewModels)
            {
                list.Add(new DropDownItem { Value = details.SaleDetailID, Text = details.SaleDetailName });
            }
            var selectList = new SelectList(list, "Value", "Text");

            ViewData["detailData"] = selectList;

            #endregion
        }

        #endregion

        #region Get Detail Type

        private char GetDetailType(Guid detailID)
        {
            GetRequest getRequest = new GetRequest() { ID = detailID };

            ProductPriceView productPriceView = _productPriceService.GetProductPrice(getRequest).ProductPriceView;

            if (productPriceView != null && productPriceView.ID != Guid.Empty)
            {
                return 'P';
            }
            else
            {
                CreditServiceView creditServiceView = _creditServiceService.GetCreditService(getRequest).CreditServiceView;

                if (creditServiceView != null && creditServiceView.ID != Guid.Empty)
                {
                    return 'C';
                }
                else
                {
                    UncreditServiceView uncreditServiceView = _uncreditServiceService.GetUncreditService(getRequest).UncreditServiceView;
                    if (uncreditServiceView != null && uncreditServiceView.ID != Guid.Empty)
                    {
                        return 'U';
                    }
                    else
                    {
                        return 'F';
                    }
                }
            }
        }

        #endregion

        #region Get Detail Type By RowID

        private char GetDetailTypeByRowID(Guid rowID)
        {
            GetRequest getRequest = new GetRequest() { ID = rowID };

            ProductSaleDetailView productSaleDetailView = _productSaleDetailService.GetProductSaleDetail(getRequest).ProductSaleDetailView;

            if (productSaleDetailView != null && productSaleDetailView.ID != Guid.Empty)
            {
                return 'P';
            }
            else
            {
                CreditSaleDetailView creditSaleDetailView = _creditSaleDetailService.GetCreditSaleDetail(getRequest).CreditSaleDetailView;

                if (creditSaleDetailView != null && creditSaleDetailView.ID != Guid.Empty)
                {
                    return 'C';
                }
                else
                {
                    UncreditSaleDetailView uncreditSaleDetailView = _uncreditSaleDetailService.GetUncreditSaleDetail(getRequest).UncreditSaleDetailView;

                    if (uncreditSaleDetailView != null && uncreditSaleDetailView.ID != Guid.Empty)
                    {
                        return 'U';
                    }
                    else
                    {
                        return 'F';
                    }
                }
            }
        }

        #endregion

        #region Get Private Production

        private Production GetProduction(Guid productionID)
        {
            Production production = new Production();
            GetRequest getRequest = new GetRequest() { ID = productionID };

            ProductPriceView productPriceView = _productPriceService.GetProductPrice(getRequest).ProductPriceView;

            if (productPriceView.ID != Guid.Empty)
            {
                production.SaleDetailName = productPriceView.ProductPriceTitle;
                production.MaxDiscount = productPriceView.MaxDiscount;
                production.Imposition = productPriceView.Imposition;
                production.UnitPrice = productPriceView.UnitPrice;
            }
            else
            {
                CreditServiceView creditServiceView = _creditServiceService.GetCreditService(getRequest).CreditServiceView;

                if (creditServiceView.ID != Guid.Empty)
                {
                    production.SaleDetailName = creditServiceView.ServiceName;
                    production.MaxDiscount = creditServiceView.MaxDiscount;
                    production.Imposition = creditServiceView.Imposition;
                    production.UnitPrice = creditServiceView.UnitPrice;
                }
                else
                {
                    UncreditServiceView uncreditServiceView = _uncreditServiceService.GetUncreditService(getRequest).UncreditServiceView;

                    production.SaleDetailName = uncreditServiceView.UncreditServiceName;
                    production.MaxDiscount = uncreditServiceView.MaxDiscount;
                    production.Imposition = uncreditServiceView.Imposition;
                    production.UnitPrice = uncreditServiceView.UnitPrice;
                }
            }

            return production;
        }

        #endregion

        #endregion

        #region Reporting
        /*
        public ActionResult GetFactorReport()
        {
            StiReport report = new StiReport();

            Guid saleID = Guid.Empty;
            if (Session["SaleID"] != null) saleID = Guid.Parse(Session["SaleID"].ToString());

            FactorView factorData = new FactorView();

            factorData.SaleView = _saleService.GetSale(saleID).data;

            #region Preparing factor SaleDetailViews

            IList<SaleDetailReportView> saleDetailViews = new List<SaleDetailReportView>();

            if (factorData.SaleView != null && factorData.SaleView.CreditSaleDetails != null)
                foreach (var saleDetail in factorData.SaleView.CreditSaleDetails)
                {
                    SaleDetailReportView saleDetailView = new SaleDetailReportView();

                    saleDetailView.Units = saleDetail.Units;
                    saleDetailView.Discount = saleDetail.Discount;
                    saleDetailView.UnitPrice = saleDetail.UnitPrice;
                    saleDetailView.SaleDetail = saleDetail.CreditServiceName;
                    saleDetailView.Imposition = saleDetail.Imposition;
                    saleDetailView.LineTotalWithoutDiscountAndImposition = saleDetail.Units * saleDetail.UnitPrice;

                    saleDetailViews.Add(saleDetailView);
                }

            if (factorData.SaleView != null && factorData.SaleView.UncreditSaleDetails != null)
                foreach (var saleDetail in factorData.SaleView.UncreditSaleDetails)
                {
                    SaleDetailReportView saleDetailView = new SaleDetailReportView();

                    saleDetailView.Units = saleDetail.Units;
                    saleDetailView.Discount = saleDetail.Discount;
                    saleDetailView.UnitPrice = saleDetail.UnitPrice;
                    saleDetailView.SaleDetail = saleDetail.UncreditServiceName;
                    saleDetailView.Imposition = saleDetail.Imposition;
                    saleDetailView.LineTotalWithoutDiscountAndImposition = saleDetail.Units * saleDetail.UnitPrice;

                    saleDetailViews.Add(saleDetailView);
                }

            if (factorData.SaleView != null && factorData.SaleView.ProductSaleDetails != null)
                foreach (var saleDetail in factorData.SaleView.ProductSaleDetails)
                {
                    SaleDetailReportView saleDetailView = new SaleDetailReportView();

                    saleDetailView.Units = saleDetail.Units;
                    saleDetailView.Discount = saleDetail.Discount;
                    saleDetailView.UnitPrice = saleDetail.UnitPrice;
                    saleDetailView.SaleDetail = saleDetail.ProductPriceTitle;
                    saleDetailView.Imposition = saleDetail.Imposition;
                    saleDetailView.LineTotalWithoutDiscountAndImposition = saleDetail.Units * saleDetail.UnitPrice;

                    saleDetailViews.Add(saleDetailView);
                }

            factorData.SaleDetailViews = saleDetailViews;

            #endregion

            //factorData.CustomerView = factorData.SaleView.Customer;

            report.Load(Server.MapPath("~/Content/Reports/FactorReport.mrt"));
            //report.RegData("Customer", factorData.CustomerView);
            //report.RegData("Sale", factorData.SaleView);
            report.RegData("SaleDetails", factorData.SaleDetailViews);

            return StiMvcViewerFxHelper.GetReportSnapshotResult(report);
        }

        public ActionResult FactorReport(Guid SaleID)
        {
            Session["SaleID"] = SaleID;

            return View();
        }

        public FileResult ExportReport()
        {
            // Return the exported report file
            return StiMvcViewerFxHelper.ExportReportResult(this.Request);
        }
        */
        #endregion



        public JsonResult Bonus()
        {
            GeneralResponse response = new GeneralResponse();
            response = _saleService.Bonus();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}
