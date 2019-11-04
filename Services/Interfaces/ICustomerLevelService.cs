using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Services.ViewModels.Customers;

namespace Services.Interfaces
{
    public interface ICustomerLevelService
    {
        GeneralResponse AddCustomerLevel(AddCustomerLevelRequest request);
        GeneralResponse EditCustomerLevel(EditCustomerLevelRequest request);
        GeneralResponse DeleteCustomerLevel(DeleteRequest request);
        GetCustomerLevelResponse GetCustomerLevel(GetRequest request);
        GetCustomerLevelsResponse GetCustomerLevels();

        GetGeneralResponse<IEnumerable<CustomerLevelView>> GetLevelHistory(Guid customerID, int pageSize, int pageNumber);

        GeneralResponse PrepareToAddCustomerLevel(AddCustomerLevelRequest levelRequest);
        GeneralResponse SendEmail(string recipient, string displayName, string subject, string htmlBody);
    }
}
