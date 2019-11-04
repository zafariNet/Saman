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

namespace Services.Implementations
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository, IUnitOfWork uow
                    , IEmployeeRepository employeeRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
        }

        public GeneralResponse AddProductCategory(AddProductCategoryRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                ProductCategory productCategory = new ProductCategory();
                productCategory.ID = Guid.NewGuid();
                productCategory.CreateDate = PersianDateTime.Now;
                productCategory.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                productCategory.ProductCategoryName = request.ProductCategoryName;
                productCategory.Note = request.Note;
                productCategory.Discontinued = request.Discontinued;

                productCategory.RowVersion = 1;

                _productCategoryRepository.Add(productCategory);
                _uow.Commit();

                ////response.success = true;

                // Validation
                if (productCategory.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in productCategory.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public GeneralResponse EditProductCategory(EditProductCategoryRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            ProductCategory productCategory = new ProductCategory();
            productCategory = _productCategoryRepository.FindBy(request.ID);

            if (productCategory != null)
            {
                try
                {
                    productCategory.ModifiedDate = PersianDateTime.Now;
                    productCategory.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if (request.ProductCategoryName != null)
                        productCategory.ProductCategoryName = request.ProductCategoryName;
                    if (request.Note != null)
                        productCategory.Note = request.Note;
                        productCategory.Discontinued = request.Discontinued;

                    if (productCategory.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        productCategory.RowVersion += 1;
                    }

                    if (productCategory.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in productCategory.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _productCategoryRepository.Save(productCategory);
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

        public GeneralResponse DeleteProductCategory(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            
                try
                {
                    _productCategoryRepository.RemoveById(request.ID);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                }
        
            return response;
        }

        public GetProductCategoryResponse GetProductCategory(GetRequest request)
        {
            GetProductCategoryResponse response = new GetProductCategoryResponse();

            try
            {
                ProductCategory productCategory = new ProductCategory();
                ProductCategoryView productCategoryView = productCategory.ConvertToProductCategoryView();

                productCategory = _productCategoryRepository.FindBy(request.ID);
                if (productCategory != null)
                    productCategoryView = productCategory.ConvertToProductCategoryView();

                response.ProductCategoryView = productCategoryView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetProductCategorysResponse GetProductCategorys()
        {
            GetProductCategorysResponse response = new GetProductCategorysResponse();

            try
            {
                IEnumerable<ProductCategoryView> productCategorys = _productCategoryRepository.FindAll()
                    .ConvertToProductCategoryViews();

                response.ProductCategoryViews = productCategorys;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

    }
}
