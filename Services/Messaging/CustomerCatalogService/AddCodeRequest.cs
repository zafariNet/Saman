using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddCodeRequestOld
    {
        public string CodeName { get; set; }
        public Guid CenterID { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }
    public class AddCodeRequest
    {
        public string CodeName { get; set; }
        public Guid CenterID { get; set; }
    }
}
