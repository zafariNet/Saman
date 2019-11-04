using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Customers;

namespace Services.Interfaces
{
    public interface INetworkCenterPriorityService
    {
        GetGeneralResponse<IEnumerable<NetworkCenterPriorityView>> GetNetworkCenter(Guid CenterID);
        
        //GeneralResponse DeleteNetworkCenterPriority(IEnumerable<DeleteRequest> requests);
        GeneralResponse UpdateAll();
        MoveResponse MoveUp(MoveRequest request);
        MoveResponse MoveDown(MoveRequest request);
    }
}
