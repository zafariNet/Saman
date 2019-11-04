using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Store.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.StoreCatalogService;
using Model.Store;
using Services.ViewModels.Store;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees.Interfaces;
using Infrastructure.Querying;
using Infrastructure.Domain;
using Services;
namespace Services.Implementations
{
    public class ProductLogService : IProductLogService
    {
        #region Declares
        private readonly IProductLogRepository _productLogRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IStoreRepository _storRepository;
        private readonly IStoreProductRepository _storeProductrepository;

        private readonly IStoreProductService _storproductService;
        #endregion

        #region Ctor
        public ProductLogService(IProductLogRepository productLogRepository, IUnitOfWork uow)
        {
            _productLogRepository = productLogRepository;
            _uow = uow;
        }

        public ProductLogService(IProductLogRepository productLogRepository, IProductRepository productRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository, IStoreRepository storeRepository, IStoreProductRepository storeProductrepository, IStoreProductService storproductService)
            : this(productLogRepository, uow)
        {
            this._productRepository = productRepository;
            _employeeRepository = employeeRepository;
            _storRepository = storeRepository;
            _storeProductrepository = storeProductrepository;
            _storproductService = storproductService;
        }
        #endregion

        #region Old Methods

        #region Add
        public GeneralResponse AddProductLog(AddProductLogRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                ProductLog productLog = new ProductLog();
                
                productLog.ID = Guid.NewGuid();
                productLog.CreateDate = PersianDateTime.Now;
                productLog.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                productLog.Note = request.Note;
                productLog.Closed = request.Closed;
                productLog.InputSerialNumber = request.InputSerialNumber;
                productLog.LogDate = request.LogDate;
                productLog.Product = this._productRepository.FindBy(request.ProductID);
                productLog.ProductSerialFrom = request.ProductSerialFrom;
                productLog.ProductSerialTo = request.ProductSerialTo;
                productLog.PurchaseBillNumber = request.PurchaseBillNumber;
                productLog.PurchaseDate = request.PurchaseDate;
                productLog.PurchaseUnitPrice = request.PurchaseUnitPrice;
                productLog.SellerName = request.SellerName;
                productLog.UnitsIO = request.UnitsIO;
                productLog.RowVersion = 1;

                #region Validation

                if (productLog.GetBrokenRules().Count() > 0)
                {


                    foreach (BusinessRule businessRule in productLog.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                Product product = productLog.Product;
                product.UnitsInStock += productLog.UnitsIO;
                _productRepository.Save(product);

                _productLogRepository.Add(productLog);
                _uow.Commit();
            }
            catch (Exception ex)
            {

                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        #endregion

        #region Edit
        public GeneralResponse EditProductLog(EditProductLogRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            ProductLog productLog = new ProductLog();
            productLog = _productLogRepository.FindBy(request.ID);

            if (productLog != null)
            {
                try
                {
                    productLog.ModifiedDate = PersianDateTime.Now;
                    productLog.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if (request.Note != null)
                        productLog.Note = request.Note;
                    productLog.Closed = request.Closed;
                    if (request.InputSerialNumber != null)
                        productLog.InputSerialNumber = request.InputSerialNumber;
                    if (request.LogDate != null)
                        productLog.LogDate = request.LogDate;
                    if (request.ProductID != null)
                        productLog.Product = this._productRepository.FindBy(request.ProductID);
                    if (request.ProductSerialFrom != null)
                        productLog.ProductSerialFrom = request.ProductSerialFrom;
                    if (request.ProductSerialTo != null)
                        productLog.ProductSerialTo = request.ProductSerialTo;
                    if (request.PurchaseBillNumber != null)
                        productLog.PurchaseBillNumber = request.PurchaseBillNumber;
                    if (request.PurchaseDate != null)
                        productLog.PurchaseDate = request.PurchaseDate;
                    productLog.PurchaseUnitPrice = request.PurchaseUnitPrice;
                    if (request.SellerName != null)
                        productLog.SellerName = request.SellerName;

                    // بدست آوردن اختلاف تعداد
                    Product product = productLog.Product;
                    if (productLog.UnitsIO != request.UnitsIO)
                    {
                        product.UnitsInStock += request.UnitsIO - productLog.UnitsIO;
                        _productRepository.Save(product);
                    }

                    productLog.UnitsIO = request.UnitsIO;

                    #region RowVersion

                    if (productLog.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        productLog.RowVersion += 1;
                    }

                    #endregion

                    #region Validation

                    if (productLog.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in productLog.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _productLogRepository.Save(productLog);
                    _uow.Commit();
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

        #region Delete
        public GeneralResponse DeleteProductLog(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            ProductLog productLog = new ProductLog();
            productLog = _productLogRepository.FindBy(request.ID);

            if (productLog != null)
            {
                try
                {
                    _productLogRepository.Remove(productLog);
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

        #region Get One
        public GetProductLogResponse GetProductLog(GetRequest request)
        {
            GetProductLogResponse response = new GetProductLogResponse();

            try
            {
                ProductLog productLog = new ProductLog();
                ProductLogView productLogView = productLog.ConvertToProductLogView();

                productLog = _productLogRepository.FindBy(request.ID);
                if (productLog != null)
                    productLogView = productLog.ConvertToProductLogView();

                response.ProductLogView = productLogView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get All
        public GetProductLogsResponse GetProductLogs()
        {
            GetProductLogsResponse response = new GetProductLogsResponse();

            try
            {
                IEnumerable<ProductLogView> productLogs = _productLogRepository.FindAll()
                    .ConvertToProductLogViews();

                response.ProductLogViews = productLogs;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetProductLogsResponse GetProductLogs(Guid productID)
        {
            GetProductLogsResponse response = new GetProductLogsResponse();

            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                Criterion criterion = new Criterion("Product.ID", productID, CriteriaOperator.Equal);
                query.Add(criterion);

                IEnumerable<ProductLogView> productLogs = _productLogRepository.FindBy(query)
                    .OrderByDescending(o => o.CreateDate)
                    .ConvertToProductLogViews();

                response.ProductLogViews = productLogs;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<ProductLogView>> GetProductLogsByFilter(IList<FilterData> filters)
        {
            GetGeneralResponse<IEnumerable<ProductLogView>> response=new GetGeneralResponse<IEnumerable<ProductLogView>>();

            string query = FilterUtilityService.GenerateFilterHQLQuery(filters, "ProductLog", null);
            Response<ProductLog> productLogViews = _productLogRepository.FindBy(query);

            response.data = productLogViews.data.ConvertToProductLogViews();
            response.totalCount = productLogViews.totalCount;

            return response;
        }

        public GetProductLogsResponse GetProductLogs(Guid productID,Guid StoreID)
        {
            GetProductLogsResponse response = new GetProductLogsResponse();

            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                Criterion criterion = new Criterion("Product.ID", productID, CriteriaOperator.Equal);
                query.Add(criterion);

                if (StoreID != Guid.Empty)
                {
                    Criterion criterionStoreID = new Criterion("Store.ID", StoreID, CriteriaOperator.Equal);
                    query.Add(criterionStoreID);
                }
                IEnumerable<ProductLogView> productLogs = _productLogRepository.FindBy(query)
                    .OrderByDescending(o => o.CreateDate)
                    .ConvertToProductLogViews();

                response.ProductLogViews = productLogs;
            }
            catch (Exception ex)
            {

            }

            return response;
        }


        #endregion

        #endregion

        #region New Methods

        #region Read All

        public GetGeneralResponse<IEnumerable<ProductLogView>> GetProductLogs(int PageSize, int PageNumber)
        {
            GetGeneralResponse<IEnumerable<ProductLogView>> response = new GetGeneralResponse<IEnumerable<ProductLogView>>();
            try
            {
                int index = (PageNumber - 1) * PageSize;
                int count = PageSize;

                Response<ProductLog> productLoges = _productLogRepository.FindAll(index, count);

                response.data = productLoges.data.ConvertToProductLogViews();
                response.totalCount = productLoges.totalCount;
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

        #region Read by Product


        public GetGeneralResponse<IEnumerable<ProductLogView>> GetProductLogsByProduct(Guid ProductID, int PageSize, int PageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<ProductLogView>> response = new GetGeneralResponse<IEnumerable<ProductLogView>>();
            try
            {
                int index = (PageNumber - 1) * PageSize;
                int count = PageSize;

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                Criterion criteriaproductID = new Criterion("Product.ID", ProductID, CriteriaOperator.Equal);
                query.Add(criteriaproductID);

                Response<ProductLog> productLoges = _productLogRepository.FindBy(query, index, count,sort);
                productLoges.data = productLoges.data.Where(x => x.Store == null);
                response.data = productLoges.data.ConvertToProductLogViews();
                response.totalCount = productLoges.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;

        }



        public GetGeneralResponse<IEnumerable<ProductLogView>> GetProductLogsByProductInStore(Guid ProductID, int PageSize, int PageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<ProductLogView>> response = new GetGeneralResponse<IEnumerable<ProductLogView>>();
            try
            {
                int index = (PageNumber - 1) * PageSize;
                int count = PageSize;

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                Criterion criteriaproductID = new Criterion("Product.ID", ProductID, CriteriaOperator.Equal);
                query.Add(criteriaproductID);


                Response<ProductLog> productLoges = _productLogRepository.FindBy(query, index, count,sort);
                productLoges.data = productLoges.data.Where(x => x.Store != null);
                response.data = productLoges.data.ConvertToProductLogViews();
                response.totalCount = productLoges.totalCount;
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

        #region Insert

        public GeneralResponse AddProductLog(AddProductLogRequest request, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                ProductLog productLog = new ProductLog();


                //if (request.IO == -1)
                //    productLog.UnitsIO = -request.DisplayUnitsIO;
                //else
                //    productLog.UnitsIO = request.DisplayUnitsIO;

                
                productLog.ID = Guid.NewGuid();

                productLog.CreateDate = PersianDateTime.Now;
                productLog.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                productLog.Note = request.Note;
                productLog.Closed = request.Closed;
                productLog.InputSerialNumber = GetNewSerialNumber;
                productLog.LogDate = request.LogDate;
                productLog.Product = this._productRepository.FindBy(request.ProductID);
                
                productLog.ProductSerialFrom = request.ProductSerialFrom;
                productLog.ProductSerialTo = request.ProductSerialTo;
                productLog.PurchaseBillNumber = request.PurchaseBillNumber;
                productLog.PurchaseDate = request.PurchaseDate;
                productLog.PurchaseUnitPrice = request.PurchaseUnitPrice;
                productLog.SellerName = request.SellerName;

                productLog.RowVersion = 1;


                #region Validation

                if (productLog.GetBrokenRules().Count() > 0)
                {


                    foreach (BusinessRule businessRule in productLog.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                Product product = productLog.Product;


                if (request.IO == -1)
                {
                    if (productLog.Product.UnitsInStock < request.DisplayUnitsIO)
                    {
                        response.ErrorMessages.Add("موجودی انبار کافی نیست");
                        return response;
                    }
                    else
                    {
                        product.UnitsInStock -= request.DisplayUnitsIO;
                        productLog.UnitsIO = -request.DisplayUnitsIO;
                    }
                }
                else
                {
                    product.UnitsInStock += request.DisplayUnitsIO;
                    productLog.UnitsIO = request.DisplayUnitsIO;
                }

                //product.UnitsInStock += productLog.UnitsIO;
                _productRepository.Save(product);



                _productLogRepository.Add(productLog);
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

        public GeneralResponse AddProductLogToStore(AddProductLogStoreRequest request, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            ProductLog productLog = new ProductLog();

            
            productLog.ID = Guid.NewGuid();

            productLog.CreateDate = PersianDateTime.Now;
            productLog.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
            productLog.Note = request.Note;
            productLog.Closed = request.Closed;
            productLog.InputSerialNumber = GetNewSerialNumber;
            productLog.LogDate = request.LogDate;
            productLog.Product = this._productRepository.FindBy(request.ProductID);

            productLog.ProductSerialFrom = request.ProductSerialFrom;
            productLog.ProductSerialTo = request.ProductSerialTo;
            productLog.PurchaseBillNumber = request.PurchaseBillNumber;
            productLog.PurchaseDate = request.PurchaseDate;
            productLog.PurchaseUnitPrice = request.PurchaseUnitPrice;
            productLog.SellerName = request.SellerName;

            productLog.RowVersion = 1;
            productLog.Store = _storRepository.FindBy(request.StoreID);

            Product product = productLog.Product;

            if (request.IO == -1)
            {
                
                productLog.UnitsIO = +request.DisplayUnitsIO;
            }
            else
            {
                
                productLog.UnitsIO = -request.DisplayUnitsIO;
            }



            #region If record exists, Edit UnitsInStock

            StoreProduct storeProduct = new StoreProduct();
            storeProduct = _storeProductrepository.FindBy(request.StoreID, request.ProductID);

            if (storeProduct != null)
            {

                storeProduct.ModifiedDate = PersianDateTime.Now;
                storeProduct.ModifiedEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                if (request.ProductID != null)
                    storeProduct.Product = this._productRepository.FindBy(request.ProductID);
                if (request.StoreID != null)
                    storeProduct.Store = this._storRepository.FindBy(request.StoreID);

                if (request.IO == -1)
                {

                    if (storeProduct.UnitsInStock < request.DisplayUnitsIO)
                    {
                        response.ErrorMessages.Add("موجودی انبار مجازی کافی نیست");
                        return response;
                    }
                    else
                    {
                        product.UnitsInStock += request.DisplayUnitsIO;
                        storeProduct.UnitsInStock -= request.DisplayUnitsIO;
                    }

                }
                if (request.IO == 1)
                {
                    if (productLog.Product.UnitsInStock < request.DisplayUnitsIO)
                    {
                        response.ErrorMessages.Add("موجودی انبار اصلی کافی نیست");
                        return response;

                    }
                    else
                    {
                        product.UnitsInStock -= request.DisplayUnitsIO;
                        storeProduct.UnitsInStock += request.DisplayUnitsIO;
                    }
                }
                storeProduct.RowVersion += 1;

                _storeProductrepository.Save(storeProduct);
            }

            else
            {
                StoreProduct _storeProduct = new StoreProduct();
                _storeProduct.ID = Guid.NewGuid();
                _storeProduct.CreateDate = PersianDateTime.Now;
                _storeProduct.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                _storeProduct.Product = this._productRepository.FindBy(request.ProductID);
                _storeProduct.Store = this._storRepository.FindBy(request.StoreID);
                _storeProduct.UnitsInStock = 0;
                if (request.IO == -1)
                {
                    if (storeProduct.UnitsInStock < request.DisplayUnitsIO)
                    {
                        response.ErrorMessages.Add("موجودی انبار مجازی کافی نیست");
                        return response;
                    }
                    else
                    {
                        product.UnitsInStock += request.DisplayUnitsIO;
                        _storeProduct.UnitsInStock -= request.DisplayUnitsIO;
                    }
                }
                else
                {
                    if (productLog.Product.UnitsInStock < request.DisplayUnitsIO)
                    {
                        response.ErrorMessages.Add("موجودی انبار اصلی کافی نیست");
                        return response;

                    }
                    else
                    {
                        product.UnitsInStock -= request.DisplayUnitsIO;
                        _storeProduct.UnitsInStock += request.DisplayUnitsIO;
                    }
                }
                _storeProduct.RowVersion = 1;

                _storeProductrepository.Add(_storeProduct);
            }


            _productRepository.Save(product);
            _productLogRepository.Add(productLog);
            #endregion

            _uow.Commit();

            return response;

        }

        #endregion

        #endregion



        #region Private Methods
        public string GetNewSerialNumber
        {
            get
            {
                try
                {
                    IList<Sort> inputSerialNumberSort = new List<Sort>();
                    inputSerialNumberSort.Add(new Sort("InputSerialNumber", false));

                    // Find Max Serial number (Only One Record)
                    Response<ProductLog> productLogs = _productLogRepository.FindAllWithSort(0, 1, inputSerialNumberSort);
                    // Max + 1
                    return (productLogs.data.Max(s => Convert.ToInt32(s.InputSerialNumber)) + 1).ToString();
                }
                catch
                {
                    return "1";
                }
            }
        }
        #endregion
    }
}
