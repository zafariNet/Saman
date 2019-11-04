using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Fiscals;

namespace Services.Messaging.FiscalCatalogService
{
    public class AddFiscalRequest
    {
        public Guid CustomerID { get; set; }
        public Guid MoneyAccountID { get; set; }
        public string DocumentSerial { get; set; }
        public DocType DocumentType { get; set; }
        public long Cost { get; set; }
        public string InvestDate { get; set; }
        public string Note { get; set; }
        public long FollowNumber { get; set; }
        public long SerialNumber { get; set; }
        public bool ForCharge { get; set; }
        public Guid CreateEmployeeID { get; set; }
        public string Phone { get; set; }
    }
}