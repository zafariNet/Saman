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
    public interface IUncreditServiceService
    {
        GeneralResponse AddUncreditService(AddUncreditServiceRequestOld request);
        GeneralResponse EditUncreditService(EditUncreditServiceRequestOld request);
        GeneralResponse DeleteUncreditService(DeleteRequest request);
        GetUncreditServiceResponse GetUncreditService(GetRequest request);
        GetUnCreditServicesResponse GetUncreditServices();
        MoveResponse MoveUp(MoveRequest request);
        MoveResponse MoveDown(MoveRequest request);

        GetGeneralResponse<IEnumerable<UncreditServiceView>> GetUncreditServices(int pageSize, int pageNumber,IList<Sort> sort);
        GetGeneralResponse<IEnumerable<UncreditServiceView>> GetUncreditServices(int pageSize, int pageNumber);
        GeneralResponse AddUncreditServices(AddUncreditServiceRequest request, Guid EmployeeID);
        GeneralResponse EditUncreditServices(EditUncreditServiceRequest request, Guid EmployeeID);
        GeneralResponse DeleteUncreditServices(IEnumerable<DeleteRequest> requests);
    }
}
