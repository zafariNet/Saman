using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Employees;

namespace Services.Interfaces
{
    public interface IMessageTemplateService
    {
        GetGeneralResponse<IEnumerable<MessageTemplateView>> GetMessageTemplates(int? MessageType, int PageSize,
            int PageNumber);

        GeneralResponse EditMessageTemplates(IEnumerable<EditeMessageTemplateRequest> requests, Guid ModifiedEmployeeID);

        GeneralResponse AddMessageTemplates(AddMessageTemplateRequest request,Guid CreateEmployeeID);

        GeneralResponse AddEmailToMessageTemplate(Guid MessageTemplateID, string Message,Guid ModifiedEmployeeID);
        GeneralResponse AddSmsToMessageTemplate(Guid MessageTemplateID, string Message, Guid ModifiedEmployeeID);

        GeneralResponse DeleteMessageTemplate(IEnumerable<DeleteRequest> requests);
    }
}
