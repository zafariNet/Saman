using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Querying;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;

namespace Services.Interfaces
{
    public interface ICampaignAgentService
    {
        GetGeneralResponse<IEnumerable<CampaignAgentView>> GetCampaignAgentes(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort);

        GeneralResponse AddCampaignAgent(IEnumerable<AddCampaignAgentRequest> requests, Guid CreateEmployeeID);

        GeneralResponse EditCampaignAgent(IEnumerable<EditCampaignAgentRequest> requests, Guid ModifiedEmployeeID);

        GeneralResponse DeleteCampaignAgenet(IEnumerable<DeleteRequest> requests);
    }
}
