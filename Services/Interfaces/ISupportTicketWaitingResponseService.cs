using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Interfaces
{
    public interface ISupportTicketWaitingResponseService
    {
        GetGeneralResponse<IEnumerable<SupportTicketWaitingResponseView>> GetSpportTicketWaitingsRespone();

        GetGeneralResponse<SupportTicketWaitingResponseView> GetSpportTicketWaitingRespone(Guid SupportID);

        GeneralResponse AddSpportTicketWaitingRespone(AddSupportTicketWaitingResponseRequest request,
            Guid CreateemployeeID);

        GeneralResponse EditSpportTicketWaitingRespone(EditSupportTicketWaitingResponseRequest request,
            Guid ModifiedEmployeeID);

        GeneralResponse DeleteSupportTicketWaitingResponse(DeleteRequest request);
    }
}
