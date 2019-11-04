using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddLevelTypeRequestOld
    {
        public string Title { get; set; }
        public Guid CreateEmployeeID { get; set; }
        public bool HasRequireNetwok { get; set; }
    }

    public class AddLevelTypeRequest
    {
        public string Title { get; set; }
        

    }
}
