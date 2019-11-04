using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class EditLevelTypeRequestOld : AddLevelTypeRequestOld
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

        public Guid ModifiedEmployeeID { get; set; }
    }

    public class EditLevelTypeRequest : AddLevelTypeRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

    }
}
