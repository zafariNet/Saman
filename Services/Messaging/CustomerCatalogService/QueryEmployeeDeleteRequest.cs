using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class QueryEmployeeDeleteRequest
    {
        public Guid QueryID { get; set; }
        public Guid EmployeeID { get; set; }
    }
}
