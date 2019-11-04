using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.StoreCatalogService;
using Services.Messaging;
using Services.ViewModels.Store;


namespace Services.Interfaces
{
    public interface IStoreProductService
    {
        GeneralResponse AddStoreProduct(AddStoreProductRequest request);
        GeneralResponse EditStoreProduct(EditStoreProductRequest request);
        GeneralResponse DeleteStoreProduct(Guid storeID, Guid productID);
        GetStoreProductResponse GetStoreProduct(Guid storeID, Guid productID);
        GetGeneralResponse<IEnumerable<StoreProductView>> GetStoreProducts(Guid storeID);

        //GetGeneralResponse<IEnumerable<StoreProductView>> GetStoreProducts(Guid StoreID);
        GetGeneralResponse<IEnumerable<StoreProductView>> GetProductStore(Guid ProductID);
       // GetGeneralResponse<IEnumerable<StoreProductView>> GetProductStore(Guid ProductID);
    }

}
