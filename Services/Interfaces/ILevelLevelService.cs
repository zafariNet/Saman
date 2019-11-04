using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Services.ViewModels.Customers;

namespace Services.Interfaces
{
    public interface ILevelLevelService
    {
        GeneralResponse AddLevelLevel(AddLevelLevelRequest request);
        GeneralResponse EditLevelLevel(EditLevelLevelRequest request);
        GeneralResponse DeleteLevelLevel(DeleteRequest2 request);
        GetLevelLevelResponse GetLevelLevel(GetRequest request);
        GetGeneralResponse<IEnumerable<LevelLevelView>> GetLevelLevels();
        GetGeneralResponse<IEnumerable<LevelLevelView>> GetLevelLevels(Guid levelID);
        GetGeneralResponse<IEnumerable<LevelLevelView>> GetLevelLevels(AjaxGetRequest request);

        GeneralResponse AddLevelLevels(IEnumerable<AddLevelLevelRequest> requests);

        GeneralResponse DeleteLevelLevels(IEnumerable<DeleteRequest2> requests);
    }
}
