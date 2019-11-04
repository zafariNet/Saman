using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Querying;
using Services.Messaging;
using Services.Messaging.Leadcatalogservice;
using Services.ViewModels.Leads;

namespace Services.Interfaces
{
    public interface  INegotiationService
    {
        GetGeneralResponse<IEnumerable<NegotiationView>> GetOwnNegotiation(int pageSize, int pageNumber,IList<FilterData> filter,IList<Sort> sort );
        GeneralResponse AddNegotiation(AddNegotiationRequest request, Guid EmployeeID);
        GeneralResponse CloseNegotiation(Guid NegotiationID, Guid EmployeeID, Guid LeadResultTemplateID, string NegotiationResultDescription, int Status);
        GeneralResponse ChangeNegotiationReferedEmployee(IEnumerable<Guid> NegotiationIDs, Guid EmployeeID, Guid ReferedEmployeeID, string NegotiationDate, string NegotiationTime, string RememberDate, string RememberTime);

        GetGeneralResponse<IEnumerable<NegotiationView>> GetChildNegotiations(Guid EmployeeID, int pageSize,
            int pageNumber, IList<FilterData> filter, IList<Sort> sort);

        GeneralResponse CreateNegotiationForCustomers(IEnumerable<Guid> CustomerIDs, Guid ReferedEmployeeID,
            Guid LeadTitleTemplateID,
            string NegotiationDesciption,
            string NegotiationDate,
            string NegotiationTime,
            string RememberDate,
            string RememberTime,
            bool? SendSms, Guid EmployeeID);

        GetGeneralResponse<IEnumerable<NegotiationView>> GetCustomerNegotiations(Guid CustomerID,
            IList<FilterData> filter, IList<Sort> sort, int pageSize, int pageNumber);

        GeneralResponse NegotiationDelay(Guid NegotiationID, Guid EmployeeID, string negotiationDate,
            string NegotiationTime, string RememberDate, string RememberTime);

        GeneralResponse EditNegotiation(EditNegotiationRequest request, Guid EmployeeID);
        GeneralResponse DeleteNegotiations(IEnumerable<DeleteRequest> requests);

    }
}
