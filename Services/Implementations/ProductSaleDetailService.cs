#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Sales.Interfaces;
using Model.Store.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.SaleCatalogService;
using Model.Sales;
using Services.ViewModels.Sales;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees.Interfaces;
using Infrastructure.Querying;
using Services.ViewModels.Reports;
using Infrastructure.Domain;

#endregion

namespace Services.Implementations
{
    public class ProductSaleDetailService : IProductSaleDetailService
    {
        #region Declares

        private readonly IProductSaleDetailRepository _productSaleDetailRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;

        #endregion

        #region Ctor

        public ProductSaleDetailService(IProductSaleDetailRepository productSaleDetailRepository, IUnitOfWork uow)
        {
            _productSaleDetailRepository = productSaleDetailRepository;
            _uow = uow;
        }

        public ProductSaleDetailService(IProductSaleDetailRepository productSaleDetailRepository, IProductRepository productRepository,
            ISaleRepository saleRepository, IUnitOfWork uow, IEmployeeRepository employeeRepository)
            : this(productSaleDetailRepository, uow)
        {
            this._productRepository = productRepository;
            this._saleRepository = saleRepository;
            _employeeRepository = employeeRepository;
        }

        #endregion

        #region Old

        public GeneralResponse AddProductSaleDetail(AddProductSaleDetailRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            //try
            //{
            //    ProductSaleDetail productSaleDetail = new ProductSaleDetail();
            //    productSaleDetail.ID = Guid.NewGuid();
            //    productSaleDetail.CreateDate = PersianDateTime.Now;
            //    productSaleDetail.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
            //    productSaleDetail.Discount = request.Discount;
            //    productSaleDetail.Imposition = request.Imposition;
            //    //productSaleDetail.ProductPrice = this._productRepository.FindBy(request.ProductID);
            //    productSaleDetail.RollbackNote = request.RollbackNote;
            //    productSaleDetail.Sale = this._saleRepository.FindBy(request.SaleID);
            //    productSaleDetail.UnitPrice = request.UnitPrice;
            //    productSaleDetail.Units = request.Units;
            //    productSaleDetail.RowVersion = 1;

            //    _productSaleDetailRepository.Add(productSaleDetail);
            //    _uow.Commit();

            //    response.hasCenter = true;

            //    // Validation
            //    if (productSaleDetail.GetBrokenRules().Count() > 0)
            //    {
            //        response.hasCenter = false;

            //        foreach (BusinessRule businessRule in productSaleDetail.GetBrokenRules())
            //        {
            //            response.ErrorMessages.Add(businessRule.Rule);
            //        }

            //        return response;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    response.hasCenter = false;
            //    response.ErrorMessages.Add(ex.Message);
            //}

            return response;
        }

        public GeneralResponse EditProductSaleDetail(EditProductSaleDetailRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            //    ProductSaleDetail productSaleDetail = new ProductSaleDetail();
            //    productSaleDetail = _productSaleDetailRepository.FindBy(request.ID);

            //    if (productSaleDetail != null)
            //    {
            //        try
            //        {
            //            productSaleDetail.ModifiedDate = PersianDateTime.Now;
            //            productSaleDetail.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
            //            productSaleDetail.Discount = request.Discount;
            //            productSaleDetail.Imposition = request.Imposition;
            //            //productSaleDetail.ProductPrice = this._productRepository.FindBy(request.ProductID);
            //            productSaleDetail.RollbackNote = request.RollbackNote;
            //            productSaleDetail.Sale = this._saleRepository.FindBy(request.SaleID);
            //            productSaleDetail.UnitPrice = request.UnitPrice;
            //            productSaleDetail.Units = request.Units;

            //            if (productSaleDetail.RowVersion != request.RowVersion)
            //            {
            //                response.hasCenter = false;
            //                response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
            //                return response;
            //            }
            //            else
            //            {
            //                productSaleDetail.RowVersion += 1;
            //            }

            //            if (productSaleDetail.GetBrokenRules().Count() > 0)
            //            {
            //                response.hasCenter = false;
            //                foreach (BusinessRule businessRule in productSaleDetail.GetBrokenRules())
            //                {
            //                    response.ErrorMessages.Add(businessRule.Rule);
            //                }

            //            return response;
            //        }

            //        _productSaleDetailRepository.Save(productSaleDetail);
            //        _uow.Commit();

            //        response.hasCenter = true;
            //    }
            //    catch (Exception ex)
            //    {
            //        response.hasCenter = false;
            //        response.ErrorMessages.Add(ex.Message);
            //    }
            //}
            //else
            //{
            //    response.hasCenter = false;
            //    response.ErrorMessages.Add("هیچ موردی جهت ویرایش وجود ندارد.");
            //}
            return response;
        }

        public GeneralResponse DeleteProductSaleDetail(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            //ProductSaleDetail productSaleDetail = new ProductSaleDetail();
            //productSaleDetail = _productSaleDetailRepository.FindBy(request.ID);

            //if (productSaleDetail != null)
            //{
            //    try
            //    {
            //        _productSaleDetailRepository.Remove(productSaleDetail);
            //        _uow.Commit();

            //        response.hasCenter = true;
            //    }
            //    catch (Exception ex)
            //    {
            //        response.hasCenter = false;
            //        response.ErrorMessages.Add(ex.Message);
            //    }
            //}

            return response;
        }

        public GetProductSaleDetailResponse GetProductSaleDetail(GetRequest request)
        {
            GetProductSaleDetailResponse response = new GetProductSaleDetailResponse();

            try
            {
                ProductSaleDetail productSaleDetail = new ProductSaleDetail();
                //ProductSaleDetailView productSaleDetailView = new ProductSaleDetailView();

                productSaleDetail = _productSaleDetailRepository.FindBy(request.ID);
                if (productSaleDetail != null)
                {
                    ProductSaleDetailView productSaleDetailView = productSaleDetail.ConvertToProductSaleDetailView();

                    response.ProductSaleDetailView = productSaleDetailView;
                }
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetProductSaleDetailsResponse GetProductSaleDetails()
        {
            GetProductSaleDetailsResponse response = new GetProductSaleDetailsResponse();

            try
            {
                IEnumerable<ProductSaleDetailView> productSaleDetails = _productSaleDetailRepository.FindAll()
                    .ConvertToProductSaleDetailViews();

                response.ProductSaleDetailViews = productSaleDetails;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        #endregion

        public GetGeneralResponse<IEnumerable<ProductSaleDetailView>> GetProductSaleDetails_ByProductID(Guid productID)
        {
            GetGeneralResponse<IEnumerable<ProductSaleDetailView>> response = new GetGeneralResponse<IEnumerable<ProductSaleDetailView>>();

            try
            {
                string hqlQuery = String.Format(@"
                    Select sd
                    From ProductSaleDetail sd 
                        Join sd.ProductPrice pp 
                        Join pp.Product p
                    Where p.ID = '{0}'", productID);

                IEnumerable<ProductSaleDetail> productSaleDetails = _productSaleDetailRepository.FindBy(hqlQuery).data;

                if (productSaleDetails != null)
                {
                    IEnumerable<ProductSaleDetailView> productSaleDetailView = productSaleDetails.ConvertToProductSaleDetailViews();
                    response.data = productSaleDetailView;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<ProductSaleDetailView>> GetProductSaleDetails(IList<FilterData> filters)
        {
            GetGeneralResponse<IEnumerable<ProductSaleDetailView>> response = new GetGeneralResponse<IEnumerable<ProductSaleDetailView>>();
            try
            {
                string query = FilterUtilityService.GenerateFilterHQLQuery(filters, "ProductSaleDetail", null);
                Response<ProductSaleDetail> productSaleDetails = _productSaleDetailRepository.FindBy(query);

                response.data = productSaleDetails.data.ConvertToProductSaleDetailViews();
                response.totalCount = productSaleDetails.totalCount;
            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        #region Sale Detail Report

        public GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> GetSaleReport(SaleReportRequest request)
        {
            GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> response = new GetGeneralResponse<IEnumerable<GetSaleDetailReportView>>();

            IQueryable<ProductSaleDetail> query = _productSaleDetailRepository.Queryable();

            if (request.SaleEmployeeID != null)
                query = query.Where(x => request.SaleEmployeeID.Contains(x.Sale.CreateEmployee.ID));

            if (request.DeliverEmployeeID != null)
                query = query.Where(x => request.DeliverEmployeeID.Contains(x.DeliverEmployee.ID));

            if (request.RollBackEmployeeID != null)
                query = query.Where(x => request.RollBackEmployeeID.Contains(x.CreateEmployee.ID)).Where(x=>x.IsRollbackDetail);

            if (request.Products != null)
            {
                query = query.Where(x => request.Products.Contains(x.ProductPrice.Product.ID));
            }
            if (request.ProductPrices != null)
            {
                query = query.Where(x => request.Products.Contains(x.ProductPrice.ID));
            }
            if (request.RollBackStartDate != null)
            {

            }
            if (request.RollBackEndDate != null)
            {

            }
            if (request.Deliverd != null)
            {
                query = query.Where(x => x.Delivered == request.Deliverd).Where(x=>x.IsRollbackDetail==false);
            }
            if (request.Confirmed != null)
            {
                query = query.Where(x => x.Sale.Closed == request.Confirmed);
            }
            if (request.RollBacked != null)
            {
                query = query.Where(x => x.Rollbacked == request.RollBacked);
            }

            

            IEnumerable<ProductSaleDetail> productSaleDetail = query.ToList();

            IList<GetSaleDetailReportView> Report = new List<GetSaleDetailReportView>();
            foreach (ProductSaleDetail _productSaleDetail in productSaleDetail)
            {
                GetSaleDetailReportView item = new GetSaleDetailReportView();

                item.ADSLPhone = _productSaleDetail.Sale.Customer.ADSLPhone;
                item.CenterName = _productSaleDetail.Sale.Customer.Center == null ? "" : _productSaleDetail.Sale.Customer.Center.CenterName;
                item.DeliverDate = _productSaleDetail.DeliverDate;
                item.DeliverEmployeeName = _productSaleDetail.DeliverEmployee == null ? "" : (string)_productSaleDetail.DeliverEmployee.Name;
                item.Discount = _productSaleDetail.LineDiscount;
                item.Imposition = _productSaleDetail.LineImposition;
                item.Name = _productSaleDetail.Sale.Customer.Name;
                item.Price = _productSaleDetail.UnitPrice;
                item.Count = _productSaleDetail.Units;
                item.ProductName = _productSaleDetail.ProductPrice.Product.ProductName;
                item.ProductPriceName = _productSaleDetail.ProductPrice.ProductPriceTitle;
                item.RollBackEmployeeName = _productSaleDetail.ModifiedEmployee == null ? "" : (string)_productSaleDetail.ModifiedEmployee.Name;
                item.RollBackPrice = _productSaleDetail.RollbackPrice;
                item.RoolBackDate = _productSaleDetail.ModifiedDate;
                item.SaleDate = _productSaleDetail.Sale.CloseDate;
                item.Total = _productSaleDetail.LineTotal;
                item.TotalRollBack = _productSaleDetail.Units;
                item.SaleDate = _productSaleDetail.Sale.CreateDate;
                item.SaleEmployeeName = _productSaleDetail.CreateEmployee.Name;


                Report.Add(item);
            }

            response.data = Report;
            response.totalCount = Report.Count();

            return response;
        }

        public GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> GetSaleReport(IList<FilterData> filters)
        {
            GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> response =
                new GetGeneralResponse<IEnumerable<GetSaleDetailReportView>>();

            string query = FilterUtilityService.GenerateFilterHQLQuery(filters, "ProductSaleDetail", null);

            Response<ProductSaleDetail> productSaleDetails = _productSaleDetailRepository.FindAll(query);

            IList<GetSaleDetailReportView> Report = new List<GetSaleDetailReportView>();

            foreach (ProductSaleDetail _productSaleDetail in productSaleDetails.data)
            {
                GetSaleDetailReportView item = new GetSaleDetailReportView();
                item.ADSLPhone = _productSaleDetail.Sale.Customer.ADSLPhone;
                item.CenterName = _productSaleDetail.Sale.Customer.Center == null
                    ? ""
                    : _productSaleDetail.Sale.Customer.Center.CenterName;
                item.Name = _productSaleDetail.Sale.Customer.Name;
                item.ProductPriceName = _productSaleDetail.ProductPrice.ProductPriceTitle;
                item.ProductName = _productSaleDetail.ProductPrice.Product.ProductName;
                item.BonusDate = _productSaleDetail.BonusDate;
                item.ComissionDate = _productSaleDetail.ComissionDate;

                if (_productSaleDetail.IsRollbackDetail)
                {
                    item.Bonus = _productSaleDetail.Bonus;
                    item.Comission = _productSaleDetail.Comission;

                    item.DeliverDate = _productSaleDetail.MainSaleDetail.DeliverDate;
                    item.DeliverEmployeeName = _productSaleDetail.MainSaleDetail.DeliverEmployee == null
                        ? ""
                        : (string) _productSaleDetail.MainSaleDetail.DeliverEmployee.Name;
                    item.Discount = _productSaleDetail.MainSaleDetail.LineDiscount;
                    item.Imposition = _productSaleDetail.MainSaleDetail.LineImposition;
                    item.Price = _productSaleDetail.MainSaleDetail.UnitPrice;
                    item.Count = _productSaleDetail.MainSaleDetail.Units;
                    item.RollBackEmployeeName = _productSaleDetail.CreateEmployee.Name;
                    item.RollBackPrice = _productSaleDetail.RollbackPrice;
                    item.RoolBackDate = _productSaleDetail.CreateDate;
                    item.SaleDate = _productSaleDetail.MainSaleDetail.CreateDate;
                    item.Total = _productSaleDetail.LineTotal;
                    item.TotalRollBack = _productSaleDetail.Units;
                    item.SaleEmployeeName = _productSaleDetail.MainSaleDetail.CreateEmployee.Name;
                }
                else
                {
                    item.Bonus = _productSaleDetail.Bonus;
                    item.Comission = _productSaleDetail.Comission;

                    item.DeliverDate = _productSaleDetail.DeliverDate;
                    item.DeliverEmployeeName = _productSaleDetail.DeliverEmployee == null
                        ? ""
                        : (string) _productSaleDetail.DeliverEmployee.Name;
                    item.Discount = _productSaleDetail.LineDiscount;
                    item.Imposition = _productSaleDetail.LineImposition;
                    item.Price = _productSaleDetail.UnitPrice;
                    item.Count = _productSaleDetail.Units;
                    item.SaleDate = _productSaleDetail.CreateDate;
                    item.Total = _productSaleDetail.LineTotal;
                    item.SaleDate = _productSaleDetail.Sale.CreateDate;
                    item.SaleEmployeeName = _productSaleDetail.CreateEmployee.Name;
                    item.CustomerID = _productSaleDetail.Sale.Customer.ID;

                    if (_productSaleDetail.Rollbacked)
                    {
                        Infrastructure.Querying.Query q = new Query();
                        Criterion crt = new Criterion("MainSaleDetail.ID", _productSaleDetail.ID, CriteriaOperator.Equal);
                        q.Add(crt);
                        ProductSaleDetail RollbakedProductSaleDetail = _productSaleDetailRepository.FindBy(q).FirstOrDefault();
                        if (RollbakedProductSaleDetail != null)
                        {
                            item.RollBackEmployeeName = RollbakedProductSaleDetail.CreateEmployee.Name;
                            item.RollBackPrice = RollbakedProductSaleDetail.LineTotal;
                            item.RoolBackDate = RollbakedProductSaleDetail.CreateDate;
                            item.TotalRollBack = RollbakedProductSaleDetail.Units;

                        }
                    }
                }
                Report.Add(item);
            }
            response.data = Report;
            response.totalCount = Report.Count();
            return response;

        }
        #endregion
    }
}
