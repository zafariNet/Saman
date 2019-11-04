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
namespace Services.Implementations
{
    public class StoreProductService : IStoreProductService
    {
        #region Declares
        private readonly IStoreProductRepository _storeProductRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Ctor
        public StoreProductService(IStoreProductRepository storeProductRepository, IUnitOfWork uow)
        {
            _storeProductRepository = storeProductRepository;
            _uow = uow;
        }

        public StoreProductService(IStoreProductRepository storeProductRepository, IStoreRepository storeRepository,
            IProductRepository productRepository, IUnitOfWork uow, IEmployeeRepository employeeRepository)
            : this(storeProductRepository, uow)
        {
            _storeRepository = storeRepository;
            _productRepository = productRepository;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Add
        public GeneralResponse AddStoreProduct(AddStoreProductRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                StoreProduct storeProduct = new StoreProduct();

                #region If record exists, Edit UnitsInStock
                if (_storeProductRepository.FindBy(request.StoreID, request.ProductID) != null)
                {
                    EditStoreProductRequest editreq = new EditStoreProductRequest()
                    {
                        ModifiedEmployeeID = request.CreateEmployeeID,
                        ProductID = request.ProductID,
                        StoreID = request.StoreID,
                        UnitsInStock = _storeProductRepository.FindBy(request.StoreID, request.ProductID).UnitsInStock + request.UnitsInStock,
                        RowVersion = _storeProductRepository.FindBy(request.StoreID, request.ProductID).RowVersion
                    };
                    GeneralResponse editRes = new GeneralResponse();
                    editRes = EditStoreProduct(editreq);

                    foreach (string err in editRes.ErrorMessages)
                        response.ErrorMessages.Add(err);
                    return response;
                }
                #endregion
                   
                storeProduct.ID = Guid.NewGuid();
                storeProduct.CreateDate = PersianDateTime.Now;
                storeProduct.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                storeProduct.Product = this._productRepository.FindBy(request.ProductID);
                storeProduct.Store = this._storeRepository.FindBy(request.StoreID);
                storeProduct.UnitsInStock = request.UnitsInStock;
                storeProduct.RowVersion = 1;

                #region Edit UnitsInStock Of Product Entity
                Product product = new Product();
                product = _productRepository.FindBy(request.ProductID);
                product.UnitsInStock -= request.UnitsInStock;
                if (product.UnitsInStock < 0)
                {
                    
                    response.ErrorMessages.Add("تعداد وارد شده از موجودی انبار مرکزی بیشتر است.");

                    return response;
                }
                else
                    _productRepository.Save(product);

                #endregion

                #region Validation
                if (storeProduct.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in storeProduct.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }
                #endregion

                _storeProductRepository.Add(storeProduct);
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
        public GeneralResponse EditStoreProduct(EditStoreProductRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            StoreProduct storeProduct = new StoreProduct();
            storeProduct = _storeProductRepository.FindBy(request.StoreID, request.ProductID);

            if (storeProduct != null)
            {
                try
                {
                    storeProduct.ModifiedDate = PersianDateTime.Now;
                    storeProduct.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if (request.ProductID != storeProduct.Product.ID)
                        storeProduct.Product = this._productRepository.FindBy(request.ProductID);
                    if (request.StoreID != storeProduct.Store.ID)
                        storeProduct.Store = this._storeRepository.FindBy(request.StoreID);

                    #region Edit UnitsInStock Of Product Entity
                    Product product = new Product();
                    product = _productRepository.FindBy(request.ProductID);
                    product.UnitsInStock -= request.UnitsInStock - storeProduct.UnitsInStock;
                    if (product.UnitsInStock < 0)
                    {

                        response.ErrorMessages.Add("تعداد وارد شده از موجودی انبار مرکزی بیشتر است.");

                        return response;
                    }
                    else
                        _productRepository.Save(product);
                    #endregion

                    storeProduct.UnitsInStock = request.UnitsInStock;

                    #region RowVersion Check
                    if (storeProduct.RowVersion != request.RowVersion)
                    {
                        
                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        storeProduct.RowVersion += 1;
                    }
                    #endregion

                    #region Validation
                    if (storeProduct.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in storeProduct.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }
                    #endregion

                    _storeProductRepository.Save(storeProduct);
                    _uow.Commit();
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

        #region Delete
        public GeneralResponse DeleteStoreProduct(Guid storeID, Guid productID)
        {
            GeneralResponse response = new GeneralResponse();

            StoreProduct storeProduct = new StoreProduct();
            storeProduct = _storeProductRepository.FindBy(storeID, productID);

            if (storeProduct != null)
            {
                try
                {
                    _storeProductRepository.Remove(storeProduct);
                    #region Edit UnitsInStock Of Product Entity
                    Product product = new Product();
                    product = _productRepository.FindBy(productID);
                    product.UnitsInStock += storeProduct.UnitsInStock;
                    _productRepository.Save(product);
                    #endregion
                    _uow.Commit();
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                }
            }

            return response;
        }
        #endregion

        #region Get one
        public GetStoreProductResponse GetStoreProduct(Guid storeID, Guid productID)
        {
            // if need must be configured correctly
            GetStoreProductResponse response = new GetStoreProductResponse();

            try
            {
                StoreProduct storeProduct = new StoreProduct();
                StoreProductView storeProductView = storeProduct.ConvertToStoreProductView();

                storeProduct = _storeProductRepository.FindBy(storeID, productID);
                if (storeProduct != null)
                    storeProductView = storeProduct.ConvertToStoreProductView();

                response.StoreProductView = storeProductView;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
        #endregion

        #region Get All
        public GetGeneralResponse<IEnumerable<StoreProductView>> GetStoreProducts(Guid storeID)
        {
            GetGeneralResponse<IEnumerable<StoreProductView>> response = new GetGeneralResponse<IEnumerable<StoreProductView>>();

            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                Criterion criterion = new Criterion("Store.ID", storeID, CriteriaOperator.Equal);
                query.Add(criterion);

                IEnumerable<StoreProductView> storeProducts = _storeProductRepository.FindBy(query)
                    .ConvertToStoreProductViews();

                response.data = storeProducts;
                response.totalCount = storeProducts.Count();
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<StoreProductView>> GetProductStore(Guid ProductID)
        {
            GetGeneralResponse<IEnumerable<StoreProductView>> response = new GetGeneralResponse<IEnumerable<StoreProductView>>();

            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                Criterion criterion = new Criterion("Product.ID", ProductID, CriteriaOperator.Equal);
                query.Add(criterion);

                IEnumerable<StoreProductView> storeProducts = _storeProductRepository.FindBy(query)
                    .ConvertToStoreProductViews();

                response.data = storeProducts;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion

    }
}
