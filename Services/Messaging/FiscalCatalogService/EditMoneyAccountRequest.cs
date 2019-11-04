using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.FiscalCatalogService
{
    public class EditMoneyAccountRequestOld : AddMoneyAccountRequestOld
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

        public Guid ModifiedEmployeeID { get; set; }
    }

    public class EditMoneyAccountRequest : AddMoneyAccountRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

    }
}
