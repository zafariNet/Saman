using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Fiscals;

namespace Services.Messaging.FiscalCatalogService
{
    public class ConfirmRequest
    {
        public Guid FiscalID { get; set; }
        public long ConfirmedCost { get; set; }
        public Guid ConfirmEmployeeID { get; set; }
        public ConfirmEnum Confirm { get; set; }
        public long RowVersion { get; set; }
        public long FiscalReciptNumber { get; set; }
    }
}
