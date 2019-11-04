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
    public interface ISuctionModeDetailService
    {
        GetGeneralResponse<IEnumerable<SuctionModeDetailview>> GetSuctionModeDetailBySuctionMode(Guid SuctionModeID, int pageSize, int pageNumber,IList<Sort> sort);
        GetGeneralResponse<IEnumerable<SuctionModeDetailview>> GetSuctionModeDetailBySuctionModeAll(Guid SuctionModeID, int pageSize, int pageNumber, IList<Sort> sort);
        GeneralResponse AddSuctionModeDetails(IEnumerable<AddSuctionModeDetailRequest> requests, Guid SuctionModeID,Guid CreateEployeeID);
        GeneralResponse EditSuctionModeDetails(IEnumerable<EditSuctionModeDetailRequest> requests, Guid ModifiedemployeeID);
        GeneralResponse DeleteSuctionModeDetails(IEnumerable<DeleteRequest> requests);

        MoveResponse MoveUp(MoveRequest request);
        MoveResponse MoveDown(MoveRequest request);

    }
}
