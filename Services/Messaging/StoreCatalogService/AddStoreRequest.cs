using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{
    public class AddStoreRequestOld
    {
        public string StoreName { get; set; }
        public Guid OwnerEmployeeID { get; set; }
        public string Note { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }

    public class AddStoreRequest
    {
        public string StoreName { get; set; }
        public Guid OwnerEmployeeID { get; set; }
        public string Note { get; set; }
    }
}
