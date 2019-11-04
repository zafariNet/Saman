

using System;
using System.Collections.Generic;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Customers;

namespace Services.Interfaces
{
    public interface ICustomerContactTemplateService
    {
        GetGeneralResponse<IEnumerable<CustomerContactTemplateView>> GetAll(int pageSize,int pageNumber);
        GetGeneralResponse<IEnumerable<CustomerContactTemplateView>> GetAllByGroup(Guid EmployeeID);

        GeneralResponse AddCustomerContactTemplate(IEnumerable<AddCustomerContactTemplateRequest> requests,
            Guid EmployeeID);

        GeneralResponse EditCustomerContactTemplate(IEnumerable<EditCustomerContactTemplateRequest> requests,
            Guid ModifiedEmployeeID);

        GeneralResponse DeleteCustomerCOntactTemplate(IEnumerable<DeleteRequest> requests);
    }
}
