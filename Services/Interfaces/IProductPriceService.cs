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
    public interface IProductPriceService
    {
        GeneralResponse AddProductPrice(AddProductPriceRequestOld request);
        GeneralResponse EditProductPrice(EditProductPriceRequestOld request);
        GeneralResponse DeleteProductPrice(DeleteRequest request);
        GetProductPriceResponse GetProductPrice(GetRequest request);
        GetProductPricesResponse GetProductPrices();
        GetProductPricesResponse GetProductPrices(Guid ProductID);
        MoveResponse MoveUp(MoveRequest request);
        MoveResponse MoveDown(MoveRequest request);



        GetGeneralResponse<IEnumerable<ProductPriceView>> GetProductPrices(Guid ProductID,int pageSize, int pageNumber,IList<Sort> sort);
        GetGeneralResponse<IEnumerable<ProductPriceView>> GetProductPrices(int pageSize, int pageNumber);
        GeneralResponse AddProductPrices(IEnumerable<AddProductPriceRequest> requests, Guid CreateEmployeeID,Guid ProductID);
        GeneralResponse EditProductPrices(IEnumerable<EditProductPriceRequest> requests, Guid ModifiedEmployeeID,Guid ProductID);
        GeneralResponse DeleteProductPrices(IEnumerable<DeleteRequest> requests);
    }
}
