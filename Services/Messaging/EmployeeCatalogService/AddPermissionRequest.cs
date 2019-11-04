using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class AddPermissionRequest
    {
        public string Title { get; set; }
        public string Group { get; set; }
        public string Key { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }
}
