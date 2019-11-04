using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SupportCatalogService
{
    public class AddSupportQcRequest
    {

        public Guid SupportID { get; set; }

        public Guid SupportStatusID { get; set; }

        public string InputTime { get; set; }

        public string OutputTime { get; set; }

        public int ExpertBehavior { get; set; }

        public int ExpertCover { get; set; }

        public int SaleAndService { get; set; }

        public long RecivedCost { get; set; }

        public string Comment { get; set; }

        public bool SendNotificationToMaster { get; set; }

        public bool SendNotificationToCustomer { get; set; }

        public Guid InstallerEmployeeID { get; set; }
    }
}
