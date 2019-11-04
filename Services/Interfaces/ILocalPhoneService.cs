#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Querying;
using Services.Messaging.EmployeeCatalogService;
using Services.Messaging;
using Services.ViewModels.Employees;
using Services.Messaging.CustomerCatalogService;

#endregion

namespace Services.Interfaces
{
    public interface ILocalPhoneService
    {
        GeneralResponse AddLocalPhone(IEnumerable<AddLocalPhoneRequest> requests, Guid CreateEmployeeID);
        GeneralResponse EditLocalPhone(IEnumerable<EditLocalPhoneRequest> requests,Guid ModifiedEmployeeID);
        GeneralResponse DeleteLocalPhone(IEnumerable<DeleteRequest> requests);
        GetGeneralResponse<LocalPhoneView> GetLocalPhone(GetRequest request);
        GetGeneralResponse<IEnumerable<LocalPhoneView>> GetLocalPhonesByEmployee(Guid employeeID);
        GetGeneralResponse<IEnumerable<LocalPhoneView>> GetLocalPhones();
        GetGeneralResponse<IEnumerable<LocalPhoneView>> GetLocalPhones(int pageSize, int pageNumber, List<FilterData> filter, IList<Sort> sort);

        string[] GetLocalPhoneStr(Guid guid);
    }
}
