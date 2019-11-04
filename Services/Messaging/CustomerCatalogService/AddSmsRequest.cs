using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddSmsRequest
    {
        public Guid CustomerID { get; set; }
        public string Body { get; set; }
        public string Note { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }
}
