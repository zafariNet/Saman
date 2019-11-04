#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Services.ViewModels.Customers;

#endregion

namespace Services.Interfaces
{
    public interface ILevelTypeService
    {
        GeneralResponse AddLevelType(AddLevelTypeRequestOld request);

        GeneralResponse EditLevelType(EditLevelTypeRequestOld request);

        GeneralResponse DeleteLevelType(DeleteRequest request);

        GetLevelTypeResponse GetLevelType(GetRequest request);

        GetGeneralResponse<IEnumerable<LevelTypeView>> GetLevelTypes(AjaxGetRequest getRequest);

        GetLevelTypesResponse GetLevelTypes();



        GetGeneralResponse<IEnumerable<LevelTypeView>> GetLevelTypes(int pageSize , int pageNumber);
        GeneralResponse AddLevelTypes(IEnumerable<AddLevelTypeRequest> requests, Guid EmployeeID);
        GeneralResponse EditleveTypes(IEnumerable<EditLevelTypeRequest> requests, Guid EmployeeID);
        GeneralResponse DeleteLeveleTypes(IEnumerable<DeleteRequest> requests);


    }
}
