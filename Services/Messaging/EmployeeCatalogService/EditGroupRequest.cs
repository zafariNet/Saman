using Model.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class EditGroupRequest : AddGroupRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

        public IEnumerable<Permit> Permissions { get; set; }
    }
}
