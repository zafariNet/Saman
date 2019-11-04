using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Services.ViewModels.Customers;
using Infrastructure.Querying;

namespace Services.Interfaces
{
    public interface IConditionService
    {
        GeneralResponse AddCondition(AddConditionRequestOld request);
        GeneralResponse EditCondition(EditConditionRequestOld request);
        GeneralResponse DeleteCondition(DeleteRequest request);
        GetConditionResponse GetCondition(GetRequest request);
        GetGeneralResponse<IEnumerable<ConditionView>> GetConditions(AjaxGetRequest getRequest);
        GetConditionsResponse GetConditions();

        GetGeneralResponse<IEnumerable<ConditionView>> GetConditions(int pageSize, int pageNumber,IList<Sort> sort);
        GeneralResponse AddConditions(IEnumerable<AddConditionRequest> requests, Guid CreateEmployeeID);
        GeneralResponse EditConditions(IEnumerable<EditConditionRequest> requests, Guid ModifiedEmployeeID);
        GeneralResponse DeleteConditions(IEnumerable<DeleteRequest> requests);
    }
}
