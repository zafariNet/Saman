using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Services.ViewModels.Customers;

namespace Services.Interfaces
{
    public interface IQueryService
    {
        GeneralResponse AddQuery(AddQueryRequestOld request);
        GeneralResponse EditQuery(EditQueryRequestOld request);
        GeneralResponse DeleteQuery(DeleteRequest request);
        GetQueryResponse GetQuery(GetRequest request);

        GetQueriesResponse GetQueries(GetQueriesRequest getRequest);
        GetQueriesResponse GetQueries(AjaxGetRequest getRequest);



        GetGeneralResponse<QueryView> GetQuery(Guid QueryID);
        GetGeneralResponse<IEnumerable<QueryView>> GetQueries(int pageSize, int pageNumber);
        GeneralResponse AddQuery(AddQueryRequest request, Guid CreateEmployeeID);
        GeneralResponse EditQuery(EditQueryRequest request, Guid ModifiedEmployeeID);
        GeneralResponse DeleteQuery(IEnumerable<DeleteRequest> requests);
    }
}
