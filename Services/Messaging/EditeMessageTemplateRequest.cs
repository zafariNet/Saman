using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;

namespace Services.Messaging
{
    public class EditeMessageTemplateRequest : AddMessageTemplateRequest
    {
        public int RowVersion { get; set; }
        public Guid ID { get; set; }
    }
}
