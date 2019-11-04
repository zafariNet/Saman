using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Services.ViewModels.Customers;

namespace Services.Interfaces
{
    public interface ISmsService
    {
        GeneralResponse AddSms(AddSmsRequest request);
        GeneralResponse EditSms(EditSmsRequest request);
        GeneralResponse DeleteSms(DeleteRequest request);
        GetSmsResponse GetSms(GetRequest request);
        GetSmssResponse GetSmss();

        GetGeneralResponse<IEnumerable<SmsView>> GetSmss(Guid customerID, int PageSize, int PageNumber);
    }
}
