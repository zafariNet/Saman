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
    public interface IQueueLocalPhoneStoreService
    {
        GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreView>> GetAllQueueLocalPhones(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort);

        GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreView>> GetqueueLocalPhoneStoreByEmployee(
            Guid EmployeeID);

        GeneralResponse AddQueueLocalPhoneStore(IEnumerable<AddQueueLocalPhoneRequest> requests, Guid OwneremployeeID,
            Guid EmployeeID);

        GeneralResponse DeleteQueueLocalPhoneStore(IEnumerable<DeleteRequest> requests);

        GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreInfoView1>> GetQueueEmployee(string QueueName,int QueueCount);
    }
}
