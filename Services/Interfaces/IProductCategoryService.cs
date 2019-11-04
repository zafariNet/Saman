using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.StoreCatalogService;
using Services.Messaging;

namespace Services.Interfaces
{
    public interface IProductCategoryService
    {
        GeneralResponse AddProductCategory(AddProductCategoryRequest request);
        GeneralResponse EditProductCategory(EditProductCategoryRequest request);
        GeneralResponse DeleteProductCategory(DeleteRequest request);
        GetProductCategoryResponse GetProductCategory(GetRequest request);
        GetProductCategorysResponse GetProductCategorys();
    }
}
