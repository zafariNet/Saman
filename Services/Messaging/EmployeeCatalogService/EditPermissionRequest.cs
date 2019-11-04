using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class EditPermissionRequest
    {
        public EditPermissionRequest()
        {

        }

        public EditPermissionRequest(Guid _employeeID)
        {
            PermitKey = null;
            EmployeeID = _employeeID;
            Guaranteed = false;
        }

        public string PermitKey { get; set; }
        public Guid EmployeeID { get; set; }
        public bool Guaranteed { get; set; }
    }
}
