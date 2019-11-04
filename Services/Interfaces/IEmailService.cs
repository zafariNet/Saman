using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Infrastructure.Querying;

namespace Services.Interfaces
{
    public interface IEmailService
    {
        GeneralResponse AddEmail(AddEmailRequest request);
        GeneralResponse EditEmail(EditEmailRequest request);
        GeneralResponse DeleteEmail(DeleteRequest request);
        GetEmailResponse GetEmail(GetRequest request);
        GetEmailsResponse GetEmails();

        GetEmailsResponse GetEmails(AjaxGetRequest request,IList<Sort> sort);
    }
}
