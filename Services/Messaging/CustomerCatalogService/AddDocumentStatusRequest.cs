using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddDocumentStatusRequest
    {
        public string DocumentStatusName { get; set; }
        public bool DefaultStatus { get; set; }
        public bool CompleteStatus { get; set; }
        public int? SortOrder { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }
}
