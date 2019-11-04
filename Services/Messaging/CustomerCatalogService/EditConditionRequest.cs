using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class EditConditionRequestOld : AddConditionRequestOld
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

        public Guid ModifiedEmployeeID { get; set; }
    }

    public class EditConditionRequest : AddConditionRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

    }
}
