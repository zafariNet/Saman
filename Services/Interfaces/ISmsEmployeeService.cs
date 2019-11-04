using Infrastructure.Querying;
using Services.Messaging;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Interfaces
{
    public interface ISmsEmployeeService
    {
        GetGeneralResponse<IEnumerable<SmsEmployeeView>> GetSmsEmployeeByOwner(Guid EmployeeID, int pageSize, int pageNumber);
        GetGeneralResponse<IEnumerable<SmsEmployeeView>> GetSmsEmployees(IList<FilterData> filter,IList<Sort> sort,int pageSize, int pageNumber);
    }
}
