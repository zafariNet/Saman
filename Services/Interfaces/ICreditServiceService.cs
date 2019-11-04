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
    public interface ICreditServiceService
    {
        GeneralResponse AddCreditService(AddCreditServiceRequestOld request);
        GeneralResponse EditCreditService(EditCreditServiceRequestOld request);
        GeneralResponse DeleteCreditService(DeleteRequest request);
        GetCreditServiceResponse GetCreditService(GetRequest request);
        GetCreditServicesResponse GetCreditServices();
        MoveResponse MoveUp(MoveRequest request);
        MoveResponse MoveDown(MoveRequest request);

        GetGeneralResponse<IEnumerable<CreditServiceView>> GetCreditServices(int pagesize, int pageNumbr, IList<Sort> sort,IList<FilterData> filter);
        GetGeneralResponse<IEnumerable<CreditServiceView>> GetCreditServices(int pageSize, int pageNumber);
        GeneralResponse AddCreditServices(AddCreditServiceRequest request, Guid EmployeeID);
        GeneralResponse EditCreditServices(EditCreditServiceRequest request, Guid EmployeeID);
        GeneralResponse DeleteCreditServices(IEnumerable<DeleteRequest> requests);
    }
}
