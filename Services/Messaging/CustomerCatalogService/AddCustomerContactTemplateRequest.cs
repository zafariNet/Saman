using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddCustomerContactTemplateRequest
    {
        public Guid GroupId { get; set; }
        public string Title { get; set; }
    }
}
