using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class AddNotificationRequest
    {

        public string NotificationTitle { get; set; }

        public string NotificationComment { get; set; }

        public IEnumerable<Guid?> ReferedEmployeeIDs { get; set; }

        public IEnumerable<Guid?> ReferedGroupIDs { get; set; }
    }
}
