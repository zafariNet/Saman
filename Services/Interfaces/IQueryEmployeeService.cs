#region Usings
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;
#endregion

namespace Services.Interfaces
{
    public interface IQueryEmployeeService
    {
        GeneralResponse AddQueryEmployee(AddQueryEmployeeRequestOld request);
        GeneralResponse EditQueryEmployee(EditQueryEmployeeRequest request);
        GeneralResponse DeleteQueryEmployee(DeleteRequest request);
        GetQueryEmployeeResponse GetQueryEmployee(GetRequest request);
        GetQueryEmployeesResponse GetQueryEmployees();
        GetQueryEmployeesResponse GetQueryEmployees(Guid queryID);
        GeneralResponse DeleteQueryEmployee(Guid queryID, Guid employeeID);

        GetGeneralResponse<IEnumerable<QueryEmployeeView>> GetQueryEmployees(Guid QueryID,int pageSize , int pageNumber);
        GeneralResponse AddQueryEmployees(Guid QueryID,IEnumerable<AddQueryEmployeeRequest> requests, Guid CreateEmployeeID);
        GeneralResponse DeleteQueryEmployees(IEnumerable<QueryEmployeeDeleteRequest> requests);
    }
}