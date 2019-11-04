using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class AddSmsEmployeerequet
    {
        public Guid OwnerEmployeeID { get; set; }
        public string Body { get; set; }
    }
}
