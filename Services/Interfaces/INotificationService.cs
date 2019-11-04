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
    public interface INotificationService
    {
        GetGeneralResponse<IEnumerable<NotificationView>> NotificationReadByEmployee(Guid ReferedEmployeeID, int pageSize, int pageNumber, IList<FilterData> filter, IList<Sort> sort);
        GetGeneralResponse<IEnumerable<NotificationView>> GetNotifications(IList<FilterData> filter,IList<Sort> sort, int pageSize, int pageNumber);
        GeneralResponse AddNotification(AddNotificationRequest request,Guid EmployeeID);

        NotificationView GetNotification(Guid NotificationID);

        GetGeneralResponse<IEnumerable<NotificationView>> GetNotificationsByCreator(Guid EmployeeID, int pageSize, int pageNumber);
    }
}
