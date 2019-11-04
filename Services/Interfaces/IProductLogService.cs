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
    public interface IProductLogService
    {
        GeneralResponse AddProductLog(AddProductLogRequestOld request);
        GeneralResponse EditProductLog(EditProductLogRequest request);
        GeneralResponse DeleteProductLog(DeleteRequest request);
        GetProductLogResponse GetProductLog(GetRequest request);
        GetProductLogsResponse GetProductLogs(Guid productID);
        GetProductLogsResponse GetProductLogs(Guid productID,Guid StoreID);
        GetProductLogsResponse GetProductLogs();

        GetGeneralResponse<IEnumerable<ProductLogView>> GetProductLogsByProductInStore(Guid ProductID, int PageSize, int PageNumber,IList<Sort> sort);
        GetGeneralResponse<IEnumerable<ProductLogView>> GetProductLogs(int PageSize, int PageNumber);
        GetGeneralResponse<IEnumerable<ProductLogView>> GetProductLogsByFilter(IList<FilterData> filters);
        GetGeneralResponse<IEnumerable<ProductLogView>> GetProductLogsByProduct(Guid ProductID, int PageSize, int PageNumber,IList<Sort> sort);

        GeneralResponse AddProductLog(AddProductLogRequest request, Guid CreateEmployeeID);
        GeneralResponse AddProductLogToStore(AddProductLogStoreRequest request, Guid CreateEmployeeID);

        
    }
}
