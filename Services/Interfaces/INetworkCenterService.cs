using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Services.ViewModels.Customers;

namespace Services.Interfaces
{
    public interface INetworkCenterService
    {
        GeneralResponse AddNetworkCenter(AddNetworkCenterRequest request);
        GeneralResponse EditNetworkCenter(EditNetworkCenterRequest request);
        GeneralResponse DeleteNetworkCenter(DeleteRequest2 request);
        GetNetworkCenterResponse GetNetworkCenter(GetRequest2 request);
        GetNetworkCentersResponse GetNetworkCenters(GetNetworkCentersRequest request);

        
    }
}
