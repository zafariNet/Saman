using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class SendSmsRequest
    {
        string Mobile1 { get; set; }
        Guid ID { get; set; }
    }
}
