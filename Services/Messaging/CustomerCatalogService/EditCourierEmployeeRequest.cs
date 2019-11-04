using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class EditCourierEmployeeRequest:AddCourierEmployeeRequest
    {
        public Guid ID { get; set; }

        public int RowVersion { get; set; }
    }
}
