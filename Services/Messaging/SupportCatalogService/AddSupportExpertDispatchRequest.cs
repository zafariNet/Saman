using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SupportCatalogService
{
    public class AddSupportExpertDispatchRequest
    {

        public Guid SupportID { get; set; }

        public Guid SupportStatusID { get; set; }

        public string DispatchDate { get; set; }

        public string DispatchTime { get; set; }

        public Guid ExpertEmployeeID { get; set; }

        public string Comment { get; set; }

        public bool SendNotificationToCustomer { get; set; }

        public string CoordinatorName { get; set; }

        public bool IsNewInstallation { get; set; }
    }
}
