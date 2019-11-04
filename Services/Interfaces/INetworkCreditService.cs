using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Services.Messaging.StoreCatalogService;
using Services.ViewModels.Store;

namespace Services.Interfaces
{
    public interface INetworkCreditService
    {
        GeneralResponse AddNetworkCredit(AddNetworkCreditRequestOld request);
        GeneralResponse EditNetworkCredit(EditNetworkCreditRequestOld request);
        GeneralResponse DeleteNetworkCredit(DeleteRequest request);
        GetNetworkCreditResponse GetNetworkCredit(GetRequest request);
        GetNetworkCreditsResponse GetNetworkCredits(GetNetworkCreditsRequest request);

        GeneralResponse AddNetworkCredit(AddNetworkCreditRequest request, Guid CreateEmployeeID);
        GeneralResponse EditNetworkCredit(EditNetworkCreditRequest request,Guid ModifiedEmployeeID);
        GeneralResponse DeleteNetworkCredits(IEnumerable<DeleteRequest> requests);
        GetGeneralResponse<NetworkCreditView> GetNetworkCredit(Guid ID);
        GetGeneralResponse<IEnumerable<NetworkCreditView>> GetNetworkCredits(Guid ID);
    }
}
