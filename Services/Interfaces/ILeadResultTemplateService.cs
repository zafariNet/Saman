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
    public interface ILeadResultTemplateService
    {
        GetGeneralResponse<IEnumerable<LeadResultTemplateView>> GetLeadResultTemplates(int pageSize, int pageNumber, IList<FilterData> filter, IList<Sort> sort);
        GeneralResponse AddLeadResultTemplate(IEnumerable<AddLeadResultTemplateRequest> requests, Guid EmployeeID);
        GeneralResponse EditLeadResultTemplate(IEnumerable<EditLeadResultTemplateRequest> requests, Guid EmployeeID);
        GeneralResponse DeleteLeadResultTemplate(IEnumerable<DeleteRequest> requests);
    }
}
