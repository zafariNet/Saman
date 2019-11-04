using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class EditSuctionModeDetailRequest:AddSuctionModeDetailRequest
    {
        public Guid ID { get; set; }
        public int Rowversion { get; set; }
    }
}
