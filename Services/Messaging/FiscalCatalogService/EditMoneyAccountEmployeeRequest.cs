using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.FiscalCatalogService
{
    public class EditMoneyAccountEmployeeRequest : AddMoneyAccountEmployeeRequestOld
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

        public Guid ModifiedEmployeeID { get; set; }
    }
}
