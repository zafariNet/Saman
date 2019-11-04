using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Employees;

namespace Services.Interfaces
{
    public interface ICourierEmployeeService
    {
        GetGeneralResponse<IEnumerable<CourierEmployeeView>> GetCourierEmployees();
        
        GeneralResponse AddCourierEmployee(AddCourierEmployeeRequest request, Guid CreateEployeeID);

        GeneralResponse EditCourierEmployee(EditCourierEmployeeRequest request, Guid ModifiedEmployeeID);

        GeneralResponse DeleteCourierEmployee(IEnumerable<DeleteRequest> requests);
    }
}
