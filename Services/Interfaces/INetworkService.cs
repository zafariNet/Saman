using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.StoreCatalogService;
using Services.Messaging;
using Model.Store;
using Services.ViewModels.Store;
using Infrastructure.Querying;

namespace Services.Interfaces
{
    public interface INetworkService
    {
        GeneralResponse AddNetwork(AddNetworkRequestOld request);
        GeneralResponse EditNetwork(EditNetworkRequestOld request);
        GeneralResponse DeleteNetwork(DeleteRequest request);
        GetNetworkResponse GetNetwork(GetRequest request);
        GetNetworksResponse GetNetworks();
        IEnumerable<Network> GetRawNetworks();

        MoveResponse MoveUp(MoveRequest request);
        MoveResponse MoveDown(MoveRequest request);
        GetGeneralResponse<IEnumerable<NetworkSummaryView>> GetNetworks(int pageSize, int pageNumber,IList<Sort> sort);
        GetGeneralResponse<IEnumerable<NetworkSummaryView>> GetNetworks(int pageSize, int pageNumber);
        GeneralResponse AddNetworks(IEnumerable<AddNetworkRequest> requests, Guid CreateEmployeeID);
        GeneralResponse EditNetworks(IEnumerable<EditNetworkRequest> requests, Guid ModifiedEmployeeID);
        GeneralResponse DeleteNetworks(IEnumerable<DeleteRequest> requests);
    }
}
