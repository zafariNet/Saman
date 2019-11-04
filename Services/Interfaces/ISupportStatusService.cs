using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Interfaces
{
    public interface ISupportStatusService
    {
        GetGeneralResponse<IEnumerable<SupportStatusView>> GetAllSupportStatuses(int pageSize, int pageNumber);

        GetGeneralResponse<IEnumerable<SupportStatusView>> GetFirstSupportStatuses();

        GetGeneralResponse<SupportStatusView> GetAllSupportStatus(Guid supportStatusID);

        GeneralResponse AddSupportStatus(AddSupportStatusRequest request, Guid CreateEmployeeID);

        GeneralResponse EditSupportStatus(IEnumerable<EditSupportStatusRequest> requests, Guid ModifiedEmployeeID);

        GeneralResponse DeleteSupportStatuses(IEnumerable<DeleteRequest> requests);

        GeneralResponse EditEmail(Guid SupportStatusID, string message, Guid ModifiedEployeeID);

        GeneralResponse EditSms(Guid SupportStatusID, string message, Guid ModifiedEployeeID);
    }
}
