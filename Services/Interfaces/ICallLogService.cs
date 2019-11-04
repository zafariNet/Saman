
using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure.Querying;
using Services.Messaging;
using Services.ViewModels.Customers;

namespace Services.Interfaces
{
    public interface ICallLogService
    {
        GetGeneralResponse<IEnumerable<CallLogView>> GetAllCallLog(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort);

        GetGeneralResponse<IEnumerable<CallLogView>> GetOwnCallLog(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort, Guid EmployeeID);

        GeneralResponse SetResult(Guid CallLogID, Guid CustomerContactTemplateID, string Description,
            Guid EmployeeID);


        GetGeneralResponse<IEnumerable<CallLogView>> GetCustomerallLog(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort, Guid CustomerID);


    }
}
