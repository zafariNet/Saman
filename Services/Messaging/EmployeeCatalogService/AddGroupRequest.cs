using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class AddGroupRequest
    {
        public string GroupName { get; set; }
        public string Permissions { get; set; }
        public Guid ParentGroupID { get; set; }
        public Guid GroupStaffID { get; set; }
        //public Guid EmployeesID { get; set; }
        public Guid CreateEmployeeID { get; set; }

        
    }
}
