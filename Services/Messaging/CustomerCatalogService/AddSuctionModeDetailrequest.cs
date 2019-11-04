using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddSuctionModeDetailRequest
    {
        public string SuctionModeDetailName { get; set; }
        public bool Discontinued { get; set; }
    }
}
