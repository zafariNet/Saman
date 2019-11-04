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
    public interface ISuctionModeService
    {
        GeneralResponse AddSuctionMode(AddSuctionModeRequestOld request);
        GeneralResponse EditSuctionMode(EditSuctionModeRequestOld request);
        GeneralResponse DeleteSuctionMode(DeleteRequest request);
        GetSuctionModeResponse GetSuctionMode(GetRequest request);
        GetSuctionModesResponse GetSuctionModes();
        MoveResponse MoveUp(MoveRequest request);
        MoveResponse MoveDown(MoveRequest request);

        GetGeneralResponse<IEnumerable<SuctionModeView>> Get_SuctionModes();

        GetGeneralResponse<IEnumerable<SuctionModeView>> Get_SuctionModes(int pageSize, int pageNumber,IList<Sort> sort);
        GeneralResponse AddSuctionModes(IEnumerable<AddSuctionModeRequest> requests, Guid CreateEmployeeID);
        GeneralResponse EditSuctionModes(IEnumerable<EditSuctionModeRequest> requests, Guid ModifiedEmployeeID);
        GeneralResponse DeleteSuctionModes(IEnumerable<DeleteRequest> requests);

    }
}
