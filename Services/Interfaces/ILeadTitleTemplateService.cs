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
    public interface ILeadTitleTemplateService
    {
        GetGeneralResponse<IEnumerable<LeadTitleTemplateView>> GetLeadTitleTemplates(int pageSize, int pageNumber, IList<FilterData> filter, IList<Sort> sort);

        GeneralResponse AddLeadTitleTemplate(IEnumerable<AddLeadTitleTemplateRequest> requests, Guid EmployeeID);
        GeneralResponse EditLeadTitleTemplate(IEnumerable<EditLeadTitleTemplateRequest> requests, Guid employeeID);
        GeneralResponse DeleteLeadTitleTemplate(IEnumerable<DeleteRequest> requests);
    }
}
