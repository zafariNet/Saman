using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using System.Collections;
using Services.ViewModels.Customers;
using Infrastructure.Querying;

namespace Services.Interfaces
{
    public interface IAgencyService
    {
        GeneralResponse AddAgency(AddAgencyRequestOld request);
        GeneralResponse EditAgency(EditAgencyRequestOld request);
        GeneralResponse DeleteAgency(DeleteRequest request);
        GetAgencyResponse GetAgency(GetRequest request);
        GetGeneralResponse<IEnumerable<AgencyView>> GetAgencies();
        MoveResponse MoveUp(MoveRequest request);
        MoveResponse MoveDown(MoveRequest request);

        GetGeneralResponse<IEnumerable<AgencyView>> GetAgencies(bool? Discontinued,int pageSize, int pageNumber,IList<Sort> sort,IList<FilterData> filter);

        GetGeneralResponse<IEnumerable<AgencyView>> GetActiveAgencies();

        GeneralResponse AddAgency(AddAgencyRequest request, Guid createEmployeeID);

        GeneralResponse EditAgency(EditAgencyRequest request, Guid modifiedEmployeeID);

        GeneralResponse EditAgencies(IEnumerable<EditAgencyRequest> Agencies, Guid ModifiedemployeeID);

        GeneralResponse DeleteAgency(IEnumerable<DeleteRequest> requests);
    }
}
