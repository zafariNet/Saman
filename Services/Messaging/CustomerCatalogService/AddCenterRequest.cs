using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddCenterRequest
    {
        public string CenterName { get; set; }
        public string Note { get; set; }
    }
}
