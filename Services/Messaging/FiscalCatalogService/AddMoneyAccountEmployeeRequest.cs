using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.FiscalCatalogService
{
    public class AddMoneyAccountEmployeeRequestOld
    {
        public Guid EmployeeID { get; set; }
        public Guid MoneyAccountID { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }

    public class AddMoneyAccountEmployeeRequest
    {
        public Guid EmployeeID { get; set; }
        public Guid MoneyAccountID { get; set; }
    }
}
