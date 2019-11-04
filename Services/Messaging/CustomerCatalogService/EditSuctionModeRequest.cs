using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class EditSuctionModeRequestOld : AddSuctionModeRequestOld
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

        public Guid ModifiedEmployeeID { get; set; }
    }

    public class EditSuctionModeRequest : AddSuctionModeRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }
    }

}
