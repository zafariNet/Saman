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
    public interface IStoreService
    {
        GeneralResponse AddStore(AddStoreRequestOld request);
        GeneralResponse EditStore(EditStoreRequestOld request);
        GeneralResponse DeleteStore(DeleteRequest request);
        GetStoreResponse GetStore(GetRequest request);
        GetStoresResponse GetStores(AjaxGetRequest request);
        GetStoresResponse GetStores();

        GetGeneralResponse<IEnumerable<StoreView>> GetStores(int pageSize, int pageNumber,IList<Sort> sort);
        GeneralResponse AddStores(IEnumerable<AddStoreRequest> requests , Guid CreateEmployeeID);
        GeneralResponse EditStores(IEnumerable<EditStoreRequest> requests, Guid ModifiedEmployeeID);
        GeneralResponse DeleteStores(IEnumerable<DeleteRequest> requests);
    }
}
