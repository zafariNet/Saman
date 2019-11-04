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
    public class ProductService : IProductService
    {
        #region Declares
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Ctor
        public ProductService(IProductRepository productRepository, IUnitOfWork uow)
        {
            _productRepository = productRepository;
            _uow = uow;
        }

        public ProductService(IProductRepository productRepository,
            IProductCategoryRepository productCategoryRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository)
            : this(productRepository, uow)
        {
            this._productCategoryRepository = productCategoryRepository;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Add
        public GeneralResponse AddProduct(AddProductRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Product product = new Product();
                product.ID = Guid.NewGuid();
                product.CreateDate = PersianDateTime.Now;
                product.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                product.ProductCategory = this._productCategoryRepository.FindBy(request.ProductCategoryID);
                product.ProductCode = request.ProductCode;
                product.ProductName = request.ProductName;
                product.UnitsInStock = 0; //request.UnitsInStock;
                product.Note = request.Note;
                product.Discontinued = request.Discontinued;

                #region Validation
                if (product.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in product.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }
                #endregion

                product.RowVersion = 1;

                _productRepository.Add(product);
                _uow.Commit();

                ////response.success = true;
            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }


        //Added By Zafari
        

        #endregion

        #region Edit
        public GeneralResponse EditProduct(IEnumerable<EditProductRequestOld> requests, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            foreach (var request in requests)
            {
                Product product = new Product();
                product = _productRepository.FindBy(request.ID);

                if (product != null)
                {
                    try
                    {
                        product.ModifiedDate = PersianDateTime.Now;
                        product.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                        product.ProductCode = request.ProductCode;
                        if (request.ProductName != null)
                            product.ProductName = request.ProductName;
                        if (request.Note != null)
                            product.Note = request.Note;
                        product.Discontinued = request.Discontinued;

                        if (product.RowVersion != request.RowVersion)
                        {

                            response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                            return response;
                        }
                        else
                        {
                            product.RowVersion += 1;
                        }

                        if (product.GetBrokenRules().Count() > 0)
                        {

                            foreach (BusinessRule businessRule in product.GetBrokenRules())
                            {
                                response.ErrorMessages.Add(businessRule.Rule);
                            }

                            return response;
                        }

                        _productRepository.Save(product);

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
            }

            try
            {
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        

        public GeneralResponse EditProduct(EditProductRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();
            Product product = new Product();
            product = _productRepository.FindBy(request.ID);

            if (product != null)
            {
                try
                {
                    product.ModifiedDate = PersianDateTime.Now;
                    product.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if (request.ProductCategoryID != product.ProductCategory.ID)
                        product.ProductCategory = this._productCategoryRepository.FindBy(request.ProductCategoryID);
                    product.ProductCode = request.ProductCode;
                    if (request.ProductName != null)
                        product.ProductName = request.ProductName;
                    //product.UnitsInStock = request.UnitsInStock;
                    if (request.Note != null)
                        product.Note = request.Note;
                    product.Discontinued = request.Discontinued;

                    if (product.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        product.RowVersion += 1;
                    }

                    if (product.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in product.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _productRepository.Save(product);
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
        public GeneralResponse DeleteProduct(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            Product product = new Product();
            product = _productRepository.FindBy(request.ID);

            if (product != null)
            {
                try
                {
                    _productRepository.Remove(product);
                    _uow.Commit();
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                }
            }

            return response;
        }

        public GeneralResponse DeleteProduct(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            foreach (var request in requests)
            {
                Product product = new Product();
                product = _productRepository.FindBy(request.ID);

                if (product != null)
                {
                    try
                    {
                        _productRepository.Remove(product);
                    }
                    catch (Exception ex)
                    {
                        response.ErrorMessages.Add(ex.Message);
                    }
                }
            }

            try
            {
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        
        #endregion

        #region Get One
        public GetProductResponse GetProduct(GetRequest request)
        {
            GetProductResponse response = new GetProductResponse();

            try
            {
                Product product = new Product();
                ProductView productView = new ProductView();

                product = _productRepository.FindBy(request.ID);
                if (product != null)
                    productView = product.ConvertToProductView();

                response.ProductView = productView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        #endregion

        #region Get All

        // Added By zafari



              
        public GetProductsResponse GetProducts(AjaxGetRequest request)
        {
            GetProductsResponse response = new GetProductsResponse();

            try
            {
                int index = (request.PageNumber - 1) * request.PageSize;
                int count = request.PageSize;

                Infrastructure.Domain.Response<Product> productsResponse = _productRepository
                    .FindAll(index, count);

                IEnumerable<ProductView> products = productsResponse.data
                    .ConvertToProductViews();

                response.ProductViews = products;
                response.Count = productsResponse.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
        

        #endregion

        #region New Methods 

        #region Read

        #region Read One

        public GetGeneralResponse<ProductView> GetProduct(Guid ProductID)
        {
            GetGeneralResponse<ProductView> response=new GetGeneralResponse<ProductView>();
            try
            {
                Product product = new Product();
                product = _productRepository.FindBy(ProductID);

                response.data = product.ConvertToProductView();
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

        #region Read Al whith paging

        public GetGeneralResponse<IEnumerable<ProductView>> GetProducts(bool onlyNotDiscontinued, int PageSize, int PageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<ProductView>> response = new GetGeneralResponse<IEnumerable<ProductView>>();
            try
            {
                int index = (PageNumber - 1) * PageSize;
                int count = PageSize;

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                Response<Product> Products = new Response<Product>();

                if (onlyNotDiscontinued)
                {
                    Criterion CriteriaDiscontinued = new Criterion("Discontinued", false, CriteriaOperator.Equal);
                    query.Add(CriteriaDiscontinued);

                    Products = _productRepository.FindBy(query, index, count,sort);
                }
                else
                {
                    Products = _productRepository.FindAllWithSort(index, count,sort);
                }

                response.data = Products.data.ConvertToProductViews();
                response.totalCount = Products.totalCount;

                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
                return response;
            }

        }

        public GetGeneralResponse<IEnumerable<ProductView>> GetProducts(int PageSize, int PageNumber)
        {
            GetGeneralResponse<IEnumerable<ProductView>> response = new GetGeneralResponse<IEnumerable<ProductView>>();
            try
            {
                int index = (PageNumber - 1) * PageSize;
                int count = PageSize;

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                Response<Product> Products = new Response<Product>();

                Products = _productRepository.FindAll(index, count);


                response.data = Products.data.ConvertToProductViews();
                response.totalCount = Products.totalCount;

                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
                return response;
            }

        }

        #endregion

        #region Read All without paging

        public GetProductsResponse GetProducts()
        {
            GetProductsResponse response = new GetProductsResponse();

            try
            {
                IEnumerable<ProductView> products = _productRepository.FindAll()
                    .ConvertToProductViews();

                response.ProductViews = products;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion

        #endregion

        #region Insert

        public GeneralResponse AddProduct(IEnumerable<AddProductRequest> requests, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (AddProductRequest request in requests)
                {
                    Product product = new Product();
                    product.ID = Guid.NewGuid();
                    product.CreateDate = PersianDateTime.Now;
                    product.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    product.ProductCode = request.ProductCode;
                    product.ProductName = request.ProductName;
                    product.UnitsInStock = 0; //request.UnitsInStock;
                    product.Note = request.Note;

                    #region Validation
                    if (product.GetBrokenRules().Count() > 0)
                    {


                        foreach (BusinessRule businessRule in product.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }
                    #endregion

                    product.RowVersion = 1;

                    _productRepository.Add(product);
                }
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
                return response;
            }
            return response;
        }

        #endregion

        #region Edit

        public GeneralResponse EditProducts(IEnumerable<EditProductRequest> requests, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                foreach (EditProductRequest request in requests)
                {
                    Product product = new Product();

                    product = _productRepository.FindBy(request.ID);

                    product.ModifiedDate = PersianDateTime.Now;
                    product.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                    product.ProductCode = request.ProductCode;
                    if (request.ProductName != null)
                        product.ProductName = request.ProductName;
                    //product.UnitsInStock = request.UnitsInStock;
                    if (request.Note != null)
                        product.Note = request.Note;
                    product.Discontinued = request.Discontinued;

                    if (product.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        product.RowVersion += 1;
                    }

                    if (product.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in product.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }
                    _productRepository.Save(product);
                }


                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }
            return response;
        }

        #endregion

        #region Delete

        public GeneralResponse DeleteProducts(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (DeleteRequest request in requests)
                {
                    Product product = new Product();
                    product = _productRepository.FindBy(request.ID);

                    _productRepository.Remove(product);
                }
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


        #endregion

        #region Moving

        public MoveResponse MoveUp(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            // Current Price is the Request
            Product currentProduct = new Product();
            currentProduct = _productRepository.FindBy(request.ID);

            // Find the Previews Price
            Product previewsProduct= new Product();
            try
            {
                previewsProduct = _productRepository.FindAll()
                                .Where(s => s.SortOrder < currentProduct.SortOrder)
                                .OrderByDescending(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (currentProduct != null && previewsProduct != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = (int)currentProduct.SortOrder;
                    int previews = (int)previewsProduct.SortOrder;

                    currentProduct.SortOrder = previews;
                    previewsProduct.SortOrder = current;

                    _productRepository.Save(currentProduct);
                    _productRepository.Save(previewsProduct);
                    _uow.Commit();
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }

                ////response.success = true;
            }

            return response;
        }

        public MoveResponse MoveDown(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            // Current Price is the Request
            Product currentProduct = new Product();
            currentProduct = _productRepository.FindBy(request.ID);

            // Find the Previews Price
            Product nextProduct= new Product();
            try
            {
                nextProduct = _productRepository.FindAll()
                                .Where(s => s.SortOrder > currentProduct.SortOrder)
                                .OrderBy(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (currentProduct != null && nextProduct != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = (int)currentProduct.SortOrder;
                    int previews = (int)nextProduct.SortOrder;

                    currentProduct.SortOrder = previews;
                    nextProduct.SortOrder = current;

                    _productRepository.Save(currentProduct);
                    _productRepository.Save(nextProduct);
                    _uow.Commit();
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }

                ////response.success = true;
            }

            return response;
        }
        #endregion

        #endregion

    }
}
