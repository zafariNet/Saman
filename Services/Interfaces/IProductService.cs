using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.StoreCatalogService;
using Services.Messaging;
using Services.ViewModels.Store;
using Infrastructure.Querying;

namespace Services.Interfaces
{
    public interface IProductService
    {
        #region Old
        GeneralResponse AddProduct(AddProductRequestOld request);
        GeneralResponse EditProduct(EditProductRequestOld request);
        GeneralResponse DeleteProduct(DeleteRequest request);
        GetProductResponse GetProduct(GetRequest request);
        GetProductsResponse GetProducts(AjaxGetRequest request);
        GetProductsResponse GetProducts();
        #endregion

        #region New Added By zafari

        MoveResponse MoveUp(MoveRequest request);
        MoveResponse MoveDown(MoveRequest request);

        GetGeneralResponse<ProductView> GetProduct(Guid ProductID);

        GeneralResponse AddProduct(IEnumerable<AddProductRequest> requests, Guid CreateEmployeeID);


        GeneralResponse EditProducts(IEnumerable<EditProductRequest> request, Guid ModifiedEmployeeID);

        GeneralResponse DeleteProducts(IEnumerable<DeleteRequest> requests);


        GetGeneralResponse<IEnumerable<ProductView>> GetProducts(bool onlyNotDiscontinued, int pageSize, int pageNumber, IList<Sort> sort);
        GetGeneralResponse<IEnumerable<ProductView>> GetProducts(int pageSize, int pageNumber);

        


        GeneralResponse DeleteProduct(IEnumerable<DeleteRequest> requests);
        
        
        #endregion
    }
}
