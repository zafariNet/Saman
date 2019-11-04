using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Support;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Interfaces
{
    public interface ISupportTicketWaitingService
    {
        GetGeneralResponse<IEnumerable<SupportTicketWaitingView>> GetSupportTicketWaitings();

        GetGeneralResponse<SupportTicketWaitingView> GetSupportTicketWaiting(Guid SupportID);

        GeneralResponse AddSupportTicketWaiting(AddSupportTicketWaitingRequest request, Guid CreateEmployeeID);

        GeneralResponse EditSupportTicketWaiting(EditSupportTicketWaitingRequest request, Guid ModifiedEmployeeID);

        GeneralResponse DeleteSupportTicketWaiting(DeleteRequest request);
    }
}
