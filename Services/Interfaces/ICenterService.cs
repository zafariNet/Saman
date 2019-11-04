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
    public interface ICenterService
    {
        GeneralResponse EditCenter(IEnumerable<EditCenterRequest> requests, Guid ModifiedEmployeeID);
        GeneralResponse DeleteCenter(IEnumerable<DeleteRequest> requests);
        GetCenterResponse GetCenter(GetRequest request);
        GetCentersResponse GetCenters(IList<Sort> sort);
        GetCentersResponse GetCenters();
        GetCenterInfoResponse GetCenterInfo(string adslPhone, int codeLength);
        GetGeneralResponse<IEnumerable<CenterView>> GetCenters(int pageSize, int pageNumber,string CenterName,IList<Sort> sort,IList<FilterData> filter);

        GeneralResponse AddCenter(IEnumerable<AddCenterRequest> requests, Guid CreateEmployeeID);

        GetGeneralResponse<IEnumerable<NetworkCenterView>> GetCoverage(Guid centerID, int pageSize, int pageNumber,IList<Sort> sort);

        GeneralResponse EditCoverage(IEnumerable<NetworkCenterView> requests , Guid CenterID);
    }
}
