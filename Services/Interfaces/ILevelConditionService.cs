using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Model.Customers;
using Services.ViewModels.Customers;

namespace Services.Interfaces
{
    public interface ILevelConditionService
    {
        GeneralResponse AddLevelCondition(AddLevelConditionRequest request);
        GeneralResponse EditLevelCondition(EditLevelConditionRequest request);
        GeneralResponse DeleteLevelCondition(DeleteRequest2 request);
        GetLevelConditionResponse GetLevelCondition(GetRequest2 request);
        GetLevelConditionsResponse GetLevelConditions();
        GetLevelConditionsResponse GetLevelConditions(Guid guid);
        GetGeneralResponse<IEnumerable<LevelConditionView>> GetLevelConditions(AjaxGetRequest request);

        GeneralResponse AddLevelConditions(IEnumerable<AddLevelConditionRequest> requests);

        GeneralResponse DeleteLevelConditions(IEnumerable<DeleteRequest2> deleteRequests);

    }
}
