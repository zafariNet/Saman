using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SupportCatalogService
{
    public class AddSupportQcProblemRequest
    {

        public Guid SupportID { get; set; }

        public string InputTime { get; set; }

        public string OutputTime { get; set; }

        public int ExpertBehavior { get; set; }

        public int ExpertCover { get; set; }

        public int SaleAndService { get; set; }

        public long RecivedCost { get; set; }

        public string Comment { get; set; }

        public bool SendNotificationToCustomer { get; set; }

        public string FiscallConfillict { get; set; }

        public Guid InstallerEmployeeID { get; set; }

        public bool Problem { get; set; }
    }
}
