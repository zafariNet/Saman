using Infrastructure.Querying;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Interfaces
{
    public interface ILocalPhoneStoreEmployeeService
    {
        //GeneralResponse GetLocalPhoneStoresFromAsterisk();

        //GetGeneralResponse<LocalPhoneStoreEmployeeView> GetAllLocalPhoneStoreEmployee(int pageSize, int pageNumber, IList<FilterData> filter, IList<Sort> sort);
        GetGeneralResponse<IEnumerable<LocalPhoneStoreEmployeeView>> GetAllLocalPhoneStoreEmployee(int pageSize,
            int pageNumber, IList<FilterData> filter, IList<Sort> sort);

        GetGeneralResponse<IEnumerable<LocalPhoneStoreEmployeeView>> GetLocalPhoneStoreEmployeeByEmployee(
            Guid EmployeeID);

        GeneralResponse AddLocalPhoneStoreEmployee(IEnumerable<AddLocalPhoneStoreEmployeeRequest> requests,Guid OwnerEmployeeID,
            Guid CreateEployeeID);

        GeneralResponse DeleteLocalPhoneStoreEmployee(IEnumerable<DeleteRequest> requests);

    }
}
