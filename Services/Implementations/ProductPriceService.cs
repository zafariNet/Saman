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
    public class ProductPriceService : IProductPriceService
    {
        #region Declares
        private readonly IProductPriceRepository _productPriceRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Ctor
        public ProductPriceService(IProductPriceRepository productPriceRepository, IUnitOfWork uow)
        {
            _productPriceRepository = productPriceRepository;
            _uow = uow;
        }

        public ProductPriceService(IProductPriceRepository productPriceRepository, IProductRepository productRepository, IUnitOfWork uow
                        , IEmployeeRepository employeeRepository)
            : this(productPriceRepository, uow)
        {
            this._productRepository = productRepository;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Old Methods

        #region Add
        public GeneralResponse AddProductPrice(AddProductPriceRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                ProductPrice productPrice = new ProductPrice();
                productPrice.ID = Guid.NewGuid();
                productPrice.CreateDate = PersianDateTime.Now;
                productPrice.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                productPrice.Imposition = request.Imposition;
                productPrice.MaxDiscount = request.MaxDiscount;
                productPrice.Product = this._productRepository.FindBy(request.ProductID);
                productPrice.ProductPriceTitle = request.ProductPriceTitle;
                productPrice.UnitPrice = request.UnitPrice;
                productPrice.Note = request.Note;
                productPrice.Discontinued = request.Discontinued;
                
                productPrice.SortOrder = GetSortOrder();
                productPrice.RowVersion = 1;

                // Validation
                if (productPrice.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in productPrice.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                _productPriceRepository.Add(productPrice);
                _uow.Commit();

                ////response.success = true;

            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        #endregion

        #region Edit
        public GeneralResponse EditProductPrice(EditProductPriceRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();
            ProductPrice productPrice = new ProductPrice();
            productPrice = _productPriceRepository.FindBy(request.ID);

            if (productPrice != null)
            {
                try
                {
                    productPrice.ModifiedDate = PersianDateTime.Now;
                    productPrice.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                        productPrice.Imposition = request.Imposition;
                        productPrice.MaxDiscount = request.MaxDiscount;
                    if (request.ProductID != null)
                        productPrice.Product = this._productRepository.FindBy(request.ProductID);
                    if (request.ProductPriceTitle != null)
                        productPrice.ProductPriceTitle = request.ProductPriceTitle;
                        productPrice.UnitPrice = request.UnitPrice;
                    if (request.Note != null)
                        productPrice.Note = request.Note;
                        productPrice.Discontinued = request.Discontinued;

                    if (productPrice.RowVersion != request.RowVersion)
                    {
                        
                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        productPrice.RowVersion += 1;
                    }

                    if (productPrice.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in productPrice.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _productPriceRepository.Save(productPrice);
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

        #region Delete
        public GeneralResponse DeleteProductPrice(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            ProductPrice productPrice = new ProductPrice();
            productPrice = _productPriceRepository.FindBy(request.ID);

            if (productPrice != null)
            {
                try
                {
                    _productPriceRepository.Remove(productPrice);
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
        public GetProductPriceResponse GetProductPrice(GetRequest request)
        {
            GetProductPriceResponse response = new GetProductPriceResponse();

            try
            {
                ProductPrice productPrice = new ProductPrice();
                ProductPriceView productPriceView = productPrice.ConvertToProductPriceView();

                productPrice = _productPriceRepository.FindBy(request.ID);
                if (productPrice != null)
                    productPriceView = productPrice.ConvertToProductPriceView();

                response.ProductPriceView = productPriceView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get All or Some
        public GetProductPricesResponse GetProductPrices()
        {
            GetProductPricesResponse response = new GetProductPricesResponse();

            try
            {
                IEnumerable<ProductPriceView> productPrices = _productPriceRepository.FindAll().Where(x=>x.Product.Discontinued==false)
                    .OrderBy(o => o.SortOrder)
                    .ConvertToProductPriceViews();

                response.ProductPriceViews = productPrices;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetProductPricesResponse GetProductPrices(Guid productID)
        {
            GetProductPricesResponse response = new GetProductPricesResponse();

            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                Criterion criterion = new Criterion("Product.ID", productID, CriteriaOperator.Equal);
                query.Add(criterion);

                IEnumerable<ProductPriceView> productPrices = _productPriceRepository.FindBy(query)
                    .OrderBy(o => o.SortOrder)
                    .ConvertToProductPriceViews();

                response.ProductPriceViews = productPrices;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

#endregion

        

        #endregion

        #region New Methods

        #region Read

        public GetGeneralResponse<IEnumerable<ProductPriceView>> GetProductPrices(Guid ProductID,int pageSize, int pageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<ProductPriceView>> response = new GetGeneralResponse<IEnumerable<ProductPriceView>>();

            int index = (pageNumber - 1) * pageSize;
            int count = pageSize;

            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                if (ProductID != Guid.Empty)
                {
                    Criterion criteriaProductID = new Criterion("Product.ID", ProductID, CriteriaOperator.Equal);
                    query.Add(criteriaProductID);
                }

                Response<ProductPrice> productPrices = _productPriceRepository.FindBy(query,index, count,sort);
                response.data = productPrices.data.ConvertToProductPriceViews();
                response.totalCount = productPrices.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<ProductPriceView>> GetProductPrices(int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<ProductPriceView>> response = new GetGeneralResponse<IEnumerable<ProductPriceView>>();

            int index = (pageNumber - 1) * pageSize;
            int count = pageSize;

            try
            {

                Response<ProductPrice> productPrices = _productPriceRepository.FindAll(index, count);
                response.data = productPrices.data.ConvertToProductPriceViews();
                response.totalCount = productPrices.totalCount;
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

        public GeneralResponse AddProductPrices(IEnumerable<AddProductPriceRequest> requests, Guid CreateEmployeeID,Guid ProductID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (AddProductPriceRequest request in requests)
                {
                    ProductPrice productPrice = new ProductPrice();

                    if (request.MaxDiscount > request.UnitPrice)
                    {
                        response.ErrorMessages.Add("تخفیف نمیتواند بیش از قیمت پایه باشد");
                        return response;
                    }


                    #region Validate Product Price Code

                    string _errorMessag=AddValidate(request.ProductPriceCode, request.ProductPriceTitle);

                    if (_errorMessag != "NoError")
                    {
                        response.ErrorMessages.Add(_errorMessag);
                        return response;
                    }

                    #endregion

                    productPrice.ID = Guid.NewGuid();
                    productPrice.CreateDate = PersianDateTime.Now;
                    productPrice.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    productPrice.Imposition = request.Imposition;
                    productPrice.MaxDiscount = request.MaxDiscount;
                    productPrice.ProductPriceCode = request.ProductPriceCode;
                    productPrice.Product = this._productRepository.FindBy(ProductID);
                    productPrice.ProductPriceTitle = request.ProductPriceTitle;
                    productPrice.UnitPrice = request.UnitPrice;
                    productPrice.Note = request.Note;
                    productPrice.Discontinued = request.Discontinued;
                    productPrice.Bonus = request.Bonus;
                    productPrice.Comission = request.Comission;
                    productPrice.SortOrder = GetSortOrder();
                    productPrice.RowVersion = 1;

                    // Validation
                    if (productPrice.GetBrokenRules().Count() > 0)
                    {


                        foreach (BusinessRule businessRule in productPrice.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _productPriceRepository.Add(productPrice);

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

        #region Edit

        public GeneralResponse EditProductPrices(IEnumerable<EditProductPriceRequest> requests, Guid ModifiedEmployeeID,Guid ProductID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (EditProductPriceRequest request in requests)
                {
                    ProductPrice productPrice = new ProductPrice();

                    #region Validate Product Price Code

                    string _errorMessag = EditValidate(request.ProductPriceCode, request.ProductPriceTitle,request.ID);

                    if (_errorMessag != "NoError")
                    {
                        response.ErrorMessages.Add(_errorMessag);
                        return response;
                    }

                    #endregion


                    if (request.MaxDiscount > request.UnitPrice)
                    {
                        response.ErrorMessages.Add("تخفیف نمیتواند بیش از قیمت پایه باشد");
                        return response;
                    }

                    productPrice = _productPriceRepository.FindBy(request.ID);
                    productPrice.ModifiedDate = PersianDateTime.Now;
                    productPrice.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                    productPrice.Imposition = request.Imposition;
                    productPrice.MaxDiscount = request.MaxDiscount;
                    productPrice.Comission = request.Comission;
                    productPrice.Bonus = request.Bonus;
                    if (request.ProductID != productPrice.Product.ID)
                        productPrice.Product = this._productRepository.FindBy(ProductID);
                    if (request.ProductPriceTitle != null)
                        productPrice.ProductPriceTitle = request.ProductPriceTitle;
                    productPrice.UnitPrice = request.UnitPrice;
                    productPrice.ProductPriceCode = request.ProductPriceCode;
                    if (request.Note != null)
                        productPrice.Note = request.Note;
                    productPrice.Discontinued = request.Discontinued;

                    if (productPrice.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        productPrice.RowVersion += 1;
                    }

                    if (productPrice.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in productPrice.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _productPriceRepository.Save(productPrice);
                }
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Delet

        public GeneralResponse DeleteProductPrices(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                foreach (DeleteRequest request in requests)
                {
                    
                    _productPriceRepository.RemoveById(request.ID);
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
            ProductPrice currentProductPrice = new ProductPrice();
            currentProductPrice = _productPriceRepository.FindBy(request.ID);

            // Find the Previews Price
            ProductPrice previewsProductPrice = new ProductPrice();
            try
            {
                previewsProductPrice = _productPriceRepository.FindAll()
                                .Where(s => s.SortOrder < currentProductPrice.SortOrder)
                                .OrderByDescending(s => s.SortOrder)
                                .FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            if (currentProductPrice != null && previewsProductPrice != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = (int)currentProductPrice.SortOrder;
                    int previews = (int)previewsProductPrice.SortOrder;

                    currentProductPrice.SortOrder = previews;
                    previewsProductPrice.SortOrder = current;

                    _productPriceRepository.Save(currentProductPrice);
                    _productPriceRepository.Save(previewsProductPrice);
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
            ProductPrice currentProductPrice = new ProductPrice();
            currentProductPrice = _productPriceRepository.FindBy(request.ID);

            // Find the Previews Price
            ProductPrice nextProductPrice = new ProductPrice();
            try
            {
                nextProductPrice = _productPriceRepository.FindAll()
                                .Where(s => s.SortOrder > currentProductPrice.SortOrder)
                                .OrderBy(s => s.SortOrder)
                                .FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            if (currentProductPrice != null && nextProductPrice != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = (int)currentProductPrice.SortOrder;
                    int previews = (int)nextProductPrice.SortOrder;

                    currentProductPrice.SortOrder = previews;
                    nextProductPrice.SortOrder = current;

                    _productPriceRepository.Save(currentProductPrice);
                    _productPriceRepository.Save(nextProductPrice);
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

        #region Private Methods


        private int GetSortOrder()
        {
            try
            {
                IEnumerable<ProductPrice> productPrices = _productPriceRepository.FindAll();
                return productPrices.Max(s => (int)s.SortOrder) + 1;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        private string AddValidate(int code,string title)
        {
            string errorMssage="NoError";

            if (code < 100 || code > 3000)
            {
                errorMssage = "کد محصول باید بزرگتر از 100 و کوچکتر از 3001 باشد";
                return errorMssage;
            }


            IEnumerable<ProductPrice> productPrices = _productPriceRepository.FindAll();

            
            foreach (ProductPrice productPrice in productPrices)
            {
                if (productPrice.ProductPriceCode == code)
                {
                    errorMssage = " این کد محصول قبلا برای " + productPrice.ProductPriceTitle + " ثبت شده است ";
                }
                if (productPrice.ProductPriceTitle == title)
                {
                    errorMssage = " محصولی با نام " + productPrice.ProductPriceTitle + "  قبلا ثبت شده است ";
                }
            }
            return errorMssage;
        }

        private string EditValidate(int code, string title , Guid ID)
        {
            string errorMssage = "NoError";

            if (code < 100 || code > 3000)
            {
                errorMssage = "کد محصول باید بزرگتر از 100 و کوچکتر از 3001 باشد";
                return errorMssage;
            }

            IEnumerable<ProductPrice> productPrices = _productPriceRepository.FindAll();


            foreach (ProductPrice productPrice in productPrices)
            {
                if (productPrice.ProductPriceCode == code)
                {
                    if(productPrice.ID != ID && productPrice.ProductPriceTitle !=title)
                    errorMssage = " این کد محصول قبلا برای " + productPrice.ProductPriceTitle + " ثبت شده است ";
                }
                if (productPrice.ProductPriceTitle == title && productPrice.ID != ID)
                {
                    errorMssage = " محصولی با نام " + productPrice.ProductPriceTitle + " ثبت شده است ";
                }
            }
            return errorMssage;
        }

        #endregion
    }
}
