using Model.Fiscals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.FiscalCatalogService
{
    public class EditFiscalRequest : AddFiscalRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }
        public ChargeStatus ChargeStatus { get; set; }

        public Guid ModifiedEmployeeID { get; set; }
    }
}
