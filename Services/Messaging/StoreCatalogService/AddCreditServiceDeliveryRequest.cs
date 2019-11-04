using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{
    public class AddCreditServiceDeliveryRequest
    {
        public Guid CreditServiceID { get; set; }
        public Guid CreditSaleDetailID { get; set; }
        public int Units { get; set; }
        public bool ExitedFromStore { get; set; }
        public string ExitDate { get; set; }
        public Guid EmployeeID { get; set; }
        public string Note { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }
}
