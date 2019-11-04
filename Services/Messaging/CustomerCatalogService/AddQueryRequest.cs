using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddQueryRequestOld
    {
        public string Title { get; set; }
        public string QueryText { get; set; }
        public string xType { get; set; }
        public string PrmDefinition { get; set; }
        public string PrmValues { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }

    public class AddQueryRequest
    {
        public string Title { get; set; }
        public string QueryText { get; set; }
        public string xType { get; set; }
        public string PrmDefinition { get; set; }
        public string PrmValues { get; set; }
    }

}
