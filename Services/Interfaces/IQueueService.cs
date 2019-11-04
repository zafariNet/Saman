using Infrastructure.Querying;
using Services.Implementations;
using Services.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;

namespace Services.Interfaces
{
    public interface IQueueService
    {

        GeneralResponse GetQueuesFromAsterisk();
        GeneralResponse QueueUpdatePersianName(IEnumerable<EditQueueRequest> requests);

        GetGeneralResponse<IEnumerable<QueueView>> GetAllQueue(int pageSize, int pageNumber, IList<FilterData> filter,
            IList<Sort> sort);
    }
}
