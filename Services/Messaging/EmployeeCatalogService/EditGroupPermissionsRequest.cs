using Model.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class EditGroupPermissionsRequest
    {
        public Guid GroupID { get; set; }
        public IEnumerable<Permit> Permissions { get; set; }
    }
}
