using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class AddLocalPhoneRequest
    {
        public string LocalPhoneNumber { get; set; }
        public Guid OwnerEmployeeID { get; set; }
    }
}
